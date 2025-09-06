using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

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

    [SerializeField] private Camera _camera1;
    private Vector3 _camera1LocalPostion;
    [SerializeField] private Camera _camera2;
    private Vector3 _camera2LocalPostion;
    [SerializeField] private Camera _camera3;
    private Vector3 _camera3LocalPostion;

    [SerializeField] private AudioSource _train;
    [SerializeField] private AudioSource _stop;

    
    // private bool _camera02Toggled = false;
    private Coroutine _cameraTransitionCoroutine;
    private Coroutine _TrainCoroutine;

    // Zoom variables
    private bool _isZoomedIn = false;
    private Vector3 _defaultCameraPos;

    // Parent for all normal buttons
    [SerializeField] private GameObject _buttonParent;

    // Special exit buttons only for Display 3
    [SerializeField] private GameObject _exitZoomButtonsParent;

    [SerializeField] private AudioSource _leverpower;
    [SerializeField] private AudioSource _leverdir; 


    // State toggles
    private bool _trainRunning = false;   
    private bool _directionIsLeft = false;

    private bool _isrunning = false; 

    void Start()
    {
        if (_directionLever != null)
            _directionLever.localPosition = _rightLeverPos;

        if (_throttleLever != null)
            _throttleLever.localPosition = _speedOffPos;

        _lastDirection = Direction.Right;
        _currentSpeed = 0f;

        if (_camera1 != null) _camera1.enabled = true;
        if (_camera2 != null) _camera2.enabled = false;
        if (_camera3 != null) _camera3.enabled = false;

        _camera1LocalPostion = _camera1.transform.localPosition;
        _camera2LocalPostion = _camera2.transform.localPosition;
        _camera3LocalPostion = _camera3.transform.localPosition;

        _defaultCameraPos = _camera1.transform.localPosition;

        if (_exitZoomButtonsParent != null)
            _exitZoomButtonsParent.SetActive(false); // hide at start
    }

    void Update()
    {
        // Move train forward
        transform.Translate(Vector3.forward * _currentSpeed * Time.deltaTime);

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

        // Switch cameras (debug / keyboard)
       
        // if (Input.GetKeyDown(KeyCode.E) || GameManager.Instance._speedzero)
        // {
        //     _camera02Toggled = !_camera02Toggled;

        //     if (_cameraTransitionCoroutine != null)
        //         StopCoroutine(_cameraTransitionCoroutine);

        //     Vector3 targetLocalPos = _camera02Toggled ? _camera3LocalPostion : _camera1LocalPostion;
        //     _cameraTransitionCoroutine = StartCoroutine(TransitionCameraPosition(targetLocalPos, 0.5f));
        // }

        // Zoom out with mouse click only if zoomed in (except Display 3 which uses exit buttons)
        if (_isZoomedIn && Input.GetMouseButtonDown(0) && (_exitZoomButtonsParent == null || !_exitZoomButtonsParent.activeSelf))
        {
            ExitZoom();
        }
    }

    private IEnumerator TransitionCameraPosition(Vector3 targetLocalPos, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startLocalPos = _camera1.transform.localPosition;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _camera1.transform.localPosition = Vector3.Lerp(startLocalPos, targetLocalPos, elapsedTime / duration);
            yield return null;
        }

        _camera1.transform.localPosition = targetLocalPos;
        _cameraTransitionCoroutine = null;
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

    // =======================
    // UI Button Functions
    // =======================

    public void ButtonTogglePower()
    {
        if (_trainRunning)
            StopTrain();
        else
            StartTrain();

        _trainRunning = !_trainRunning;
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

    public void ButtonToggleDirection()
    {
        _directionIsLeft = !_directionIsLeft;

        if (_directionIsLeft)
            SetDirection(Direction.Left);
        else
            SetDirection(Direction.Right);
    }

    private void SetDirection(Direction dir)
    {
        _lastDirection = dir;
        if (_directionLever != null)
            _directionLever.localPosition = (dir == Direction.Left) ? _leftLeverPos : _rightLeverPos;
    }

    // =======================
    // Zoom buttons
    // =======================

    public void ButtonZoomToCamera2()
    {
        if (_cameraTransitionCoroutine != null)
            StopCoroutine(_cameraTransitionCoroutine);

        _cameraTransitionCoroutine = StartCoroutine(TransitionCameraPosition(_camera2LocalPostion, 0.5f));
        _isZoomedIn = true;

        if (_buttonParent != null)
            _buttonParent.SetActive(false);
    }

    public void ButtonZoomToCamera3()
    {
        if (_cameraTransitionCoroutine != null)
            StopCoroutine(_cameraTransitionCoroutine);

        _cameraTransitionCoroutine = StartCoroutine(TransitionCameraPosition(_camera3LocalPostion, 0.5f));
        _isZoomedIn = true;

        // Hide normal buttons, show special exit buttons
        if (_buttonParent != null)
            _buttonParent.SetActive(false);
        if (_exitZoomButtonsParent != null)
            _exitZoomButtonsParent.SetActive(true);
    }

    public void ButtonSwitchCamera1()
    {
        ExitZoom();
    }

    // Called when exiting zoom (Display 2 with click, or Display 3 with buttons)
    public void ExitZoom()
    {
        if (_cameraTransitionCoroutine != null)
            StopCoroutine(_cameraTransitionCoroutine);

        _cameraTransitionCoroutine = StartCoroutine(TransitionCameraPosition(_camera1LocalPostion, 0.5f));
        _isZoomedIn = false;

        if (_buttonParent != null)
            _buttonParent.SetActive(true);
        if (_exitZoomButtonsParent != null)
            _exitZoomButtonsParent.SetActive(false);
    }

    // Specific exit for Display 3’s buttons
    public void ExitZoomFromDisplay3()
    {
        ExitZoom();
    }
}