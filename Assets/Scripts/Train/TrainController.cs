using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

public class TrainController : MonoBehaviour
{
    [SerializeField] private float _speed = 30f;
    private float _currentSpeed = 0f;
    [SerializeField] private float _accelerationTime = 1f;
    
    public Direction lastDirection = Direction.Right;

    [SerializeField] private Transform _directionLever;
    private Vector3 _rightLeverPos = new Vector3(0.46f, 0.8125f, 0f);
    private Vector3 _leftLeverPos = new Vector3(-0.469f, 0.8125f, 0f);

    [SerializeField] private Transform _throttleLever;
    private Vector3 _speedOnPos = new Vector3(1.20625f, 0.75f, 0.1875f);
    private Vector3 _speedOffPos = new Vector3(1.20625f, 0.75f, -0.266f);

    [SerializeField] private AudioSource _train;
    [SerializeField] private AudioSource _stop;

    private Coroutine _cameraTransitionCoroutine;
    private Coroutine _TrainCoroutine;
    private Coroutine _effectsCoroutine;

    private Vector3 _defaultCameraPos;

    [SerializeField] private GameObject _buttonParent;
    [SerializeField] private GameObject _exitZoomButtonsParent;

    [SerializeField] private AudioSource _leverpower;
    [SerializeField] private AudioSource _leverdir;
    
    private bool _isrunning = false;

    // ===== POST PROCESSING EFFECTS =====
    [SerializeField] private PostProcessVolume _postProcessVolume;
    private ChromaticAberration _chromaticAberration;

    [SerializeField] private float _chromaticstart = 0.5f;
    [SerializeField] private float _chromaticend = 1f;


    // ===== WAYPOINT ROTATION ONLY =====
    [SerializeField] private float _rotationSpeed = 3f; // Smooth rotation speed
    private Waypoint _currentWaypoint;
    private float _waypointReachDistance = 1f; // Distance to switch to next waypoint
    private bool _isRotatingToWaypoint = false;

    

    void Start()
    {
        if (_directionLever != null)
            _directionLever.localPosition = _rightLeverPos;

        if (_throttleLever != null)
            _throttleLever.localPosition = _speedOffPos;

        lastDirection = Direction.Right;
        _currentSpeed = 0f;

        if (_exitZoomButtonsParent != null)
            _exitZoomButtonsParent.SetActive(false);

        // ===== INITIALIZE POST PROCESSING EFFECTS =====
        if (_postProcessVolume != null)
        {
            _postProcessVolume.profile.TryGetSettings(out _chromaticAberration);
            if (_chromaticAberration != null)
                _chromaticAberration.intensity.value = _chromaticstart;
        }
    }

    void Update()
    {
        // ===== WAYPOINT ROTATION (ONLY Y AXIS) =====
        if (_currentWaypoint != null && _isRotatingToWaypoint)
        {
            // Calculate direction to waypoint (only horizontal plane)
            Vector3 targetPos = _currentWaypoint.transform.position;
            Vector3 directionToWaypoint = new Vector3(
                targetPos.x - transform.position.x,
                0, // Ignore Y difference
                targetPos.z - transform.position.z
            ).normalized;

            if (directionToWaypoint != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToWaypoint);
                // Smoothly rotate towards target (only Y rotation)
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    targetRotation, 
                    Time.deltaTime * _rotationSpeed
                );
            }

            // Check if we reached this waypoint (switch to next)
            float distance = Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(targetPos.x, 0, targetPos.z)
            );

            if (distance < _waypointReachDistance)
            {
                // Move to next waypoint in chain (for forks)
                if (_currentWaypoint.nextWaypoint != null)
                {
                    _currentWaypoint = _currentWaypoint.nextWaypoint;
                }
                else
                {
                    // No more waypoints, stop rotating
                    _isRotatingToWaypoint = false;
                }
            }
        }
       
        transform.Translate(Vector3.forward * (_currentSpeed * Time.deltaTime));

        // Change direction with keyboard
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

        // Change speed with keyboard
        if (Input.GetKeyDown(KeyCode.W)&& !_isrunning)
        {
            _leverpower.Play();
            StartTrain();
            _isrunning = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            // GameManager.Instance._speedzero = false;
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
        float _elapsedTime = 0f;
        while (_elapsedTime < _accelerationTime)
        {
            _elapsedTime += Time.deltaTime;
            float per = _elapsedTime / _accelerationTime;
            _currentSpeed = Mathf.Lerp(0f, _speed, per * per);

            yield return null;
        }
    }

    private IEnumerator TrainStop()
    {
        float _elapsedTime = 0f;
        while (_elapsedTime < _accelerationTime)
        {
            _elapsedTime += Time.deltaTime;
            float per = _elapsedTime / _accelerationTime;
            _currentSpeed = Mathf.Lerp(_speed, 0f, per * per);

            yield return null;
        }
    }

    private IEnumerator TransitionEffects(float targetChromatic)
    {
        float _elapsedTime = 0f;
        float startChromatic = _chromaticAberration.intensity.value;

        while (_elapsedTime < _accelerationTime)
        {
            _elapsedTime += Time.deltaTime;
            float per = _elapsedTime / _accelerationTime;

            if (_chromaticAberration != null)
                _chromaticAberration.intensity.value = Mathf.Lerp(startChromatic, targetChromatic, per);

            yield return null;
        }

        // Ensure final value is set
        if (_chromaticAberration != null)
            _chromaticAberration.intensity.value = targetChromatic;
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

        // Transition effects to on
        if (_effectsCoroutine != null)
            StopCoroutine(_effectsCoroutine);
        _effectsCoroutine = StartCoroutine(TransitionEffects(_chromaticend));
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

        // Transition effects to off
        if (_effectsCoroutine != null)
            StopCoroutine(_effectsCoroutine);
        _effectsCoroutine = StartCoroutine(TransitionEffects(_chromaticstart));
    }

    private void SetDirection(Direction dir)
    {
        lastDirection = dir;
        if (_directionLever != null)
            _directionLever.localPosition = (dir == Direction.Left) ? _leftLeverPos : _rightLeverPos;
    }
}