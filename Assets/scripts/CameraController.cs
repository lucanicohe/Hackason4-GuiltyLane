using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _displayCamera;

    private bool _display2Activated;
    private bool _display3Activated;

    void Update()
    {
        // == Display2 enabled toggle ==//
        if (Input.GetKeyDown(KeyCode.Alpha2) && _display2Activated)
        {
            _displayCamera.enabled = false;
            _display2Activated = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !_display2Activated)
        {
            // Disable other display cameras
            if (_display3Activated)
            {
                _display3Activated = false;
            }
            _displayCamera.transform.localPosition = new Vector3(-4f, 7.154908f, 1.870422f);
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
            // Disable other display cameras
            if (_display2Activated)
            {
                _display2Activated = false;
            }
            _displayCamera.transform.localPosition = new Vector3(-1.785f, 7.154908f, 1.870422f);
            _displayCamera.enabled = true;
            _display3Activated = true;
        }
    }
}
