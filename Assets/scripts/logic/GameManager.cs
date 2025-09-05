using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
    public const int BE_DISGUISED_INT = -1;
    public const string BE_DISGUISED_STRING = "■■■■";
    [SerializeField]
    TextMeshProUGUI _goldGuiltText;
    [SerializeField]
    TextMeshProUGUI _tollForkText;

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
                if (_isOfficialIdentityHintEffect && ChooseDisguised)
                {
                    _RightHPD.SelectRoleToShow = _RightHPD.SelectRole;
                }
                if (_isOfficialNumberHintEffect && ChooseDisguised)
                {
                    _RightHPD.PersonnumToShow = _RightHPD.Personnum;
                }
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
                if (_isOfficialIdentityHintEffect && !ChooseDisguised)
                {
                    _LeftHPD.SelectRoleToShow = _LeftHPD.SelectRole;
                }
                if (_isOfficialNumberHintEffect && !ChooseDisguised)
                {
                    _RightHPD.PersonnumToShow = _RightHPD.Personnum;
                }
                return DisguisedPersonData(_LeftHPD);
            }
            return _LeftHPD;
        }
        private set { }
    }

    [SerializeField]
    private float GetGoldGuilty = 6;
    [SerializeField]
    TeamBuilder teamBuilder;
    [SerializeField]
    TrainController trainController;
    [SerializeField]
    Fade fade;
    public bool _speedzero = false; //電車のスピードをゼロにする

    //自分が持っているお金
    public int Gold = 0;
    private int GoldForShow
    {
        get
        {
            if (_roundLevel == RoundLevel.Sixth)
            {
                return BE_DISGUISED_INT;
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
                return BE_DISGUISED_INT;
            }
            return GuiltyPercent;
        }
    }
    //給料
    public int Salary = 12;
    //通行料
    public int Toll = 15;
    //ラウンドごとの給料
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
        if (_roundLevel == RoundLevel.ToEnd)
        {
            HandleGameOver(true);
            return;
        }
        MainValue();
        //Confirmをtrueにするのは、
        if (_confirm)
        {
            _confirm = false;
            StartCoroutine(startmaingame());
        }
        // ここの処理は、人を引いたあとに行う
    }

    /// <summary>
    /// ラウンドを変える処理・一定のgoldが溜まっているなら通れる
    /// </summary>
    private void ChangeRound()
    {
        _roundLevel++;
        Toll = TollArray[(int)_roundLevel];
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
        //役職が右と左で同じじゃないようにする
        if (_RightHPD.SelectRole != _LeftHPD.SelectRole)
        {
            DecidedMainValue = true;
            InitialiZationOfItem();
            ChooseDisguised = UnityEngine.Random.value < 0.5;
            _RightHPD = InitialiZationOfersonData(_RightHPD);
            _LeftHPD = InitialiZationOfersonData(_LeftHPD);
            if (_isDealEffect)
            {
                if (_RightHPD.RoleID == 3)
                {
                    _RightHPD.TotalInheritance += _RightHPD.Personnum;
                }
                if (_LeftHPD.RoleID == 3)
                {
                    _RightHPD.TotalInheritance += _RightHPD.Personnum;
                }
            }
            if (_isInheritanceSystemEffect)
            {
                if (_RightHPD.RoleID == 3)
                {
                    _RightHPD.TotalInheritance *= 2;
                }
                if (_LeftHPD.RoleID == 3)
                {
                    _RightHPD.TotalInheritance *= 2;
                }
            }
            Debug.Log(JsonUtility.ToJson(_RightHPD, true));
            Debug.Log(JsonUtility.ToJson(_LeftHPD, true));
        }
    }

    private TeamBuilder.HitPersonData InitialiZationOfersonData(TeamBuilder.HitPersonData data)
    {
        data.PersonnumToShow = data.Personnum;
        data.TotalInheritanceToShow = data.TotalInheritance;
        data.TotalGuiltyPercentToShow = data.TotalGuiltyPercent;
        return data;
    }
    private TeamBuilder.HitPersonData DisguisedPersonData(TeamBuilder.HitPersonData data)
    {
        data = InitialiZationOfersonData(data);
        switch (_roundLevel)
        {
            case RoundLevel.First:
                return data;
            case RoundLevel.Second:
                //どちらかの身分が分からなくなる
                //お金もわからくなる
                data.SelectRoleToShow = BE_DISGUISED_STRING;
                data.TotalInheritanceToShow = BE_DISGUISED_INT;
                return data;
            case RoundLevel.Third:
                //両方の身分が分からなくなる
                data.SelectRoleToShow = BE_DISGUISED_STRING;
                data.TotalInheritanceToShow = BE_DISGUISED_INT;
                return data;
            case RoundLevel.Fourth:
            case RoundLevel.Fifth:
            case RoundLevel.Sixth:
                //自分の罪率と金が分からなくなる
                data.SelectRoleToShow = BE_DISGUISED_STRING;
                data.PersonnumToShow = BE_DISGUISED_INT;
                data.TotalInheritanceToShow = BE_DISGUISED_INT;
                data.TotalGuiltyPercentToShow = BE_DISGUISED_INT;
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
            switch (_RightHPD.RoleID)
            {
                case 0:
                    ResultData.TotleRoyalKilledNum += _RightHPD.Personnum;
                    break;
                case 1:
                    ResultData.TotleNobleKilledNum += _RightHPD.Personnum;
                    break;
                case 2:
                    ResultData.TotleCitizenKilledNum += _RightHPD.Personnum;
                    break;
                case 3:
                    ResultData.TotlePrisonerKilledNum += _RightHPD.Personnum;
                    break;
            }
        }
        else if (trainController._lastDirection == TrainController.Direction.Left)
        {
            Debug.Log("左確定");
            _TotalGuiltyPercent = _LeftHPD.TotalGuiltyPercent;
            _TotalInheritance = _LeftHPD.TotalInheritance;
            switch (_RightHPD.RoleID)
            {
                case 0:
                    ResultData.TotleRoyalKilledNum += _RightHPD.Personnum;
                    break;
                case 1:
                    ResultData.TotleNobleKilledNum += _RightHPD.Personnum;
                    break;
                case 2:
                    ResultData.TotleCitizenKilledNum += _RightHPD.Personnum;
                    break;
                case 3:
                    ResultData.TotlePrisonerKilledNum += _RightHPD.Personnum;
                    break;
            }
        }
        ++_problemCount;
        //人を引いたあとに処理する↓
        SelectSide(_TotalGuiltyPercent, _TotalInheritance);
        Debug.Log($"problem:{_problemCount}");
        DecidedMainValue = false;
        Debug.Log($"gold:{Gold}guilty:{GuiltyPercent}");

        //これらの処理を1ラウンドに３回行う
    }

    private void SelectSide(int TotalGuiltyPercent, int TotalInheritance)
    {
        GuiltyPercent += TotalGuiltyPercent;
        Gold += TotalInheritance;
        ResultData.TotalGuiltyPercentEarned += TotalGuiltyPercent;
        ResultData.TotleGoldEarned += TotalInheritance;

        //ラウンド内のMainGameの処理を３回行ったら
        if (GuiltyPercent >= 100)
        {
            HandleGameOver(false);
            return;
        }
        if (_problemCount > 3)
        {
            if (Gold - Toll < 0)
            {
                HandleGameOver(false);
                return;
            }
            else
            {
                Gold -= Toll;
                Gold += Salary;
                ResultData.TotleGoldEarned += Salary;
                ResultData.TotalTollPayed += Toll;
            }
            //３回行ったら、ショップに行ってアイテムを買う/使う
            //ラウンドが進む
            ChangeRound();
            Debug.Log($"ラウンド：{_roundLevel}");
            _problemCount = 1;
        }
        _goldGuiltText.text = $"Gold : {(GoldForShow == BE_DISGUISED_INT ? BE_DISGUISED_STRING : GoldForShow)}\nGuilt : {(GuiltyPercentForShow == BE_DISGUISED_INT ? BE_DISGUISED_STRING : GuiltyPercentForShow)}%";
        _tollForkText.text = $"Toll\n{Toll}\n\nFork\n{(int)_roundLevel}-{_problemCount}";
        return;
    }

    /// <summary>
    /// Gold,Guiltyを受け取るタイミングを人を轢いたあとにした
    /// </summary>
    /// <returns></returns>
    private IEnumerator startmaingame()
    {
        yield return new WaitForSeconds(GetGoldGuilty);
        MainGame();
    }
    void Start()
    {
        _goldGuiltText.text = $"Gold  : {GoldForShow}\nGuilt : {GuiltyPercentForShow}%";
        _tollForkText.text = $"Toll\n{Toll}\n\nFork\n{(int)_roundLevel}-{_problemCount}";
    }

    private void HandleGameOver(bool isGameClear)
    {
        if (isGameClear)
        {
            Debug.Log("ゲームクリア");
        }
        else
        {
            Debug.Log("ゲームオーバー");
        }
        _speedzero = true;
        AudioManager.Instance.StopAllBGM();
        AudioManager.Instance.StopAllSE();
        ResultData.RoundLevel = _roundLevel;
        ResultData.problemCount = _problemCount;
        fade.FadeIn(2, () => { SceneManager.LoadScene("ResultScene"); });
    }


    #region itemMethod

    private void InitialiZationOfItem()
    {
        _isOfficialIdentityHintEffect = false;
        _isOfficialNumberHintEffect = false;
    }
    private bool _isOfficialIdentityHintEffect = false;
    private bool _isOfficialNumberHintEffect = false;
    private bool _isDealEffect = false;
    private bool _isInheritanceSystemEffect = false;

    public bool OfficialIdentityHint()
    {
        if (RightHPD.SelectRoleToShow.Equals(BE_DISGUISED_STRING)
        || LeftHPD.SelectRoleToShow.Equals(BE_DISGUISED_STRING))
        {
            _isOfficialIdentityHintEffect = true;
            return true;
        }
        return false;
    }

    public bool OfficialNumberHint()
    {
        if (RightHPD.PersonnumToShow == BE_DISGUISED_INT
        || LeftHPD.PersonnumToShow == BE_DISGUISED_INT)
        {
            _isOfficialNumberHintEffect = true;
            return true;
        }
        return false;
    }

    public bool Deal()
    {
        if (_isDealEffect)
        {
            return false;
        }
        _isDealEffect = true;
        return true;
    }

    public bool InheritanceSystem()
    {
        if (_isInheritanceSystemEffect)
        {
            return false;
        }
        _isInheritanceSystemEffect = true;
        return true;
    }
    #endregion
}

public static class ResultData
{
    public static int TotleRoyalKilledNum = 0;
    public static int TotleNobleKilledNum = 0;
    public static int TotleCitizenKilledNum = 0;
    public static int TotlePrisonerKilledNum = 0;
    public static int TotleGoldEarned = 0;
    public static int TotalGuiltyPercentEarned = 0;
    public static GameManager.RoundLevel RoundLevel  = GameManager.RoundLevel.First;
    public static int problemCount = 0;
    public static int TotalTollPayed = 0;
}
