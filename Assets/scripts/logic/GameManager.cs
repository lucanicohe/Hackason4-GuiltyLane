using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    TextMeshProUGUI total;
    public static bool ChooseDisguised;
    private TeamBuilder.HitPersonData _RightHPD;
    public TeamBuilder.HitPersonData RightHPD
    {
        get
        {
            if (ChooseDisguised || _roundLevel == RoundLevel.Third
            || _roundLevel == RoundLevel.Fifth
            || _roundLevel == RoundLevel.Sixth)
            {
                return DisguisedPersonData(_RightHPD);
            }
            return _RightHPD;
        }
        private set { }
    }

    private TeamBuilder.HitPersonData _LeftHPD;
    public TeamBuilder.HitPersonData LeftHPD
    {
        get
        {
            if (!ChooseDisguised || _roundLevel == RoundLevel.Third
            || _roundLevel == RoundLevel.Fifth
            || _roundLevel == RoundLevel.Sixth)
            {
                return DisguisedPersonData(_LeftHPD);
            }
            return _LeftHPD;
        }
        private set { }
    }

    [SerializeField]
    TeamBuilder teamBuilder;
    [SerializeField]
    TrainController trainController;

    //自分が持っているお金
    public int Gold = 0;
    private int GoldForShow
    {
        get
        {
            if (_roundLevel == RoundLevel.Sixth)
            {
                return -1;
            }
            return Gold;
        }
    }

    //罪率
    public int GuiltyPercent = 0;
    private int GuiltyPercentForShow
    {
        get
        {
            if (_roundLevel == RoundLevel.Sixth)
            {
                return -1;
            }
            return GuiltyPercent;
        }
    }
    //給料
    public int Salary = 12;
    public int Toll = 15;
    private int[] TollArray = new int[] { 0, 15, 30, 60, 100, 150, 200 };


    //合計のGuiltyPercent
    private int _TotalGuiltyPercent;
    public int TotalGuiltyPercent => _TotalGuiltyPercent;

    //合計のInheritance
    private int _TotalInheritance;
    public int TotalInheritance => _TotalInheritance;

    //ラウンド内の何回行うか-->子の変数が3以上になったら次のラウンドへ進む
    private int _problemCount = 1;
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

    public RoundLevel _roundLevel = RoundLevel.First;

    void Update()
    {
        MainValue();
        //Confirmをtrueにするのは、
        if (_confirm)
        {
            _confirm = false;
            MainGame();
            if (GuiltyPercent >= 100)
            {
                Debug.Log("ゲームクリア");
                SceneManager.LoadScene("ResultScene");
                return;
            }
        }
        // ここの処理は、人を引いたあとに行う


        // switch (_roundLevel)
        // {
        //     case RoundLevel.None:
        //         break;
        //     case RoundLevel.First:
        //         break;
        //     case RoundLevel.Second:
        //         break;
        //     case RoundLevel.Third:
        //         break;
        //     case RoundLevel.Fourth:
        //         break;
        //     case RoundLevel.Fifth:
        //         break;
        //     case RoundLevel.Sixth:
        //         break;
        //     case RoundLevel.ToEnd:
        //         break;
        // }
    }

    /// <summary>
    /// ラウンドを変える処理・一定のgoldが溜まっているなら通れる
    /// </summary>
    private void ChangeRound()
    {
        _roundLevel++;
        Toll = TollArray[(int)_roundLevel];

        // if (_roundLevel == RoundLevel.None)
        // {
        //     //ラウンドFirstに行く処理
        //     _roundLevel = RoundLevel.First;
        //     Toll = 30;
        // }
        // else if (_roundLevel == RoundLevel.First)
        // {
        //     //secondラウンドに行く処理
        //     _roundLevel = RoundLevel.Second;
        //     Toll = 60;
        // }
        // else if (_roundLevel == RoundLevel.Second)
        // {
        //     //thirdラウンドに行く処理
        //     _roundLevel = RoundLevel.Third;
        //     Toll = 100;
        // }
        // else if (_roundLevel == RoundLevel.Third)
        // {
        //     //fourthラウンドに行く処理
        //     _roundLevel = RoundLevel.Fourth;
        //     Toll = 150;
        // }
        // else if (_roundLevel == RoundLevel.Fourth)
        // {
        //     //fifthラウンドに行く処理
        //     _roundLevel = RoundLevel.Fifth;
        //     Toll = 200;
        // }
        // else if (_roundLevel == RoundLevel.Fifth)
        // {
        //     //toendに行く処理
        //     _roundLevel = RoundLevel.ToEnd;
        //     Toll = 300;
        // }
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
            ChooseDisguised = Random.value < 0.5;
        }
    }

    private TeamBuilder.HitPersonData DisguisedPersonData(TeamBuilder.HitPersonData data)
    {
        switch (_roundLevel)
        {
            case RoundLevel.First:
                return data;
            case RoundLevel.Second:
                //どちらかの身分が分からなくなる
                //お金もわからくなる
                data.SelectRole = "???";
                data.TotalInheritance = -1;
                return data;
            case RoundLevel.Third:
                //両方の身分が分からなくなる
                data.SelectRole = "???";
                data.TotalInheritance = -1;
                return data;
            case RoundLevel.Fourth:
                //更にどちらかの人数も分からなくなる
                data.SelectRole = "???";
                data.Personnum = -1;
                data.TotalInheritance = -1;
                data.TotalGuiltyPercent = -1;
                return data;
            case RoundLevel.Fifth:
                data.SelectRole = "???";
                data.Personnum = -1;
                data.TotalInheritance = -1;
                data.TotalGuiltyPercent = -1;
                return data;
            case RoundLevel.Sixth:
                //自分の罪率と金が分からなくなる
                data.SelectRole = "???";
                data.Personnum = -1;
                data.TotalInheritance = -1;
                data.TotalGuiltyPercent = -1;
                return data;

            default: return data;
        }
    }

    private void MainGame()
    {
        if (trainController._lastDirection == TrainController.Direction.Right)
        {
            Debug.Log("右確定");
            _TotalGuiltyPercent = _RightHPD.TotalGuiltyPercent;
            _TotalInheritance = _RightHPD.TotalInheritance;
        }
        else if (trainController._lastDirection == TrainController.Direction.Left)
        {
            Debug.Log("左確定");
            _TotalGuiltyPercent = _LeftHPD.TotalGuiltyPercent;
            _TotalInheritance = _LeftHPD.TotalInheritance;
        }
        //人を引いたあとに処理する↓
        SelectSide(_TotalGuiltyPercent, _TotalInheritance);
        Debug.Log($"problem:{_problemCount}");
        DecidedMainValue = false;
        Debug.Log($"gold:{Gold}guilty:{GuiltyPercent}");

        ++_problemCount;

        //これらの処理を1ラウンドに３回行う
    }

    private void SelectSide(int TotalGuiltyPercent, int TotalInheritance)
    {
        GuiltyPercent += TotalGuiltyPercent;
        Gold += TotalInheritance;
        //ラウンド内のMainGameの処理を３回行ったら
        if (_problemCount > 3)
        {
            if (Gold - Toll < 0)
            {
                Debug.Log("ゲームオーバー");
                SceneManager.LoadScene("ResultScene");
            }
            else
            {
                Gold -= Toll;
                Gold += Salary;
            }
            //３回行ったら、ショップに行ってアイテムを買う/使う
            //ラウンドが進む
            ChangeRound();
            Debug.Log($"ラウンド：{_roundLevel}");
            _problemCount = 1;
        }
        total.text = $"Gold : {(GoldForShow == -1 ? "???" : GoldForShow)}\nGuilt : {(GuiltyPercentForShow == -1 ? "???" : GuiltyPercentForShow)}\nNextToll : {Toll}\nRound : {_roundLevel}-{_problemCount}";
        Debug.Log($"GuiltyPercent:{GuiltyPercent}\nGold:{Gold}");
        return;
    }
    void Start()
    {
        total.text = $"Gold : {GoldForShow}\nGuilt : {GuiltyPercentForShow}\nNextToll : {Toll}\nRound : {_roundLevel}-{_problemCount}";
    }
}
