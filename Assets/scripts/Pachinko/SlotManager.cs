using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using Unity.VisualScripting;

public class SlotManager : MonoBehaviour
{
    [Header("SlotData")]
    [SerializeField]
    SlotData _slotData;
    SlotParam[] _decision = new SlotParam[3];
    bool _isSuccess = false;
    bool _beginning = false;

    void Start()
    {
            
    }

    // 抽選
    public void Lottery()
    {
        if (!_beginning) return;

        Debug.Log("抽選開始！");
        _decision[0] = ChooseNumber();
        _decision[1] = ChooseNumber();
        _decision[2] = ChooseNumber();

        _isSuccess = _decision.All(x => x.id == _decision[0].id);

        if (_isSuccess)
        {// 当たり！！
            
        }
        else
        {
            return;
        }
    }

    SlotParam ChooseNumber()
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
                return data;
            }
        }

        return null;
    }


    public void RegisterBall(PachinkoBall ball)
    {
        ball._onEnter.Subscribe(x =>
        {
            _beginning = x;
        })
        .AddTo(ball);
    }



}
