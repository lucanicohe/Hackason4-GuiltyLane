using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Header("Settings")]
    public int _canReelCount = 0;
    private bool _isSpinning = false;
    private Coroutine _lotteryCoroutine;

    private int _score = 0;

    void Start()
    {
        // 初期スコア表示
        if (_scoreText != null) _scoreText.text = "Score: " + _score.ToString();
    }

    void Update()
    {
        //_canReelCountが残っていて、かつスピン中でなければ抽選開始
        if (_canReelCount > 0 && !_isSpinning)
        {
            _lotteryCoroutine = StartCoroutine(Lottery());
        }
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

        // 4. 結果判定
        bool isSuccess = _decision.All(x => x.id == _decision[0].id);

        if (isSuccess)
        {
            // 当たり！！
            // 注意: 元のコードはwhileループで無限加算していたため修正しました
            switch (_decision[0].id)
            {
                case 0:
                    _score += 100;
                    if (genParticle != null) StartCoroutine(genParticle.PlayFireWorkParticleCor());
                    break;
                case 1:
                    _score += 500;
                    if (genParticle != null) StartCoroutine(genParticle.PlayFireWorkParticleCor());
                    break;
                case 2:
                    _score += 1000;
                    if (genParticle != null) StartCoroutine(genParticle.PlayGoldenShawerParticleCor());
                    break;
                case 3:
                    _score += 5000;
                    if (genParticle != null) StartCoroutine(genParticle.PlayGoldenShawerParticleCor());
                    break;
                case 4:
                    _score += 10000;
                    if (genParticle != null) StartCoroutine(genParticle.PlayGoldenShawerParticleCor());
                    break;
                case 5:
                    _score += 100;
                    if (genParticle != null) StartCoroutine(genParticle.PlayFireWorkParticleCor());
                    break;
                case 6:
                    _score += 500;
                    if (genParticle != null) StartCoroutine(genParticle.PlayFireWorkParticleCor());
                    break;
                case 7:
                    _score += 1000;
                    if (genParticle != null) StartCoroutine(genParticle.PlayGoldenShawerParticleCor());
                    break;
                case 8:
                    _score += 5000;
                    if (genParticle != null) StartCoroutine(genParticle.PlayGoldenShawerParticleCor());
                    break;
                case 9:
                    _score += 10000;
                    if (genParticle != null) StartCoroutine(genParticle.PlayGoldenShawerParticleCor());
                    break;
                default:
                    break;
            }

            if (_scoreText != null) _scoreText.text = "Score: " + _score.ToString();
        }

        yield return new WaitForSeconds(1.5f); // 結果表示のため少し待つ
        _isSpinning = false;
        _lotteryCoroutine = null;
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
}