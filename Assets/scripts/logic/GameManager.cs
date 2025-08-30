using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.PlayerLoop;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    TextMeshProUGUI total;
    private TeamBuilder.HitPersonData _RightHPD;
    public TeamBuilder.HitPersonData RightHPD => _RightHPD;

    private TeamBuilder.HitPersonData _LeftHPD;
    public TeamBuilder.HitPersonData LeftHPD => _LeftHPD;
    [SerializeField]
    TeamBuilder teamBuilder;
    [SerializeField]
    TrainController trainController;

    //自分が持っているお金
    public int Gold = 0;
    //罪率
    public int GuiltyPercent = 0;
    //給料
    public int Salary = 12;

    //合計のGuiltyPercent
    private int _TotalGuiltyPercent;
    public int TotalGuiltyPercent => _TotalGuiltyPercent;

    //合計のInheritance
    private int _TotalInheritance;
    public int TotalInheritance => _TotalInheritance;

    //ラウンド内の何回行うか-->子の変数が3以上になったら次のラウンドへ進む
    private int _problemCount = 0;
    //_RightHPD,_LeftHPDの値を１回だけ作るため
    private bool DecidedMainValue = false;
    //右か、左か、選ばない、を選んだ後に確定するためのbool
    public bool _confirm = false;
    //ラウンド
    public enum RoundLevel
    {
        None,
        First,
        Second,
        Third,
        Fourth,
        Fifth,
        Sixth,
        ToEnd,
    }

    public RoundLevel _roundLevel = RoundLevel.None;

    void Update()
    {
        MainValue();
        //Confirmをtrueにするのは、
        if(_confirm) MainGame();
        // ここの処理は、人を引いたあとに行う
       
        //ラウンド内のMainGameの処理を３回行ったら
        if (_problemCount >= 3)
        {
            //３回行ったら、ショップに行ってアイテムを買う/使う
            //ラウンドが進む
            ChangeRound();
            Debug.Log($"ラウンド：{_roundLevel}");
            _problemCount = 0;
            return;
        }
        switch (_roundLevel)
        {
            case RoundLevel.None:
                break;
            case RoundLevel.First:
                break;
            case RoundLevel.Second:
                break;
            case RoundLevel.Third:
                break;
            case RoundLevel.Fourth:
                break;
            case RoundLevel.Fifth:
                break;
            case RoundLevel.Sixth:
                break;
            case RoundLevel.ToEnd:
                break;
        }
    }

    /// <summary>
    /// ラウンドを変える処理・一定のgoldが溜まっているなら通れる
    /// </summary>
    private void ChangeRound()
    {
        if (_roundLevel == RoundLevel.None)
        {
            //ラウンドFirstに行く処理
            _roundLevel = RoundLevel.First;
        }
        else if (_roundLevel == RoundLevel.First)
        {
            //secondラウンドに行く処理
            _roundLevel = RoundLevel.Second;
        }
        else if (_roundLevel == RoundLevel.Second)
        {
            //thirdラウンドに行く処理
            _roundLevel = RoundLevel.Third;
        }
        else if (_roundLevel == RoundLevel.Third)
        {
            //fourthラウンドに行く処理
            _roundLevel = RoundLevel.Fourth;
        }
        else if (_roundLevel == RoundLevel.Fourth)
        {
            //fifthラウンドに行く処理
            _roundLevel = RoundLevel.Fifth;
        }
        else if (_roundLevel == RoundLevel.Fifth)
        {
            //toendに行く処理
            _roundLevel = RoundLevel.ToEnd;
        }
    }

    private void MainValue()
    {
        if (DecidedMainValue)
        {
            return;
        }
        //右の人の役職、人数を決める-->ok
        //左の人の役職、人数を決める-->ok
        _RightHPD = teamBuilder.TeamBuild();
        _LeftHPD = teamBuilder.TeamBuild();
        Debug.Log($"rightrole:{_RightHPD.SelectRole}\nrightperson:{_RightHPD.Personnum}\nrightinheritance:{_RightHPD.Inheritance}\nrighttotalinheritance:{_RightHPD.TotalInheritance}\nrightGuiltyPercent:{_RightHPD.GuiltyPercent}\nrighttotalGuiltypercent:{_RightHPD.TotalGuiltyPercent}");
        Debug.Log($"leftrole:{_LeftHPD.SelectRole}\nleftperson:{_LeftHPD.Personnum}\nleftinheritance:{_LeftHPD.Inheritance}\nlefttotalinheritance:{_LeftHPD.TotalInheritance}\nleftGuiltyPercent:{_LeftHPD.GuiltyPercent}\nlefttotalGuiltypercent:{_LeftHPD.TotalGuiltyPercent}");
        //役職が右と左で同じじゃないようにする
        if (_RightHPD.SelectRole != _LeftHPD.SelectRole)
        {
            DecidedMainValue = true;
        }

    }

    private void MainGame()
    {
        if (_confirm)
        {
            if (trainController._lastDirection == TrainController.Direction.Right)
            {
                Debug.Log("右確定");
                _TotalGuiltyPercent = _RightHPD.TotalGuiltyPercent;
                _TotalInheritance = _RightHPD.TotalInheritance;
                Debug.Log($"totalG:{TotalGuiltyPercent}totalI{TotalInheritance}");
            }
            else if (trainController._lastDirection == TrainController.Direction.Left)
            {
                Debug.Log("左確定");
                _TotalGuiltyPercent = _LeftHPD.TotalGuiltyPercent;
                _TotalInheritance = _LeftHPD.TotalInheritance;
            }
            //人を引いたあとに処理する↓
            SelectSide(_TotalGuiltyPercent, _TotalInheritance);
            ++_problemCount;
            Debug.Log($"problem:{_problemCount}");
            DecidedMainValue = false;
            _confirm = false;
            Debug.Log($"gold:{Gold}guilty:{GuiltyPercent}");
            return;
        }
        //これらの処理を1ラウンドに３回行う
    }

    private void SelectSide(int TotalGuiltyPercent, int TotalInheritance)
    {
        GuiltyPercent += TotalGuiltyPercent;
        Gold += TotalInheritance;
        total.text = $"Gold:{Gold}\nGuilt:{GuiltyPercent}";
        Debug.Log($"GuiltyPercent:{GuiltyPercent}\nGold:{Gold}");
        return;
    }
    void Start()
    {

    }
}
