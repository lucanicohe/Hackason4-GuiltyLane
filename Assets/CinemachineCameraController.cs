using System;
using Cinemachine;
using UnityEngine;

public class CinemachineCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera[] Cameras = new CinemachineVirtualCamera[3];
    
    private void Update()
    {
        if(Input.GetKeyDown((KeyCode.Keypad1)))
        {   
            ChangeCamera("cam1");
        }
        if(Input.GetKeyDown((KeyCode.Keypad2)))
        {      
            ChangeCamera("cam2");
        }
        if (Input.GetKeyDown((KeyCode.Keypad3)))
        {
            ChangeCamera("mainCam");
        }
    }

    private void ChangeCamera(string camName)
    {
        foreach (var cam in Cameras)
        {
            if (camName == cam.name)
            {
                cam.Priority = 20;
            }
            else
            {
                cam.Priority = 10;
            }
        }
    }

    
}
