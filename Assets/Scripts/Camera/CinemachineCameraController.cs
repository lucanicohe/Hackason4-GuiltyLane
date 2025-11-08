using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class CinemachineCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera[] Cameras;

    [SerializeField] private AudioSource _gunshot;
    [SerializeField] private AudioSource _sad;
    [SerializeField] private PostProcessVolume _postProcessVolume;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private GameObject _fadeobject;
    [SerializeField] private GameObject _Deadcam; 
    [SerializeField] private GameObject _ESC; 

    private int activeIndex = 0;
    private bool _sewerslideTriggered = false;

    void Start()
    {
        ChangeCamera(4);
        _Deadcam.SetActive(false); 


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

        if (GameManager.Instance.GuiltyPercent >= 100 && !_sewerslideTriggered)
        {
            _sewerslideTriggered = true;
            _fadeobject.SetActive(true); 
            
            StartCoroutine("Sewerslide");
        }
    }

    private void ChangeCamera(int index)
    {
        for (int i = 0; i < Cameras.Length; i++)
        {
            Cameras[i].Priority = (i == index) ? 100 : 0;
        }
        activeIndex = index;
    }

    private IEnumerator Sewerslide()
    {
        _Deadcam.SetActive(true); 
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

        yield return new WaitForSeconds(2f);
        _ESC.SetActive(true); 
    }
}