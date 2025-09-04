using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    [SerializeField] private float _speed = 30f;
    private float _currentSpeed = 0f;

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
    
    [SerializeField] private AudioSource _train;
    private Vector3 _camera3LocalPostion;

    private bool _camera01Toggled = false;
    private bool _camera02Toggled = false;
    private Coroutine _cameraTransitionCoroutine;
    
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
    }

    void Update()
    {
        // Move train forward
        transform.Translate(Vector3.forward * _currentSpeed * Time.deltaTime);

        // Change direction
        if (Input.GetKeyDown(KeyCode.A))
        {
            _lastDirection = Direction.Left;
            if (_directionLever != null)
                _directionLever.localPosition = _leftLeverPos;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _lastDirection = Direction.Right;
            if (_directionLever != null)
                _directionLever.localPosition = _rightLeverPos;
        }

        // Change speed
        if (Input.GetKeyDown(KeyCode.W) && !GameManager.Instance._speedzero)
        {
            _currentSpeed = _speed;
            _train.Play();
            //AudioManager.Instance.PlayBGM(AudioManager.BGMType.railway_train_339502);
            AudioManager.Instance.StopPlayBGM(AudioManager.BGMType.電車停車中);
            if (_throttleLever != null)
                _throttleLever.localPosition = _speedOnPos;
        }
        else if (Input.GetKeyDown(KeyCode.S) || GameManager.Instance._speedzero)
        {
            GameManager.Instance._speedzero = false; //TODO:Qを押したように、とまったときにカメラを動かしたい→この行を消すとできるが、コルーチンがうまくいかないのか画面ががくがくする。
            //AudioManager.Instance.StopPlayBGM(AudioManager.BGMType.railway_train_339502);
            AudioManager.Instance.PlayBGM(AudioManager.BGMType.電車停車中);
            _train.Stop(); 
            _currentSpeed = 0f;
            if (_throttleLever != null)
                _throttleLever.localPosition = _speedOffPos;
        }

        // Switch cameras
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     _cameraToggled = !_cameraToggled;
        //     if (_camera1 != null) _camera1.enabled = !_cameraToggled;
        //     if (_camera2 != null) _camera2.enabled = _cameraToggled;
        // }
        if (Input.GetKeyDown(KeyCode.Q) || GameManager.Instance._speedzero)
        {
            _camera01Toggled = !_camera01Toggled;

            if (_cameraTransitionCoroutine != null)
                StopCoroutine(_cameraTransitionCoroutine);

            Vector3 targetLocalPos = _camera01Toggled ? _camera2LocalPostion : _camera1LocalPostion;
            _cameraTransitionCoroutine = StartCoroutine(TransitionCameraPosition(targetLocalPos, 0.5f));
        }
        if (Input.GetKeyDown(KeyCode.P) || GameManager.Instance._speedzero)
        {
            _camera02Toggled = !_camera02Toggled;

            if (_cameraTransitionCoroutine != null)
                StopCoroutine(_cameraTransitionCoroutine);

            Vector3 targetLocalPos = _camera02Toggled ? _camera3LocalPostion : _camera1LocalPostion;
            _cameraTransitionCoroutine = StartCoroutine(TransitionCameraPosition(targetLocalPos, 0.5f));
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
}