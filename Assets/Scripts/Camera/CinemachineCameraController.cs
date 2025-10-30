using Cinemachine;
using UnityEngine;

public class CinemachineCameraController : MonoBehaviour
{
    [Header("Cinemachine Virtual Cameras")]
    [SerializeField] private CinemachineVirtualCamera mainTrainCamera;
    [SerializeField] private CinemachineVirtualCamera[] subCameras;

    private int activeIndex = -1; 

    private void Awake()
    {
        SetMainCameraActive();
    }

    private void Update()
    {
        for (int i = 0; i < subCameras.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ChangeCamera(i);
            }
        }
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
