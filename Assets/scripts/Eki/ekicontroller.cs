using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ekicontroller : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _ekiCanvas;
    [SerializeField] private GameObject _fadeObject; // The entire fade GameObject
    [SerializeField] private Image _fadeImage;
    [SerializeField] private GameObject _twoDoors;
    [SerializeField] private GameObject _payoutScreen;

    [Header("Audio")]
    [SerializeField] private float _normalVolume = 0.3f;
    [SerializeField] private AudioSource _spatialSwitchAudio;
    [SerializeField] private AudioSource _payoutAudio; // New payout audio

    [Header("Timing")]
    [SerializeField] private float _fadeDuration = 3f;

    [Header("Stop Image")]
    [SerializeField] private float _stopImageRadius = 20f;

    private AudioSource _musicSource;
    private TrainController _trainController;
    private CinemachineCameraController _cameraController;
    private SlotManager _slotManager;

    private bool _ekiPossible = false;

    private Coroutine _fadeCoroutine;

    private SpriteRenderer _stopImage;
    private Transform _player;

    // States: Train -> Pachinko -> Payout
    private enum GameState { Train, Pachinko, Payout }
    private GameState _currentState = GameState.Train;

    private void Awake()
    {
        GameObject recordObj = GameObject.FindGameObjectWithTag("Record");
        if (recordObj != null)
            _musicSource = recordObj.GetComponent<AudioSource>();

        _trainController = FindObjectOfType<TrainController>();
        _cameraController = FindObjectOfType<CinemachineCameraController>();
        _slotManager = FindObjectOfType<SlotManager>();

        if (_ekiCanvas != null)
            _ekiCanvas.SetActive(false);

        if (_fadeObject != null)
            _fadeObject.SetActive(false);
        
        SetFadeAlpha(0f);

        if (_twoDoors != null)
            _twoDoors.SetActive(false);

        if (_payoutScreen != null)
            _payoutScreen.SetActive(false);

        if (_musicSource != null)
            _musicSource.volume = _normalVolume;

        // Initialize payout audio
        if (_payoutAudio != null)
        {
            _payoutAudio.volume = 0f;
            _payoutAudio.loop = true;
        }

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
        // E key - progress to next state
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleEKeyPress();
        }

        // Q key - return to train
        if (Input.GetKeyDown(KeyCode.Q))
        {
            HandleQKeyPress();
        }

        HandleStopImage();
    }

    private void HandleEKeyPress()
    {
        switch (_currentState)
        {
            case GameState.Train:
                // Only allow if inside eki collider
                if (_ekiPossible)
                {
                    TransitionToPachinko();
                }
                break;

            case GameState.Pachinko:
                // Stop pachinko before transitioning (destroys balls and resets indicators)
                if (_slotManager != null)
                    _slotManager.StopPachinko();
                
                // Transition from pachinko to payout
                TransitionToPayout();
                break;

            case GameState.Payout:
                // Do nothing - already at final state
                break;
        }
    }

    private void HandleQKeyPress()
    {
        // Q always returns to train (unless already in train)
        if (_currentState != GameState.Train)
        {
            // Stop pachinko if leaving from pachinko state (destroys balls and resets indicators)
            if (_currentState == GameState.Pachinko && _slotManager != null)
            {
                _slotManager.StopPachinko();
            }
            
            TransitionToTrain();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _ekiPossible = true;
        if (_currentState == GameState.Train)
        {
            _ekiCanvas?.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _ekiPossible = false;
        if (_currentState == GameState.Train)
        {
            _ekiCanvas?.SetActive(false);
        }
    }

    // ===================== TRAIN -> PACHINKO =====================
    private void TransitionToPachinko()
    {
        _currentState = GameState.Pachinko;
        
        // Disable train controls
        if (_trainController != null)
            _trainController.enabled = false;
        
        _cameraController?.ChangeCamera(6);

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeToPachinkoRoutine());
    }

    private IEnumerator FadeToPachinkoRoutine()
    {
        // Fade to black (music fades to 0)
        yield return FadeToBlack();

        // Show pachinko (twoDoors)
        if (_twoDoors != null)
            _twoDoors.SetActive(true);

        SetSpatialAudio2D();

        // Fade visual back to clear but keep music at 0
        yield return FadeToClearVisualOnly();
    }

    // ===================== PACHINKO -> PAYOUT =====================
    private void TransitionToPayout()
    {
        _currentState = GameState.Payout;

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeToPayoutRoutine());
    }

    private IEnumerator FadeToPayoutRoutine()
    {
        // Fade to black (visual + spatial switch audio to 0)
        yield return FadeToBlackWithSpatialAudio();

        // Hide pachinko, show payout
        if (_twoDoors != null)
            _twoDoors.SetActive(false);

        if (_payoutScreen != null)
            _payoutScreen.SetActive(true);

        // Start payout audio
        if (_payoutAudio != null)
        {
            _payoutAudio.Play();
            _payoutAudio.volume = 0.7f;
        }

        // Fade back to clear (visual only, spatial switch stays at 0, payout audio plays)
        yield return FadeToClearVisualOnly();
    }

    // ===================== PAYOUT/PACHINKO -> TRAIN =====================
    private void TransitionToTrain()
    {
        _currentState = GameState.Train;

        if (_trainController != null)
            _trainController.enabled = true;
        
        _cameraController?.ChangeCamera(4);

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeToTrainRoutine());
    }

    private IEnumerator FadeToTrainRoutine()
    {
        // Fade to black (visual + fade out payout audio)
        yield return FadeToBlackWithPayoutAudio();

        // Hide everything
        if (_twoDoors != null)
            _twoDoors.SetActive(false);

        if (_payoutScreen != null)
            _payoutScreen.SetActive(false);

        // Stop payout audio
        if (_payoutAudio != null)
        {
            _payoutAudio.Stop();
            _payoutAudio.volume = 0f;
        }

        // Restore spatial switch audio to normal (0.7 volume, 3D)
        if (_spatialSwitchAudio != null)
        {
            _spatialSwitchAudio.volume = 0.7f;
            _spatialSwitchAudio.spatialBlend = 1f;
        }

        SetSpatialAudio3D();

        // Fade back to clear and restore music to normal volume
        yield return FadeToClearWithMusic();

        // Update eki canvas visibility if still in collider
        if (_ekiPossible)
        {
            _ekiCanvas?.SetActive(true);
        }
        else
        {
            _ekiCanvas?.SetActive(false);
        }
        
        SetStopImageAlpha(0f);
    }

    // ===================== CORE FADE ROUTINES =====================
    // Fade visual AND music to black
    private IEnumerator FadeToBlack()
    {
        // Enable fade object
        if (_fadeObject != null)
            _fadeObject.SetActive(true);

        float elapsed = 0f;
        float startAlpha = _fadeImage.color.a;
        float startVolume = _musicSource != null ? _musicSource.volume : 0f;
        Color fadeColor = _fadeImage.color;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _fadeDuration;

            fadeColor.a = Mathf.Lerp(startAlpha, 1f, t);
            _fadeImage.color = fadeColor;

            if (_musicSource != null)
                _musicSource.volume = Mathf.Lerp(startVolume, 0f, t);

            yield return null;
        }

        fadeColor.a = 1f;
        _fadeImage.color = fadeColor;
        if (_musicSource != null)
            _musicSource.volume = 0f;
    }

    // Fade visual only to black (keep music at current level)
    private IEnumerator FadeToBlackVisualOnly()
    {
        // Enable fade object
        if (_fadeObject != null)
            _fadeObject.SetActive(true);

        float elapsed = 0f;
        float startAlpha = _fadeImage.color.a;
        Color fadeColor = _fadeImage.color;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _fadeDuration;

            fadeColor.a = Mathf.Lerp(startAlpha, 1f, t);
            _fadeImage.color = fadeColor;

            yield return null;
        }

        fadeColor.a = 1f;
        _fadeImage.color = fadeColor;
    }

    // Fade visual to black AND fade spatial switch audio to 0
    private IEnumerator FadeToBlackWithSpatialAudio()
    {
        // Enable fade object
        if (_fadeObject != null)
            _fadeObject.SetActive(true);

        float elapsed = 0f;
        float startAlpha = _fadeImage.color.a;
        float startSpatialVolume = _spatialSwitchAudio != null ? _spatialSwitchAudio.volume : 0f;
        Color fadeColor = _fadeImage.color;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _fadeDuration;

            fadeColor.a = Mathf.Lerp(startAlpha, 1f, t);
            _fadeImage.color = fadeColor;

            if (_spatialSwitchAudio != null)
                _spatialSwitchAudio.volume = Mathf.Lerp(startSpatialVolume, 0f, t);

            yield return null;
        }

        fadeColor.a = 1f;
        _fadeImage.color = fadeColor;
        if (_spatialSwitchAudio != null)
            _spatialSwitchAudio.volume = 0f;
    }

    // Fade visual to black AND fade payout audio to 0
    private IEnumerator FadeToBlackWithPayoutAudio()
    {
        // Enable fade object
        if (_fadeObject != null)
            _fadeObject.SetActive(true);

        float elapsed = 0f;
        float startAlpha = _fadeImage.color.a;
        float startPayoutVolume = _payoutAudio != null ? _payoutAudio.volume : 0f;
        Color fadeColor = _fadeImage.color;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _fadeDuration;

            fadeColor.a = Mathf.Lerp(startAlpha, 1f, t);
            _fadeImage.color = fadeColor;

            if (_payoutAudio != null)
                _payoutAudio.volume = Mathf.Lerp(startPayoutVolume, 0f, t);

            yield return null;
        }

        fadeColor.a = 1f;
        _fadeImage.color = fadeColor;
        if (_payoutAudio != null)
            _payoutAudio.volume = 0f;
    }

    // Fade visual to clear but keep music at 0
    private IEnumerator FadeToClearVisualOnly()
    {
        // Instantly set visual to transparent
        Color fadeColor = _fadeImage.color;
        fadeColor.a = 0f;
        _fadeImage.color = fadeColor;

        // Music stays at 0

        // Disable fade object so UI underneath is clickable
        if (_fadeObject != null)
            _fadeObject.SetActive(false);

        yield return null;
    }

    // Fade visual to clear AND restore music to normal volume
    private IEnumerator FadeToClearWithMusic()
    {
        // Instantly set visual to transparent
        Color fadeColor = _fadeImage.color;
        fadeColor.a = 0f;
        _fadeImage.color = fadeColor;

        // Restore music volume
        if (_musicSource != null)
            _musicSource.volume = _normalVolume;

        // Disable fade object so UI underneath is clickable
        if (_fadeObject != null)
            _fadeObject.SetActive(false);

        yield return null;
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
        SetStopImageAlpha(dist <= _stopImageRadius && _currentState == GameState.Train ? 1f : 0f);
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