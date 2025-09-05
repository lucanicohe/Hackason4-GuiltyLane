using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _displayCamera;

    private bool _display1Activated;
    private bool _display2Activated;
    private bool _display3Activated;

    private Vector3 _targetPosition;
    [SerializeField] private float _moveSpeed = 5f;

    void Start()
    {
        _targetPosition = _displayCamera.transform.localPosition;
    }

    void Update()
    {
        // == Display1 enabled toggle ==//
        if (Input.GetKeyDown(KeyCode.Alpha1) && _display1Activated)
        {
            _displayCamera.enabled = false;
            _display1Activated = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && !_display1Activated)
        {
            if (_display2Activated || _display3Activated)
            {
                _display2Activated = false;
                _display3Activated = false;
            }
            _targetPosition = new Vector3(-6.43f,
                _displayCamera.transform.localPosition.y,
                _displayCamera.transform.localPosition.z);

            _displayCamera.enabled = true;
            _display1Activated = true;
        }

        // == Display2 enabled toggle ==//
        if (Input.GetKeyDown(KeyCode.Alpha2) && _display2Activated)
        {
            _displayCamera.enabled = false;
            _display2Activated = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !_display2Activated)
        {
            if (_display1Activated || _display3Activated)
            {
                _display1Activated = false;
                _display3Activated = false;
            }
            _targetPosition = new Vector3(-4f,
                _displayCamera.transform.localPosition.y,
                _displayCamera.transform.localPosition.z);

            _displayCamera.enabled = true;
            _display2Activated = true;
        }

        // == Display3 enabled toggle ==//
        if (Input.GetKeyDown(KeyCode.Alpha3) && _display3Activated)
        {
            _displayCamera.enabled = false;
            _display3Activated = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !_display3Activated)
        {
            if (_display1Activated || _display2Activated)
            {
                _display1Activated = false;
                _display2Activated = false;
            }
            _targetPosition = new Vector3(-1.785f,
                _displayCamera.transform.localPosition.y,
                _displayCamera.transform.localPosition.z);

            _displayCamera.enabled = true;
            _display3Activated = true;
        }

        // === MoveTowards === //
        _displayCamera.transform.localPosition =
            Vector3.MoveTowards(
                _displayCamera.transform.localPosition,
                _targetPosition,
                _moveSpeed * Time.deltaTime
            );
    }
}

