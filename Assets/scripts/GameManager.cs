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
    //自分が持っているお金
    public int Gold = 0;
    //罪率
    public int GuiltyPercent = 0;
    //給料
    public int Salary = 12;

    //選択肢
    private bool _isRight;
    private bool _isLeft;
    private bool _isNothing;

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
        ToEnd,
    }

    /// <summary>
    /// ラウンドを変えます。
    /// </summary>
    public RoundLevel _roundLevel = RoundLevel.None;

    void Update()
    {
        MainValue();

        //Arrowキーを押したらSelectTeamのbool値を変えてmaingameの処理をする
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectTeam(true, false, false);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectTeam(false, true, false);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SelectTeam(false, false, true);
        }
        //enterで確定
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            Confirm = true;
            MainGame();
        }
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
            case RoundLevel.ToEnd:
                break;
        }
    }

    /// <summary>
    /// ラウンドを変える処理
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
        if (DecidedMainValue) return;
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
        // //右の人の役職、人数を決める-->ok
        // //左の人の役職、人数を決める-->ok
        // var Right = teamBuilder.TeamBuild();
        // var Left = teamBuilder.TeamBuild();

        // //右、左どちらかを選んだら、罪率が上がる＆所持金が上がる
        // if (_isRight)
        // {
        //     // Debug.Log("右");
        //     _TotalGuiltyPercent = _RightHPD.TotalGuiltyPercent;
        //     _TotalInheritance = _RightHPD.TotalInheritance;
        //     return;
        // }
        // else if (_isLeft)
        // {
        //     // Debug.Log("左");
        //     _TotalGuiltyPercent = _LeftHPD.TotalGuiltyPercent;
        //     _TotalInheritance = _LeftHPD.TotalInheritance;
        //     return;
        // }
        // //右か左か選ばないかを決める-->ok-->SelectTeam
        // //どちらも選ばない場合は、所持金が規定量増える(給料)
        // else if (_isNothing)
        // {
        //     //Debug.Log("選ばない");
        //     return;
        // }

        if (Confirm)
        {
            if (_isRight)
            {
                Debug.Log("右確定");
                
            }
            else if (_isLeft)
            {
                Debug.Log("左確定");
            }
            else if (_isNothing)
            {
                Debug.Log("選ばない確定");
            }
            SelectSide(TotalGuiltyPercent, TotalInheritance);
                ++_problemCount;
                DecidedMainValue = false;
                SelectTeam(false, false, false);
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
    /// <summary>
    /// 選択肢を管理する＆選んだ時にゲッターに入れる
    /// </summary>
    /// <param name="isRight"></param>
    /// <param name="isLeft"></param>
    /// <param name="isNothing"></param>
    private void SelectTeam(bool isRight, bool isLeft, bool isNothing)
    {
        _isRight = isRight;
        _isLeft = isLeft;
        _isNothing = isNothing;
        //右、左どちらかを選んだら、罪率が上がる＆所持金が上がる
        if (_isRight)
        {
            // Debug.Log("右");
            _TotalGuiltyPercent = _RightHPD.TotalGuiltyPercent;
            _TotalInheritance = _RightHPD.TotalInheritance;
        }
        else if (_isLeft)
        {
            // Debug.Log("左");
            _TotalGuiltyPercent = _LeftHPD.TotalGuiltyPercent;
            _TotalInheritance = _LeftHPD.TotalInheritance;
        }
        //右か左か選ばないかを決める-->ok-->SelectTeam
        //どちらも選ばない場合は、所持金が規定量増える(給料)
        else if (_isNothing)
        {
            //Debug.Log("選ばない");
            _TotalGuiltyPercent = 0;
            _TotalInheritance = Salary;
        }
    }

    void Start()
    {
        AudioManager.Instance.PlayBGM(AudioManager.E_BGMType.罪人の街);
        //MainGame();
    }
}
