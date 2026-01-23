using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TrainController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 30f;
    [SerializeField] private float _accelerationTime = 1f;
    [SerializeField] private float _rotationSpeed = 120f; // degrees per second
    [SerializeField] private float _waypointReachDistance = 1.5f;

    private float _currentSpeed = 0f;

    public enum Direction { Right, Left }
    public Direction _lastDirection = Direction.Right;

    [Header("Levers")]
    [SerializeField] private Transform _directionLever;
    [SerializeField] private Transform _throttleLever;

    private readonly Vector3 _rightLeverPos = new Vector3(0.46f, 0.8125f, 0f);
    private readonly Vector3 _leftLeverPos  = new Vector3(-0.469f, 0.8125f, 0f);
    private readonly Vector3 _speedOnPos    = new Vector3(1.20625f, 0.75f, 0.1875f);
    private readonly Vector3 _speedOffPos   = new Vector3(1.20625f, 0.75f, -0.266f);

    [Header("Audio")]
    [SerializeField] private AudioSource _train;
    [SerializeField] private AudioSource _stop;
    [SerializeField] private AudioSource _leverpower;
    [SerializeField] private AudioSource _leverdir;

    [Header("UI")]
    [SerializeField] private GameObject _buttonParent;
    [SerializeField] private GameObject _exitZoomButtonsParent;

    private Coroutine _trainCoroutine;
    private Coroutine _effectsCoroutine;
    private bool _isRunning = false;

    [Header("Depth Of Field")]
    [SerializeField] private PostProcessVolume _postProcessVolume;
    [SerializeField] private float _focalStopped = 8f;
    [SerializeField] private float _focalMoving = 18f;

    private DepthOfField _depthOfField;

    [Header("Waypoints")]
    [SerializeField] private Waypoint _currentWaypoint;

    public bool IsMoving => _currentSpeed > 0.01f;


    [Header("Rotation Smoothing")]
[SerializeField] private float _rotationSmoothTime = 0.4f;

private Quaternion _rotationVelocity;


    

    

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

        if (_postProcessVolume != null)
        {
            _postProcessVolume.profile.TryGetSettings(out _depthOfField);
            if (_depthOfField != null)
                _depthOfField.focusDistance.value = _focalStopped;
        }
    }

    void Update()
    {
        MoveAlongWaypoint();

        // Direction input
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

        // Speed input
        if (Input.GetKeyDown(KeyCode.W) && !GameManager.Instance._speedzero && !_isRunning)
        {
            _leverpower.Play();
            StartTrain();
            _isRunning = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || GameManager.Instance._speedzero)
        {
            GameManager.Instance._speedzero = false;
            StopTrain();
            _leverpower.Play();
            _isRunning = false;
        }
    }

    // ================= MOVEMENT =================

    private void MoveAlongWaypoint()
    {
        if (_currentWaypoint == null || _currentSpeed <= 0f)
            return;

        Vector3 targetPos = _currentWaypoint.transform.position;
        Vector3 flatTarget = new Vector3(targetPos.x, transform.position.y, targetPos.z);

        Vector3 toTarget = flatTarget - transform.position;
        float distance = toTarget.magnitude;

        if (distance <= _waypointReachDistance)
        {
            _currentWaypoint = _currentWaypoint.nextWaypoint;
            return;
        }

        Vector3 direction = toTarget.normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

transform.rotation = SmoothDampQuaternion(
    transform.rotation,
    targetRotation,
    ref _rotationVelocity,
    _rotationSmoothTime
);
        

        transform.position += direction * (_currentSpeed * Time.deltaTime);
    }

    private static Quaternion SmoothDampQuaternion(
    Quaternion current,
    Quaternion target,
    ref Quaternion velocity,
    float smoothTime
)
{
    if (Time.deltaTime < Mathf.Epsilon)
        return current;

    float dot = Quaternion.Dot(current, target);
    float multi = dot > 0f ? 1f : -1f;

    target.x *= multi;
    target.y *= multi;
    target.z *= multi;
    target.w *= multi;

    Vector4 result = new Vector4(
        Mathf.SmoothDamp(current.x, target.x, ref velocity.x, smoothTime),
        Mathf.SmoothDamp(current.y, target.y, ref velocity.y, smoothTime),
        Mathf.SmoothDamp(current.z, target.z, ref velocity.z, smoothTime),
        Mathf.SmoothDamp(current.w, target.w, ref velocity.w, smoothTime)
    );

    Quaternion q = new Quaternion(result.x, result.y, result.z, result.w);
    q.Normalize();
    return q;
}

    public void SetRotationWaypoint(Waypoint waypoint)
    {
        _currentWaypoint = waypoint;
    }

    // ================= TRAIN STATE =================

    private IEnumerator TrainDeparture()
    {
        float elapsed = 0f;
        while (elapsed < _accelerationTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _accelerationTime;
            _currentSpeed = Mathf.Lerp(0f, _speed, t * t);
            yield return null;
        }
        _currentSpeed = _speed;
    }

    private IEnumerator TrainStop()
    {
        float elapsed = 0f;
        float startSpeed = _currentSpeed;

        while (elapsed < _accelerationTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _accelerationTime;
            _currentSpeed = Mathf.Lerp(startSpeed, 0f, t * t);
            yield return null;
        }
        _currentSpeed = 0f;
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
        if (_trainCoroutine != null)
            StopCoroutine(_trainCoroutine);

        _trainCoroutine = StartCoroutine(TrainDeparture());

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
        if (_trainCoroutine != null)
            StopCoroutine(_trainCoroutine);

        _trainCoroutine = StartCoroutine(TrainStop());

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
        {
            _directionLever.localPosition =
                (dir == Direction.Left) ? _leftLeverPos : _rightLeverPos;
        }
    }
}