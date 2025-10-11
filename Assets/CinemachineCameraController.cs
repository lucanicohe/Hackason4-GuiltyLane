using System;
using Cinemachine;
using UnityEngine;

public class CinemachineCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera[] Cameras;

    private int activeIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeCamera(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeCamera(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeCamera(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeCamera(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeCamera(4);
    }

    private void ChangeCamera(int index)
    {
        for (int i = 0; i < Cameras.Length; i++)
        {
            Cameras[i].Priority = (i == index) ? 100 : 0;
        }
        activeIndex = index;
    }
}
