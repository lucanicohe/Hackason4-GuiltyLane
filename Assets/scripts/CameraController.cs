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
        _targetPosition = new Vector3(-3.089999f,
                9.9f,
                -4.359997f);
        _displayCamera.transform.localPosition = _targetPosition;
    }

    void Update()
    {
        // == Display1 enabled toggle ==//
        if (Input.GetKeyDown(KeyCode.Alpha1) && _display1Activated)
        {
            // _displayCamera.enabled = false;
            // _display1Activated = false;
            MainCamPos();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && !_display1Activated)
        {
            // Disable other displays
            if (_display2Activated) _display2Activated = false;
            if (_display3Activated) _display3Activated = false;

            _displayCamera.transform.localPosition = new Vector3(-6.42f, 7.154907f, 1.870422f);
            _displayCamera.enabled = true;
            _display1Activated = true;
        }

        // == Display2 enabled toggle ==//
        if (Input.GetKeyDown(KeyCode.Alpha2) && _display2Activated)
        {
            // _displayCamera.enabled = false;
            // _display2Activated = false;
            MainCamPos();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !_display2Activated)
        {
            if (_display1Activated) _display1Activated = false;
            if (_display3Activated) _display3Activated = false;

            _displayCamera.transform.localPosition = new Vector3(-4f, 7.154908f, 1.870422f);
            _displayCamera.enabled = true;
            _display2Activated = true;
        }

        // == Display3 enabled toggle ==//
        if (Input.GetKeyDown(KeyCode.Alpha3) && _display3Activated)
        {
            // _displayCamera.enabled = false;
            // _display3Activated = false;
            MainCamPos();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !_display3Activated)
        {
            if (_display1Activated) _display1Activated = false;
            if (_display2Activated) _display2Activated = false;

            _displayCamera.transform.localPosition = new Vector3(-1.785f, 7.154908f, 1.870422f);
            _displayCamera.enabled = true;
            _display3Activated = true;
        }

        // === MoveTowards === //
        // _displayCamera.transform.localPosition =
        //     Vector3.MoveTowards(
        //         _displayCamera.transform.localPosition,
        //         _targetPosition,
        //         _moveSpeed * Time.deltaTime
        //     );
    }

    public void MainCamPos()
    {
        _display1Activated = false;
        _display2Activated = false;
        _display3Activated = false;

        _targetPosition = new Vector3(-3.089999f,
                9.9f,
                -4.359997f);
        _displayCamera.transform.localPosition = _targetPosition;
    }
}
