using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ekicontroller : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _ekiCanvas;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private GameObject _twoDoors;

    [Header("Audio")]
    [SerializeField] private float _normalVolume = 0.3f;

    [Header("Timing")]
    [SerializeField] private float _fadeDuration = 3f;

    private AudioSource _musicSource;
    private TrainController _trainController;
    private CinemachineCameraController _cameraController;

    private bool _ekiPossible = false;
    private bool _isRunning = false;

    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        GameObject recordObj = GameObject.FindGameObjectWithTag("Record");
        if (recordObj != null)
            _musicSource = recordObj.GetComponent<AudioSource>();

        _trainController = FindObjectOfType<TrainController>();
        _cameraController = FindObjectOfType<CinemachineCameraController>();

        if (_ekiCanvas != null)
            _ekiCanvas.SetActive(false);

        SetFadeAlpha(0f);

        if (_twoDoors != null)
            _twoDoors.SetActive(false);

        if (_musicSource != null)
            _musicSource.volume = _normalVolume;
    }

    private void Update()
    {
        if (_ekiPossible && !_isRunning && Input.GetKeyDown(KeyCode.E))
        {
            StartFadeIn();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartFadeOut();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _ekiPossible = true;
        if (_ekiCanvas != null)
            _ekiCanvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _ekiPossible = false;
        if (_ekiCanvas != null)
            _ekiCanvas.SetActive(false);
    }

    // ===================== FADE IN (E) =====================

    private void StartFadeIn()
    {
        _isRunning = true;

        if (_trainController != null)
            _trainController.enabled = false;

        if (_cameraController != null)
            _cameraController.ChangeCamera(6); // camera 6 on fade in

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeRoutine(1f, 0f, true));
    }

    // ===================== FADE OUT (Q) =====================

    private void StartFadeOut()
    {
        _isRunning = false;

        if (_trainController != null)
            _trainController.enabled = true;

        if (_cameraController != null)
            _cameraController.ChangeCamera(4); // back to camera 4

        if (_twoDoors != null)
            _twoDoors.SetActive(false); // immediate

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeRoutine(0f, _normalVolume, false));
        _ekiPossible = false;
        if (_ekiCanvas != null)
            _ekiCanvas.SetActive(false);
    }

    // ===================== CORE ROUTINE =====================

    private IEnumerator FadeRoutine(float targetAlpha, float targetVolume, bool enableDoorsAtEnd)
    {
        float elapsed = 0f;

        float startAlpha = _fadeImage.color.a;
        float startVolume = _musicSource != null ? _musicSource.volume : 0f;

        Color fadeColor = _fadeImage.color;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _fadeDuration;

            fadeColor.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            _fadeImage.color = fadeColor;

            if (_musicSource != null)
                _musicSource.volume = Mathf.Lerp(startVolume, targetVolume, t);

            yield return null;
        }

        fadeColor.a = targetAlpha;
        _fadeImage.color = fadeColor;

        if (_musicSource != null)
            _musicSource.volume = targetVolume;

        if (enableDoorsAtEnd && _twoDoors != null)
            _twoDoors.SetActive(true);
    }

    private void SetFadeAlpha(float a)
    {
        if (_fadeImage == null) return;
        Color c = _fadeImage.color;
        c.a = a;
        _fadeImage.color = c;
    }
}