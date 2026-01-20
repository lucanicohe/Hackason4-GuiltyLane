using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EkiMove : MonoBehaviour
{
    [SerializeField] private Button _tmoButton;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private GameObject _twoDoors;

    [SerializeField] private float _fadeDuration = 2f;

    private void Awake()
    {
        // Ensure initial states
        if (_fadeImage != null)
        {
            Color c = _fadeImage.color;
            c.a = 0f;
            _fadeImage.color = c;
        }

        if (_twoDoors != null)
            _twoDoors.SetActive(false);

        if (_tmoButton != null)
            _tmoButton.onClick.AddListener(OnTmoPressed);
    }

    private void OnDestroy()
    {
        if (_tmoButton != null)
            _tmoButton.onClick.RemoveListener(OnTmoPressed);
    }

    public void OnTmoPressed()
    {
        if (_fadeImage != null)
            StartCoroutine(FadeInAndActivate());
    }

    private IEnumerator FadeInAndActivate()
    {
        float elapsed = 0f;
        Color c = _fadeImage.color;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, elapsed / _fadeDuration);
            _fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        _fadeImage.color = c;

        if (_twoDoors != null)
            _twoDoors.SetActive(true);
    }

   
}