using System.Collections;
using UnityEngine;

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

    private Coroutine _cameraTransitionCoroutine;
    private Coroutine _TrainCoroutine;

    private Vector3 _defaultCameraPos;

    // Parent for all normal buttons
    [SerializeField] private GameObject _buttonParent;

    // Special exit buttons only for Display 3
    [SerializeField] private GameObject _exitZoomButtonsParent;

    [SerializeField] private AudioSource _leverpower;
    [SerializeField] private AudioSource _leverdir; 
    
    private bool _isrunning = false; 

    void Start()
    {
        if (_directionLever != null)
            _directionLever.localPosition = _rightLeverPos;

        if (_throttleLever != null)
            _throttleLever.localPosition = _speedOffPos;

        _lastDirection = Direction.Right;
        _currentSpeed = 0f;

        if (_exitZoomButtonsParent != null)
            _exitZoomButtonsParent.SetActive(false); // hide at start
    }

    void Update()
    {
        // Move train forward
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

    private void StartTrain()
    {
        if (_TrainCoroutine != null)
            StopCoroutine(_TrainCoroutine);
        _TrainCoroutine = StartCoroutine(TrainDepature());
        _train.Play();
        _stop.Stop();
        if (_throttleLever != null)
            _throttleLever.localPosition = _speedOnPos;
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
    }

    private void SetDirection(Direction dir)
    {
        _lastDirection = dir;
        if (_directionLever != null)
            _directionLever.localPosition = (dir == Direction.Left) ? _leftLeverPos : _rightLeverPos;
    }
}