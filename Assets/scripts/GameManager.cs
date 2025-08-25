using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : Singleton<GameManager>
{
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

    private void ChangeFirstRound()
    {
        //ラウンドFirstに行く処理
        _roundLevel = RoundLevel.First;
    }
    private void ChangeSecondRound()
    {
        //secondラウンドに行く処理
        _roundLevel = RoundLevel.Second;
    }
    private void ChangeThirdRound()
    {
        //thirdラウンドに行く処理
        _roundLevel = RoundLevel.Third;
    }
    private void ChangeFourthRound()
    {
        //fourthラウンドに行く処理
        _roundLevel = RoundLevel.Fourth;
    }
    private void ChangeFifthRound()
    {
        //fifthラウンドに行く処理
        _roundLevel = RoundLevel.Fifth;
    }
    private void ChangeToEndRound()
    {
        //toendに行く処理
        _roundLevel = RoundLevel.ToEnd;
    }

    private void MainGame()
    {
        //右の人の役職、人数を決める-->ok
        //左の人の役職、人数を決める-->ok
        var Right = teamBuilder.TeamBuild();
        var Left = teamBuilder.TeamBuild();
        //右、左どちらかを選んだら、罪率が上がる＆所持金が上がる
        if (_isRight)
        {
            _TotalGuiltyPercent = Right.TotalGuiltyPercent;
            _TotalInheritance = Right.TotalInheritance;
            SelectSide(TotalGuiltyPercent, TotalInheritance);
            return;
        }
        else if (_isLeft)
        {
            _TotalGuiltyPercent = Left.TotalGuiltyPercent;
            _TotalInheritance = Left.TotalInheritance;
            SelectSide(TotalGuiltyPercent, TotalInheritance);
            return;
        }
        //右か左か選ばないかを決める-->ok-->SelectTeam
        //どちらも選ばない場合は、所持金が規定量増える(給料)
        else if (_isNothing)
        {
            Gold += Salary;
            return;
        }        
        
        
        //これらの処理を1ラウンドに３回行う
        //３回行ったら、ショップに行ってアイテムを買う/使う
        //ラウンドが進む
    }

    private void SelectSide(int TotalGuiltyPercent, int TotalInheritance)
    {
        GuiltyPercent += TotalGuiltyPercent;
        Gold += TotalInheritance;
        return;
    }
    /// <summary>
    /// 選択肢を管理する
    /// </summary>
    /// <param name="isRight"></param>
    /// <param name="isLeft"></param>
    /// <param name="isNothing"></param>
    private void SelectTeam(bool isRight, bool isLeft, bool isNothing)
    {
        _isRight = isRight;
        _isLeft = isLeft;
        _isNothing = isNothing;
    }

    void Start()
    {
        AudioManager.Instance.PlayBGM(AudioManager.E_BGMType.罪人の街);
    }
}
