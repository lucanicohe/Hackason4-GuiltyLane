using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ekicontroller : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _ekiCanvas; // Canvas root
    [SerializeField] private Image _fadeImage;
    [SerializeField] private GameObject _twoDoors;

    [Header("Audio")]
    [SerializeField] private float _normalVolume = 0.3f;

    [Header("Timing")]
    [SerializeField] private float _fadeDuration = 3f;

    private AudioSource _musicSource;

    private bool _ekiPossible = false;
    private bool _isRunning = false;

    private void Awake()
    {
        // Find music by tag
        GameObject recordObj = GameObject.FindGameObjectWithTag("Record");
        if (recordObj != null)
            _musicSource = recordObj.GetComponent<AudioSource>();

        // Canvas starts OFF
        if (_ekiCanvas != null)
            _ekiCanvas.SetActive(false);

        // Fade image init
        if (_fadeImage != null)
        {
            Color c = _fadeImage.color;
            c.a = 0f;
            _fadeImage.color = c;
        }

        // Doors start OFF
        if (_twoDoors != null)
            _twoDoors.SetActive(false);

        if (_musicSource != null)
            _musicSource.volume = _normalVolume;
    }

    private void Update()
    {
        if (_ekiPossible && !_isRunning && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(EkiSequence());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        _ekiPossible = true;

        if (_ekiCanvas != null)
            _ekiCanvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        _ekiPossible = false;

        if (_ekiCanvas != null)
            _ekiCanvas.SetActive(false);
    }

    private IEnumerator EkiSequence()
    {
        _isRunning = true;

        float elapsed = 0f;
        Color fadeColor = _fadeImage.color;
        float startVolume = _musicSource != null ? _musicSource.volume : 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _fadeDuration;

            fadeColor.a = Mathf.Lerp(0f, 1f, t);
            _fadeImage.color = fadeColor;

            if (_musicSource != null)
                _musicSource.volume = Mathf.Lerp(startVolume, 0f, t);

            yield return null;
        }

        // Final clamp
        fadeColor.a = 1f;
        _fadeImage.color = fadeColor;

        if (_musicSource != null)
            _musicSource.volume = 0f;

        if (_twoDoors != null)
            _twoDoors.SetActive(true);
    }
}