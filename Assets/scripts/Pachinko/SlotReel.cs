using System.Collections;
using UnityEngine;

public class SlotReel : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _spinSpeed = 2.0f; // 回転速度
    [SerializeField] private int _totalSymbols = 9;   // 画像に含まれる絵柄の総数

    private Material _targetMaterial;
    private bool _isSpinning = false;

    void Awake()
    {
        if (_renderer == null) _renderer = GetComponent<Renderer>();
        _targetMaterial = _renderer.material;
    }

    void Update()
    {
        // 回転フラグが立っている間、Yオフセットをずらし続ける
        if (_isSpinning)
        {
            Vector2 offset = _targetMaterial.mainTextureOffset;
            offset.y = (offset.y + _spinSpeed * Time.deltaTime) % 1.0f;
            _targetMaterial.mainTextureOffset = offset;
        }
    }

    // 回転開始
    public void StartSpin()
    {
        _isSpinning = true;
    }

    // 回転停止（指定したIDの絵柄で止める）
    public IEnumerator StopSpinCoroutine(int stopId)
    {
        _isSpinning = false;

        // 各絵柄のオフセットステップを計算
        float step = 1.0f / _totalSymbols;

        float targetY = stopId * step;

        // 現在のオフセット
        float currentY = _targetMaterial.mainTextureOffset.y;
        float t = 0;
        float startY = currentY;

        if (targetY < startY) targetY += 1.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime * 2.0f; 
            float newY = Mathf.Lerp(startY, targetY, t);

            Vector2 offset = _targetMaterial.mainTextureOffset;
            offset.y = newY % 1.0f;
            _targetMaterial.mainTextureOffset = offset;

            yield return null;
        }

        // 最後にきっちり合わせる
        Vector2 finalOffset = _targetMaterial.mainTextureOffset;
        finalOffset.y = (stopId * step);
        _targetMaterial.mainTextureOffset = finalOffset;
    }
}
