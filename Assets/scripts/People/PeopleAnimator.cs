using System.Collections;
using UnityEngine;

public class PeopleAnimator : MonoBehaviour
{
    [SerializeField] private Texture[] _textures;
    [SerializeField] private float _frameTime = 0.3f;

    private Material _material;
    private int _currentIndex = 0;
    private Coroutine _playRoutine;

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
            return;

        // This creates a unique instance so you don’t animate all objects using the same material
        _material = renderer.material;

        if (_textures != null && _textures.Length > 0)
            _currentIndex = Random.Range(0, _textures.Length);
    }

    private void OnEnable()
    {
        if (_material == null || _textures == null || _textures.Length == 0)
            return;

        _playRoutine = StartCoroutine(PlayAnimation());
    }

    private void OnDisable()
    {
        if (_playRoutine != null)
            StopCoroutine(_playRoutine);
    }

    private IEnumerator PlayAnimation()
    {
        while (true)
        {
            _material.SetTexture("_MainTex", _textures[_currentIndex]);

            _currentIndex++;
            if (_currentIndex >= _textures.Length)
                _currentIndex = 0;

            yield return new WaitForSeconds(_frameTime);
        }
    }
}