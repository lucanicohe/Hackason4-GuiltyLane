using UnityEngine;
using System.Collections;

public class IntroController : MonoBehaviour
{
    [SerializeField] private GameObject _introObject;
    [SerializeField] private GameObject _goldGuildObject;
    [SerializeField] private GameObject _intro;
    //[SerializeField] private GameObject __cool;
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private CinemachineCameraController _cameraController;

    private bool _inputDetected = false;

    private void Start()
    {
        // _bgmSource.Play();
        _introObject.SetActive(true);
        //_intro.SetActive(true); 
        //__cool.SetActive(false); 
        //StartCoroutine("intro");
        _goldGuildObject.SetActive(false);

        // Start game at Camera 1
        //_cameraController.StartAtCamera1();
    }

    private void Update()
    {
        if (_inputDetected)
            return;

        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            _inputDetected = true;

           

            // Zoom out immediately when input happens
            _cameraController.ZoomOutToCamera4();

            StartCoroutine(SwitchObjectsAfterDelay(3f));
        }
    }

    private IEnumerator SwitchObjectsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        _introObject.SetActive(false);
        _goldGuildObject.SetActive(true);
    }
/*
    private IEnumerator intro()
    {
       // yield return new WaitForSeconds(1);
        __cool.SetActive(true);
        yield return new WaitForSeconds(1);
        __cool.SetActive(false);

        //_intro.SetActive(false); 
    }*/
}