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
    [SerializeField] private Camera _camera2;
    private bool _cameraToggled = false;

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
        if (Input.GetKeyDown(KeyCode.W))
        {
            _currentSpeed = _speed;
            if (_throttleLever != null)
                _throttleLever.localPosition = _speedOnPos;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _currentSpeed = 0f;
            if (_throttleLever != null)
                _throttleLever.localPosition = _speedOffPos;
        }

        // Switch cameras
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _cameraToggled = !_cameraToggled;
            if (_camera1 != null) _camera1.enabled = !_cameraToggled;
            if (_camera2 != null) _camera2.enabled = _cameraToggled;
        }
    }
}