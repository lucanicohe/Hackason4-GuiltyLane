using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour
{
    [SerializeField] private Image _targetImage;
    [SerializeField] private float _fadeTime = 3f;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Color color = _targetImage.color;
        float startAlpha = color.a;
        float t = 0f;

        while (t < _fadeTime)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, 0f, t / _fadeTime);
            _targetImage.color = color;
            yield return null;
        }

        color.a = 0f;
        _targetImage.color = color;

        Destroy(gameObject);
    }
}