using UnityEngine;

public class Amover : MonoBehaviour
{
    [SerializeField] private float _distance = 50f;
    [SerializeField] private float _speed = 100f;

    private RectTransform _rectTransform;
    private Vector2 _startPos;
    private float _direction = 1f;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _startPos = _rectTransform.anchoredPosition;
    }

    private void Update()
    {
        Vector2 pos = _rectTransform.anchoredPosition;
        pos.x += _direction * _speed * Time.deltaTime;

        if (Mathf.Abs(pos.x - _startPos.x) >= _distance)
            _direction *= -1f;

        _rectTransform.anchoredPosition = pos;
    }
}