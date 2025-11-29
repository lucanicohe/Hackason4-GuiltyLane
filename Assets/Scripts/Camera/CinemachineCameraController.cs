using Cinemachine;
using UnityEngine;

public class CinemachineCameraController : MonoBehaviour
{
    [Header("Cinemachine Virtual Cameras")]
    [SerializeField] private CinemachineVirtualCamera mainTrainCamera;
    [SerializeField] private CinemachineVirtualCamera[] subCameras;
    
    private int activeIndex = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeCamera(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeCamera(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeCamera(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeCamera(3);
    }

    private void ChangeCamera(int index)
    {
        if (index < 0 || index >= subCameras.Length)
        {
            Debug.LogWarning($"Invalid camera index: {index}");
            return;
        }
        
        if (activeIndex == index)
        {
            SetMainCameraActive();
            activeIndex = -1;
        }
        else
        {
            mainTrainCamera.Priority = 0;
            
            for (int i = 0; i < subCameras.Length; i++)
            {
                subCameras[i].Priority = i == index ? 100 : 0;
            }
            activeIndex = index;
        }
    }

    private void SetMainCameraActive()
    {
        if (mainTrainCamera) mainTrainCamera.Priority = 100;
        foreach (var cam in subCameras)
        {
            cam.Priority = 0;
        }
        activeIndex = -1;
    }
}