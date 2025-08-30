using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.PlayerLoop;

public class GameManager : Singleton<GameManager>
{
    private TeamBuilder.HitPersonData _RightHPD;
    private TeamBuilder.HitPersonData _LeftHPD;
    [SerializeField]
    TeamBuilder teamBuilder;

    [SerializeField]
    PrefabController prefabController;

    //自分が持っているお金
    public int Gold = 0;
    //罪率
    public int GuiltyPercent = 0;
    //給料
    public int Salary = 12;

    //選択肢->0:no,1:right,2:left
    private int _selectTeam = 0;

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
    private bool Confirm = false;
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

        //Arrowキーを押したらSelectTeamのbool値を変えてmaingameの処理をする
        if (Input.GetKeyDown(KeyCode.D))
        {
            _selectTeam = 1;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _selectTeam = 2;
        }
        //Confirmをtrueにするのは、
        // Confirm = true;
        // ここの処理は、人を引いたあとに行う
        //  MainGame();
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
            prefabController.SetPrefabRole(_RightHPD, _LeftHPD);

            if (prefabController.IsPrefabDestroy)
            {
                prefabController.MakePrefab(_RightHPD, _LeftHPD);
            }

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
        if (Confirm)
        {
            if (_selectTeam == 1)
            {
                Debug.Log("右確定");
                _TotalGuiltyPercent = _RightHPD.TotalGuiltyPercent;
                _TotalInheritance = _RightHPD.TotalInheritance;
                StartCoroutine(prefabController.MoveRightPrefab(_RightHPD, _LeftHPD));
            }
            else if(_selectTeam == 2)
            {
                Debug.Log("左確定");
                _TotalGuiltyPercent = _LeftHPD.TotalGuiltyPercent;
                _TotalInheritance = _LeftHPD.TotalInheritance;
                StartCoroutine(prefabController.MoveLeftPrefab(_RightHPD, _LeftHPD));
            }
            //人を引いたあとに処理する↓
            SelectSide(_TotalGuiltyPercent, _TotalInheritance);
            ++_problemCount;
            DecidedMainValue = false;
            _selectTeam = 0;
            Confirm = false;
            return;
        }
        //これらの処理を1ラウンドに３回行う
    }

    private void SelectSide(int TotalGuiltyPercent, int TotalInheritance)
    {
        GuiltyPercent += TotalGuiltyPercent;
        Gold += TotalInheritance;
        Debug.Log($"GuiltyPercent:{GuiltyPercent}\nGold:{Gold}");
        return;
    }
    void Start()
    {
       
    }
}
