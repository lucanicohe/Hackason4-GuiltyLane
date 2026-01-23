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
    [SerializeField] private AudioSource _spatialSwitchAudio; 

    [Header("Timing")]
    [SerializeField] private float _fadeDuration = 3f;

    [Header("Stop Image")]
    [SerializeField] private float _stopImageRadius = 20f;

    private AudioSource _musicSource;
    private TrainController _trainController;
    private CinemachineCameraController _cameraController;

    private bool _ekiPossible = false;
    private bool _isRunning = false;

    private Coroutine _fadeCoroutine;

    private SpriteRenderer _stopImage;
    private Transform _player;

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

        GameObject stopObj = GameObject.FindGameObjectWithTag("StopImage");
        if (stopObj != null)
        {
            _stopImage = stopObj.GetComponent<SpriteRenderer>();
            SetStopImageAlpha(0f);
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            _player = playerObj.transform;
    }

    private void Update()
    {
        if (_ekiPossible && !_isRunning && Input.GetKeyDown(KeyCode.E))
            StartFadeIn();

        if (Input.GetKeyDown(KeyCode.Q))
            StartFadeOut();

        HandleStopImage();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _ekiPossible = true;
        _ekiCanvas?.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _ekiPossible = false;
        _ekiCanvas?.SetActive(false);
    }

    // ===================== FADE IN =====================

    private void StartFadeIn()
    {
        _isRunning = true;

        _trainController?.enabled.Equals(false);
        _cameraController?.ChangeCamera(6);

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeRoutine(1f, 0f, true));
    }

    // ===================== FADE OUT =====================

    private void StartFadeOut()
    {
        _isRunning = false;

        _trainController.enabled = true;
        _cameraController?.ChangeCamera(4);

        if (_twoDoors != null)
        {
            _twoDoors.SetActive(false);
            SetSpatialAudio3D();
        }

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeRoutine(0f, _normalVolume, false));

        _ekiPossible = false;
        _ekiCanvas?.SetActive(false);
        SetStopImageAlpha(0f);
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
        {
            _twoDoors.SetActive(true);
            SetSpatialAudio2D();
        }
    }

    // ===================== HELPERS =====================

    private void SetFadeAlpha(float a)
    {
        if (_fadeImage == null) return;
        Color c = _fadeImage.color;
        c.a = a;
        _fadeImage.color = c;
    }

    private void HandleStopImage()
    {
        if (_stopImage == null || _player == null) return;

        float dist = Vector3.Distance(transform.position, _player.position);
        SetStopImageAlpha(dist <= _stopImageRadius ? 1f : 0f);
    }

    private void SetStopImageAlpha(float a)
    {
        if (_stopImage == null) return;
        Color c = _stopImage.color;
        c.a = a;
        _stopImage.color = c;
    }

    private void SetSpatialAudio2D()
    {
        if (_spatialSwitchAudio == null) return;
        _spatialSwitchAudio.spatialBlend = 0f;
    }

    private void SetSpatialAudio3D()
    {
        if (_spatialSwitchAudio == null) return;
        _spatialSwitchAudio.spatialBlend = 1f;
    }
}