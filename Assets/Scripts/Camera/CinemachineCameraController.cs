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

    private int _activeIndex = 0;
    private int _lastIndex = 0;

    private bool _sewerslideTriggered = false;

    void Start()
    {
        _Deadcam.SetActive(false);
        _fadeobject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ToggleCamera(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ToggleCamera(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ToggleCamera(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ToggleCamera(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ToggleCamera(4);
        if (Input.GetKeyDown(KeyCode.L))      ToggleCamera(5);

        if (GameManager.Instance.GuiltyPercent >= 100 && !_sewerslideTriggered)
        {
            _sewerslideTriggered = true;
            _fadeobject.SetActive(true);
            StartCoroutine(Sewerslide());
        }
    }

    // ================= CAMERA TOGGLE =================

    private void ToggleCamera(int index)
    {
        if (index == _activeIndex)
        {
            ChangeCamera(_lastIndex);
        }
        else
        {
            _lastIndex = _activeIndex;
            ChangeCamera(index);
        }
    }

    public void ChangeCamera(int index)
    {
        for (int i = 0; i < Cameras.Length; i++)
        {
            Cameras[i].Priority = (i == index) ? 100 : 0;
        }

        _activeIndex = index;
    }

    // ================= SEQUENCE =================

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
                color.a = elapsed;
                _fadeImage.color = color;
                yield return null;
            }

            color.a = 1f;
            _fadeImage.color = color;
        }

        yield return new WaitForSeconds(2f);
        _ESC.SetActive(true);
    }

    // ================= PUBLIC CAMERA CALLS =================

    public void StartAtCamera1()
    {
        ChangeCamera(1);
    }

    public void ZoomOutToCamera4()
    {
        ChangeCamera(4);
    }

    public void GoToCamera6()
    {
        ChangeCamera(6);
    }
}