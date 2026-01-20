using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TrainController : MonoBehaviour
{
    [SerializeField] private float _speed = 30f;
    private float _currentSpeed = 0f;
    [SerializeField] private float _accelerationTime = 1f;

    public enum Direction { Right, Left }
    public Direction _lastDirection = Direction.Right;

    [SerializeField] private Transform _directionLever;
    private Vector3 _rightLeverPos = new Vector3(0.46f, 0.8125f, 0f);
    private Vector3 _leftLeverPos = new Vector3(-0.469f, 0.8125f, 0f);

    [SerializeField] private Transform _throttleLever;
    private Vector3 _speedOnPos = new Vector3(1.20625f, 0.75f, 0.1875f);
    private Vector3 _speedOffPos = new Vector3(1.20625f, 0.75f, -0.266f);

    [SerializeField] private AudioSource _train;
    [SerializeField] private AudioSource _stop;

    private Coroutine _TrainCoroutine;
    private Coroutine _effectsCoroutine;

    [SerializeField] private GameObject _buttonParent;
    [SerializeField] private GameObject _exitZoomButtonsParent;

    [SerializeField] private AudioSource _leverpower;
    [SerializeField] private AudioSource _leverdir;

    private bool _isrunning = false;

    // ===== DEPTH OF FIELD =====
    [SerializeField] private PostProcessVolume _postProcessVolume;
    private DepthOfField _depthOfField;

    [SerializeField] private float _focalStopped = 8f;
    [SerializeField] private float _focalMoving = 18f;

    // ===== WAYPOINT ROTATION ONLY =====
    [SerializeField] private float _rotationSpeed = 3f;
    private Waypoint _currentWaypoint;
    private float _waypointReachDistance = 1f;
    private bool _isRotatingToWaypoint = false;


    public bool IsMoving => _currentSpeed > 0.01f;
    

    void Start()
    {
        if (_directionLever != null)
            _directionLever.localPosition = _rightLeverPos;

        if (_throttleLever != null)
            _throttleLever.localPosition = _speedOffPos;

        _lastDirection = Direction.Right;
        _currentSpeed = 0f;

        if (_exitZoomButtonsParent != null)
            _exitZoomButtonsParent.SetActive(false);

        // ===== INIT DEPTH OF FIELD =====
        if (_postProcessVolume != null)
        {
            _postProcessVolume.profile.TryGetSettings(out _depthOfField);
            if (_depthOfField != null)
                _depthOfField.focusDistance.value = _focalStopped;
        }
    }

    void Update()
    {
        // ===== WAYPOINT ROTATION (Y ONLY) =====
        if (_currentWaypoint != null && _isRotatingToWaypoint)
        {
            Vector3 targetPos = _currentWaypoint.transform.position;
            Vector3 dir = new Vector3(
                targetPos.x - transform.position.x,
                0,
                targetPos.z - transform.position.z
            ).normalized;

            if (dir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRot,
                    Time.deltaTime * _rotationSpeed
                );
            }

            float distance = Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(targetPos.x, 0, targetPos.z)
            );

            if (distance < _waypointReachDistance)
            {
                if (_currentWaypoint.nextWaypoint != null)
                    _currentWaypoint = _currentWaypoint.nextWaypoint;
                else
                    _isRotatingToWaypoint = false;
            }
        }

        transform.Translate(Vector3.forward * (_currentSpeed * Time.deltaTime));

        // Direction
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetDirection(Direction.Left);
            _leverdir.Play();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SetDirection(Direction.Right);
            _leverdir.Play();
        }

        // Speed
        if (Input.GetKeyDown(KeyCode.W) && !GameManager.Instance._speedzero && !_isrunning)
        {
            _leverpower.Play();
            StartTrain();
            _isrunning = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || GameManager.Instance._speedzero)
        {
            GameManager.Instance._speedzero = false;
            StopTrain();
            _leverpower.Play();
            _isrunning = false;
        }
    }

    public void SetRotationWaypoint(Waypoint waypoint)
    {
        _currentWaypoint = waypoint;
        _isRotatingToWaypoint = true;
    }

    private IEnumerator TrainDepature()
    {
        float elapsed = 0f;
        while (elapsed < _accelerationTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _accelerationTime;
            _currentSpeed = Mathf.Lerp(0f, _speed, t * t);
            yield return null;
        }
    }

    private IEnumerator TrainStop()
    {
        float elapsed = 0f;
        while (elapsed < _accelerationTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _accelerationTime;
            _currentSpeed = Mathf.Lerp(_speed, 0f, t * t);
            yield return null;
        }
    }

    private IEnumerator TransitionDepthOfField(float targetFocal)
    {
        if (_depthOfField == null) yield break;

        float elapsed = 0f;
        float start = _depthOfField.focusDistance.value;

        while (elapsed < _accelerationTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _accelerationTime;
            _depthOfField.focusDistance.value = Mathf.Lerp(start, targetFocal, t);
            yield return null;
        }

        _depthOfField.focusDistance.value = targetFocal;
    }

    private void StartTrain()
    {
        if (_TrainCoroutine != null)
            StopCoroutine(_TrainCoroutine);
        _TrainCoroutine = StartCoroutine(TrainDepature());

        _train.Play();
        _stop.Stop();

        if (_throttleLever != null)
            _throttleLever.localPosition = _speedOnPos;

        if (_effectsCoroutine != null)
            StopCoroutine(_effectsCoroutine);
        _effectsCoroutine = StartCoroutine(TransitionDepthOfField(_focalMoving));
    }

    private void StopTrain()
    {
        if (_TrainCoroutine != null)
            StopCoroutine(_TrainCoroutine);
        _TrainCoroutine = StartCoroutine(TrainStop());

        _train.Stop();
        _stop.Play();

        if (_throttleLever != null)
            _throttleLever.localPosition = _speedOffPos;

        if (_effectsCoroutine != null)
            StopCoroutine(_effectsCoroutine);
        _effectsCoroutine = StartCoroutine(TransitionDepthOfField(_focalStopped));
    }

    private void SetDirection(Direction dir)
    {
        _lastDirection = dir;
        if (_directionLever != null)
            _directionLever.localPosition = (dir == Direction.Left) ? _leftLeverPos : _rightLeverPos;
    }
}