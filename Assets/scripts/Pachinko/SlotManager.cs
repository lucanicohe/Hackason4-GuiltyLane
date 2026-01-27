using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using TMPro;

public class SlotManager : MonoBehaviour
{
    [Header("SlotData")]
    [SerializeField]
    SlotData _slotData;
    SlotParam[] _decision = new SlotParam[3];

    [Header("Scripts")]
    [SerializeField]
    private GenParticle genParticle;

    [Header("Reels (Quad等にアタッチしたSlotReel)")]
    [SerializeField] private SlotReel[] _reels; // Slot1, Slot2, Slot3の実体

    [Header("UI")]
    [SerializeField]
    TextMeshProUGUI _scoreText;
    
    [Header("Bonus Indicators")]
    [SerializeField] private Image[] _bonusIndicators = new Image[4];
    private Color _normalColor = new Color(254f/255f, 255f/255f, 163f/255f); // FEFFA3
    private Color _bonusColor = new Color(200f/255f, 100f/255f, 100f/255f); // Subtle red
    private Color _whiteColor = Color.white;

    [Header("Settings")]
    public int _canReelCount = 0;
    private bool _isSpinning = false;
    private Coroutine _lotteryCoroutine;
    private Coroutine _blinkCoroutine;

    void Start()
    {
        // 初期スコア表示
        if (_scoreText != null) _scoreText.text = "";
        
        // Set initial indicator colors
        SetBonusIndicatorColor(_normalColor);
    }

    void Update()
    {
        //_canReelCountが残っていて、かつスピン中でなければ抽選開始
        if (_canReelCount > 0 && !_isSpinning)
        {
            _lotteryCoroutine = StartCoroutine(Lottery());
        }
        if (_scoreText != null)
            _scoreText.text = $"たま : {GameManager.Instance.Balls}";
    }

    // Called when leaving pachinko (from ekicontroller or other scripts)
    public void StopPachinko()
    {
        // Stop all running coroutines
        if (_lotteryCoroutine != null)
        {
            StopCoroutine(_lotteryCoroutine);
            _lotteryCoroutine = null;
        }
        
        if (_blinkCoroutine != null)
        {
            StopCoroutine(_blinkCoroutine);
            _blinkCoroutine = null;
        }

        // Stop all reels
        foreach (var reel in _reels)
        {
            if (reel != null)
            {
                reel.StopSpin();
            }
        }

        // Reset state
        _isSpinning = false;
        _canReelCount = 0;

        // Reset indicator colors to normal (yellow)
        SetBonusIndicatorColor(_normalColor);

        // Destroy all balls in the scene
        DestroyAllBalls();

        Debug.Log("Pachinko stopped and reset");
    }

    // Destroy all PachinkoBall objects in the scene
    private void DestroyAllBalls()
    {
        PachinkoBall[] balls = FindObjectsOfType<PachinkoBall>();
        foreach (PachinkoBall ball in balls)
        {
            if (ball != null)
            {
                Destroy(ball.gameObject);
            }
        }
        Debug.Log($"Destroyed {balls.Length} balls");
    }

    // 抽選と演出のコルーチン
    public IEnumerator Lottery()
    {
        _isSpinning = true;
        _canReelCount--;
        
        Debug.Log("抽選開始！");

        // 1. 全リール回転開始
        foreach (var reel in _reels)
        {
            reel.StartSpin();
        }

        // 2. 内部で抽選を行う
        _decision[0] = ChooseNumber();
        _decision[1] = ChooseNumber();
        _decision[2] = ChooseNumber();

        Debug.Log("抽選結果: " + _decision[2].number + " " + _decision[1].number + " " + _decision[0].number);

        // 3. 少し待ってから順番に止める（スロット演出）
        float waitTime = 1.0f; // 最初の待ち時間

        // リール1停止
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(_reels[0].StopSpinCoroutine(_decision[0].id));

        // リール2停止
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(_reels[1].StopSpinCoroutine(_decision[1].id));

        // リール3停止
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(_reels[2].StopSpinCoroutine(_decision[2].id)); // 最後のリールが止まるのを待つ

        // 4. 結果判定 - Check for big win (3 of a kind) first, then small win (pair)
        bool isBigWin = _decision.All(x => x.id == _decision[0].id);
        bool isSmallWin = false;
        
        if (!isBigWin)
        {
            // Check for any pair (2 matching numbers anywhere)
            isSmallWin = _decision[0].id == _decision[1].id || 
                        _decision[1].id == _decision[2].id || 
                        _decision[0].id == _decision[2].id;
        }

        if (isBigWin)
        {
            // Big Win: 3 matching numbers - instantly add 400 balls
            GameManager.Instance.AddBalls(400);
            
            if (_scoreText != null)
                _scoreText.text = "おおあたり！ +400";

            if (genParticle != null)
                StartCoroutine(genParticle.PlayGoldenShawerParticleCor());
            
            // Blink red/white for 3 seconds
            if (_blinkCoroutine != null)
                StopCoroutine(_blinkCoroutine);
            _blinkCoroutine = StartCoroutine(BlinkIndicators(3f));
            
            Debug.Log("Big Win! Added 400 balls instantly");
        }
        else if (isSmallWin)
        {
            // Small Win: pair of matching numbers - instantly add 10 balls
            GameManager.Instance.AddBalls(10);
            
            if (_scoreText != null)
                _scoreText.text = "こあたり！ +10";

            if (genParticle != null)
                StartCoroutine(genParticle.PlayGoldenShawerParticleCor());
            
            // Blink red/white for 1 second
            if (_blinkCoroutine != null)
                StopCoroutine(_blinkCoroutine);
            _blinkCoroutine = StartCoroutine(BlinkIndicators(1f));
            
            Debug.Log("Small Win! Added 10 balls instantly");
        }
        else
        {
            // No win - reset indicators to normal
            SetBonusIndicatorColor(_normalColor);
        }

        yield return new WaitForSeconds(1.5f); // 結果表示のため少し待つ
        _isSpinning = false;
        _lotteryCoroutine = null;
    }

    // Blink indicators between red and white for specified duration
    private IEnumerator BlinkIndicators(float duration)
    {
        float elapsed = 0f;
        bool isRed = true;
        float blinkInterval = 0.2f; // Blink every 0.2 seconds
        
        while (elapsed < duration)
        {
            SetBonusIndicatorColor(isRed ? _bonusColor : _whiteColor);
            isRed = !isRed;
            
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }
        
        // Return to normal color after blinking
        SetBonusIndicatorColor(_normalColor);
        _blinkCoroutine = null;
    }

    // 抽選
    SlotParam ChooseNumber()
    { 
        int total = 0;
        foreach (var data in _slotData.slotParams)
        {
            total += data.probability;
        }

        int p = Random.Range(0, total);

        int currentProbability = 0;
        foreach (var data in _slotData.slotParams)
        {
            currentProbability += data.probability;

            if (p < currentProbability)
            {
                return data;
            }
        }

        // 万が一漏れた場合は最後の要素かnullを返す
        return _slotData.slotParams[_slotData.slotParams.Count - 1];
    }

    // Set bonus indicator colors
    private void SetBonusIndicatorColor(Color color)
    {
        foreach (Image indicator in _bonusIndicators)
        {
            if (indicator != null)
            {
                indicator.color = color;
            }
        }
    }
}