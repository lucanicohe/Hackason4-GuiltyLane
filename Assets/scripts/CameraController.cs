using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _display2Camera;
    [SerializeField] private Camera _display3Camera;
    private bool _display2Activated;
    private bool _display3Activated;

    void Update()
    {
        // == Display2 enabled toggle ==//
        if (Input.GetKeyDown(KeyCode.Alpha2) && _display2Activated)
        {
            _display2Camera.enabled = false;
            _display2Activated = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !_display2Activated)
        {
            // Disable other display cameras
            if (_display3Activated)
            {
                _display3Camera.enabled = false;
                _display3Activated = false;   
            }

            _display2Camera.enabled = true;
            _display2Activated = true;
        }

        // == Display3 enabled toggle ==//
        if (Input.GetKeyDown(KeyCode.Alpha3) && _display3Activated)
        {
            _display3Camera.enabled = false;
            _display3Activated = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !_display3Activated)
        {
            // Disable other display cameras
            if (_display2Activated)
            {
                _display2Camera.enabled = false;
                _display2Activated = false;   
            }

            _display3Camera.enabled = true;
            _display3Activated = true;
        }
    }
}
