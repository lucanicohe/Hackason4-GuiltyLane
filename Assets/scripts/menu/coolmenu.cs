using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoolMenu : MonoBehaviour
{
    [SerializeField] private Image _targetImage;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _frameTime = 0.1f;

    private int _currentIndex;
    private int _direction = 1;
    private bool _paused;

    private void Start()
    {
        StartCoroutine(PlayLoop());
    }

    public void Pause(bool pause)
    {
        _paused = pause;
    }

    private IEnumerator PlayLoop()
    {
        while (true)
        {
            if (_paused)
            {
                yield return null;
                continue;
            }

            _targetImage.sprite = _sprites[_currentIndex];

            if (_currentIndex == _sprites.Length - 1)
                _direction = -1;
            else if (_currentIndex == 0)
                _direction = 1;

            _currentIndex += _direction;

            yield return new WaitForSeconds(_frameTime);
        }
    }
}