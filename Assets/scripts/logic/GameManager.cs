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
    [SerializeField] public TextMeshProUGUI _infoText;
    [SerializeField] private AudioSource _goldsound; 

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
                TeamBuilder.HitPersonData data = DisguisedPersonData(_RightHPD);
                if (_isOfficialIdentityHintEffect && ChooseDisguised)
                {
                    data.SelectRoleToShow = data.SelectRole;
                }
                if (_isOfficialNumberHintEffect && ChooseDisguised)
                {
                    data.PersonnumToShow = data.Personnum;
                }
                return data;
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
                TeamBuilder.HitPersonData data = DisguisedPersonData(_LeftHPD);
                if (_isOfficialIdentityHintEffect && ChooseDisguised)
                {
                    data.SelectRoleToShow = data.SelectRole;
                }
                if (_isOfficialNumberHintEffect && ChooseDisguised)
                {
                    data.PersonnumToShow = data.Personnum;
                }
                return data;
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
    public int Toll {
        get
        {
            return TollArray[(int)_roundLevel];
        }
        private set { } }
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
        GoldAndGuiltTextUpdate();
        if (_roundLevel == RoundLevel.ToEnd)
        {
            HandleGameOver(true);
            return;
        }
        if (_shopCoroutine == null && _mainGamestartCoroutine == null)
        {
            MainValue();
        }
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
        if (_isFinancialExpertEffect)
        {
            Toll = Mathf.RoundToInt(Toll * 0.8f);
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
        //役職が右と左で同じじゃないようにする
        if (_RightHPD.SelectRole != _LeftHPD.SelectRole)
        {
            if (_isGuaranteeEffect
            && !(RightHPD.SelectRole == "Prisoner" || LeftHPD.SelectRole == "Prisoner"))
            {
                return;
            }
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
                    _RightHPD.TotalInheritance = Mathf.RoundToInt(_RightHPD.TotalInheritance * 1.5f);
                }
                if (_LeftHPD.RoleID == 3)
                {
                    _LeftHPD.TotalInheritance = Mathf.RoundToInt(_LeftHPD.TotalInheritance * 1.5f);
                }
            }
            Debug.Log(JsonUtility.ToJson(_RightHPD, true));
            Debug.Log(JsonUtility.ToJson(_LeftHPD, true));
        }
    }

    private TeamBuilder.HitPersonData InitialiZationOfersonData(TeamBuilder.HitPersonData data)
    {
        data.SelectRoleToShow = data.SelectRole;
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
        var tempTotalGuiltyPercent = 0;
        var tempTotalInheritance = 0;
        if (trainController._lastDirection == TrainController.Direction.Right)
        {
            Debug.Log("右確定");
            _TotalGuiltyPercent = _RightHPD.TotalGuiltyPercent;
            _TotalInheritance = _RightHPD.TotalInheritance;
            tempTotalGuiltyPercent = _LeftHPD.TotalGuiltyPercent;
            tempTotalInheritance = _LeftHPD.TotalInheritance;
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
            tempTotalGuiltyPercent = _RightHPD.TotalGuiltyPercent;
            tempTotalInheritance = _RightHPD.TotalInheritance;
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
        SelectSide(_TotalGuiltyPercent, _TotalInheritance, tempTotalGuiltyPercent, tempTotalInheritance);
        Debug.Log($"problem:{_problemCount}");
        DecidedMainValue = false;
        Debug.Log($"gold:{Gold}guilty:{GuiltyPercent}");
        _goldsound.Play();

        //これらの処理を1ラウンドに３回行う
    }

    private void SelectSide(int TotalGuiltyPercent, int TotalInheritance, int otherTotalGuiltyPercent, int otherTotalInheritance)
    {
        var tempGuiltyPercent = TotalGuiltyPercent;
        var tempInheritance = TotalInheritance;

        if (_isFamilyDealEffect)
        {
            tempInheritance += otherTotalInheritance;
            tempGuiltyPercent += Mathf.RoundToInt(TotalGuiltyPercent * 1.1f);
        }
        if (_isDefenseAttorneyEffect)
        {
            tempInheritance += Mathf.RoundToInt(tempInheritance * 1.2f);
        }
        if (_isCollectorEffect)
        {
            tempInheritance += Mathf.RoundToInt(tempInheritance * (1 + GuiltyPercent * 0.01f));
        }
        if (_isInvestmentEffect)
        {
            tempInheritance += Mathf.RoundToInt(tempInheritance * 5.0f);
        }
        Gold += tempInheritance;
        GuiltyPercent += tempGuiltyPercent;

        ResultData.TotalGuiltyPercentEarned += TotalGuiltyPercent;
        ResultData.TotleGoldEarned += TotalInheritance;

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
            // _shopCoroutine = StartCoroutine(ShopSequenceCoroutine());
        }
        GoldAndGuiltTextUpdate();
        _shopCoroutine = StartCoroutine(ShopSequenceCoroutine());
    }

    private void GoldAndGuiltTextUpdate()
    {
        _goldGuiltText.text = $"Gold : {(GoldForShow == BE_DISGUISED_INT ? BE_DISGUISED_STRING : GoldForShow)}\nGuilt : {(GuiltyPercentForShow == BE_DISGUISED_INT ? BE_DISGUISED_STRING : GuiltyPercentForShow)}%";
        _tollForkText.text = $"Toll\n{Toll}\n\nFork\n{(int)_roundLevel}-{_problemCount}";
    }

    [Header("Shop")]
    [SerializeField] private GameObject _shopAlertImage;
    [SerializeField] private ShopManager _shopManager;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private TMP_Text _shopCountdownText;
    [SerializeField] private float _shopOpenTime = 30;
    [SerializeField] private float _mainGameRestartTime = 10;
    private Coroutine _shopCoroutine = null;
    private Coroutine _mainGamestartCoroutine = null;

    private IEnumerator ShopSequenceCoroutine()
    {
        if (_shopAlertImage != null) _shopAlertImage.SetActive(true);
        yield return new WaitForSeconds(3f);
        if (_shopAlertImage != null) _shopAlertImage.SetActive(false);

        // Disable inventory when shop opens
        if (_inventoryManager != null) _inventoryManager.InventoryActive = false;
        if (_shopManager != null) _shopManager.shop.SetActive(true);

        // Enable countdown text
        if (_shopCountdownText != null)
        {
            _shopCountdownText.gameObject.SetActive(true);
            float timer = _shopOpenTime;
            while (timer > 0f)
            {
                _shopCountdownText.text = $"Shop closes in: {Mathf.Ceil(timer)}";
                timer -= Time.deltaTime;
                yield return null;
            }
            _shopCountdownText.text = "Shop closed!";
            _shopCountdownText.gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(_shopOpenTime);
        }

        // Close shop and re-enable inventory
        if (_shopManager != null) _shopManager.shop.SetActive(false);
        if (_inventoryManager != null) _inventoryManager.InventoryActive = true;
        yield return TimerOfMainGameStart();
    }

    [SerializeField] private TMP_Text _mainGameCountdownText;
    private IEnumerator TimerOfMainGameStart()
    {
        if (_mainGameCountdownText != null)
        {
            _mainGameCountdownText.gameObject.SetActive(true);
            float timer = _mainGameRestartTime;
            while (timer > 0f)
            {
                _mainGameCountdownText.text = $"NextGame : {Mathf.Ceil(timer)}";
                timer -= Time.deltaTime;
                yield return null;
            }
            _mainGameCountdownText.text = "Start!!";
            _mainGameCountdownText.gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(_mainGameRestartTime);
        }
        if (_inventoryManager != null) _inventoryManager.InventoryActive = false;
        _shopCoroutine = null;
        _mainGamestartCoroutine = null;
    }

    public void _closeSHOP()
    {
        if(_shopCoroutine != null)StopCoroutine(_shopCoroutine);
        _mainGamestartCoroutine = StartCoroutine(TimerOfMainGameStart());
        if (_shopManager != null) _shopManager.shop.SetActive(false);
        if (_inventoryManager != null) _inventoryManager.InventoryActive = true;
    }

    public void StartMainGameButton()
    {
        if(_mainGamestartCoroutine != null)StopCoroutine(_mainGamestartCoroutine);
        if (_shopManager != null) _shopManager.shop.SetActive(false);
        if (_inventoryManager != null) _inventoryManager.InventoryActive = false;
        _shopCoroutine = null;
        _mainGamestartCoroutine = null;
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
        GoldAndGuiltTextUpdate();
        _infoText.text = "";
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
        ResultData.RoundLevel = _roundLevel;
        ResultData.problemCount = _problemCount;
        fade.FadeIn(2, () => { SceneManager.LoadScene("ResultScene"); });
    }


    #region itemMethod

    private void InitialiZationOfItem()
    {
        _isOfficialIdentityHintEffect = false;
        _isOfficialNumberHintEffect = false;
        _isGuaranteeEffect = false;
        _isDefenseAttorneyEffect = false;
        _isFinancialExpertEffect = false;
        _isBlessingOfThePriestEffect = false;
        _isChurchsBlessingEffect = false;
        _isInvestmentEffect = false;
    }
    private bool _isOfficialIdentityHintEffect = false;
    private bool _isOfficialNumberHintEffect = false;
    private bool _isDealEffect = false;//恒久効果
    private bool _isInheritanceSystemEffect = false;//恒久効果
    private bool _isGuaranteeEffect = false;
    private bool _isDefenseAttorneyEffect = false;
    private bool _isFinancialExpertEffect = false;
    private bool _isBlessingOfThePriestEffect = false;
    private bool _isChurchsBlessingEffect = false;
    private bool _isFamilyDealEffect = false; //恒久効果
    private bool _isCollectorEffect = false; //恒久効果
    private bool _isInvestmentEffect = false;



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

    public bool Guarantee()
    {
        if (_isGuaranteeEffect)
        {
            return false;
        }
        _isGuaranteeEffect = true;
        return true;
    }

    public bool DefenseAttorney()
    {
        if (_isDefenseAttorneyEffect)
        {
            return false;
        }
        _isDefenseAttorneyEffect = true;
        return true;
    }

    public bool FinancialExpert()
    {
        if (_isFinancialExpertEffect)
        {
            return false;
        }
        if (_problemCount != 3)
        {
            return false;
        }
        _isDefenseAttorneyEffect = true;
        return true;
    }

    public bool BlessingOfThePriest()
    {
        if (_isBlessingOfThePriestEffect)
        {
            return false;
        }
        GuiltyPercent = Mathf.RoundToInt(GuiltyPercent / 2f);
        _isBlessingOfThePriestEffect = true;
        return true;
    }

    public bool ChurchsBlessing()
    {
        if (_isChurchsBlessingEffect)
        {
            return false;
        }
        if (GuiltyPercent <= 50)
        {
            return false;
        }
        _isChurchsBlessingEffect = true;
        GuiltyPercent = 0;
        return true;
    }

    public bool PapalAmnesty()
    {
        var temp = Mathf.RoundToInt(GuiltyPercent * 0.9f);
        if (temp <= 0)
        {
            return false;
        }
        GuiltyPercent = temp;
        return true;
    }

    public bool FamilyDeal()
    {
        if (_isFamilyDealEffect)
        {
            return false;
        }
        _isFamilyDealEffect = true;
        return true;
    }

    public bool Collector()
    {
        if (_isCollectorEffect)
        {
            return false;
        }
        _isCollectorEffect = true;
        return false;
    }

    public bool Investment()
    {
        var temp = UnityEngine.Random.value;
        if (temp < 0.1f)
        {
            _isInvestmentEffect = true;
            return true;
        }
        return false;
    }

    public int GoldLoan(int price)
    {
        price = Mathf.RoundToInt(price * Random.Range(0.8f, 1.5f));
        Gold += price;
        return price;
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
