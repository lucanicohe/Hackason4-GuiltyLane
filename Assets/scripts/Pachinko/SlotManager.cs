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

    [Header("Slot表示用")]
    [SerializeField]
    SpriteRenderer _slot1;
    [SerializeField]
    SpriteRenderer _slot2;
    [SerializeField]
    SpriteRenderer _slot3;
    [SerializeField]
    TextMeshProUGUI _scoreText;

    [Header("結果")]
    bool _isSuccess = false;

    private int _score = 0;

    void Start()
    {
            
    }

    // 抽選
    public IEnumerator Lottery()
    {

        Debug.Log("抽選開始！");

        // TODO:抽選アニメーション

        yield return new WaitForSeconds(1.0f);


        _decision[0] = ChooseNumber(ref _slot1);
        _decision[1] = ChooseNumber(ref _slot2);
        _decision[2] = ChooseNumber(ref _slot3);

        Debug.Log("抽選結果: " + _decision[0].id + " " + _decision[1].id + " " + _decision[2].id);

        _isSuccess = _decision.All(x => x.id == _decision[0].id);

        if (_isSuccess)
        {// 当たり！！
            switch (_decision[0].id)
            {
                case 0:
                    _score += 100;
                    StartCoroutine(genParticle.PlaySlotParticleCor());
                    break;
                case 1:
                    _score += 500;
                    StartCoroutine(genParticle.PlaySlotParticleCor());
                    break;
                case 2:
                    _score += 1000;
                    StartCoroutine(genParticle.PlaySlotParticleCor());
                    break;
                default:
                    break;
            }

            _scoreText.text = "Score: " + _score.ToString();
        }
    }

    // 抽選する
    SlotParam ChooseNumber(ref SpriteRenderer _slotNumber)
    {
        int total = 0;
        foreach(var data in _slotData.slotParams)
        {
            total += data.probability;
        }

        int p = Random.Range(0, total);

        int probability = 0;
        foreach (var data in _slotData.slotParams)
        {
            probability += data.probability;

            if(p < probability)
            {
                _slotNumber.sprite = data.number;
                return data;
            }
        }

        return null;
    }




}
