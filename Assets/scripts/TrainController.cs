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

    void Start()
    {
        
        if (_directionLever != null)
            _directionLever.localPosition = _rightLeverPos;

        if (_throttleLever != null)
            _throttleLever.localPosition = _speedOffPos;

        _lastDirection = Direction.Right;
        _currentSpeed = 0f; 
    }

    void Update()
    {
        
        transform.Translate(Vector3.forward * _currentSpeed * Time.deltaTime);

        
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
    }
}