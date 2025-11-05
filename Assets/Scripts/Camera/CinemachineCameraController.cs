using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class CinemachineCameraController : MonoBehaviour
{
    [Header("Cinemachine Virtual Cameras")]
    [SerializeField] private CinemachineVirtualCamera mainTrainCamera;
    [SerializeField] private CinemachineVirtualCamera[] subCameras;

    [SerializeField] private AudioSource _gunshot;
    [SerializeField] private AudioSource _sad;
    [SerializeField] private PostProcessVolume _postProcessVolume;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private GameObject _fadeobject; 

    private int activeIndex = 0;
    private bool _sewerslideTriggered = false;

    void Start()
    {
        _fadeobject.SetActive(false); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeCamera(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeCamera(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeCamera(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeCamera(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeCamera(4);
        if (Input.GetKeyDown(KeyCode.L)) ChangeCamera(5);

        // if (GameManager.Instance.GuiltyPercent >= 100 && !_sewerslideTriggered)
        // {
        //     _sewerslideTriggered = true;
        //     _fadeobject.SetActive(true); 
            
        //     StartCoroutine("Sewerslide");
        // }
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

    private IEnumerator Sewerslide()
    {
        _sad.Play();
        yield return new WaitForSeconds(3f);
        _gunshot.Play();
        yield return new WaitForSeconds(1f);
        ChangeCamera(5);
        
        

        if (_fadeImage != null)
        {
            float elapsed = 0f;
            Color color = _fadeImage.color;
            color.a = 0f;
            _fadeImage.color = color;

            while (elapsed < 1f)
            {
                elapsed += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsed / 1f);
                _fadeImage.color = color;
                yield return null;
            }

            color.a = 1f;
            _fadeImage.color = color;
        }
    }
}