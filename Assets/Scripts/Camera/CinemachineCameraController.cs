using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

public class CinemachineCameraController : MonoBehaviour
{
    [Header("Cinemachine Virtual Cameras")]
    [SerializeField] private CinemachineVirtualCamera mainTrainCamera;
    [SerializeField] private CinemachineVirtualCamera[] subCameras;
    [SerializeField] private GameObject TrainStationPrefab;
    
    private int activeIndex = 0;
    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) await ChangeCamera(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) await ChangeCamera(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) await ChangeCamera(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) await ChangeCamera(3);
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            await ChangeCamera(4);
        }
        
    }

    private async Task ChangeCamera(int index)
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
            await Task.Delay(1000);
            if(index == 4)
            {
                SwitchToTrainStation();
            }
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

    private void SwitchToTrainStation()
    {
        TrainStationPrefab.SetActive(true);
    }

}