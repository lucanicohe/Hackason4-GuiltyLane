using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public const int BE_DISGUISED_INT = -1;
    public const string BE_DISGUISED_STRING = "■■■■";

    [SerializeField] private TextMeshProUGUI _goldGuiltText;
    [SerializeField] private TextMeshProUGUI _tollForkText;
    [SerializeField] private ShopManager _shopManager;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private GameObject _shopAlertImage;
    [SerializeField] private float GetGoldGuilty = 6f;

    [Header("Shop Countdown")]
    [SerializeField] private TextMeshProUGUI _shopCountdownText;
    [SerializeField] private float _shopOpenTime = 30f; 

    [SerializeField] private TeamBuilder teamBuilder;
    [SerializeField] private TrainController trainController;
    [SerializeField] private Fade fade;

    public static bool ChooseDisguised;
    public bool _speedzero = false;

    public int Gold = 0;
    public int GuiltyPercent = 0;
    public int Salary = 12;
    public int Toll = 15;
    private int[] TollArray = new int[] { 0, 15, 30, 60, 100, 150, 200 };

    private int _TotalGuiltyPercent;
    private int _TotalInheritance;

    public int _problemCount = 1;
    private bool DecidedMainValue = false;
    public bool _confirm = false;

    public enum RoundLevel { None, First, Second, Third, Fourth, Fifth, Sixth, ToEnd }
    public RoundLevel _roundLevel = RoundLevel.First;

    public TeamBuilder.HitPersonData _RightHPD;
    public TeamBuilder.HitPersonData _LeftHPD;

    public TeamBuilder.HitPersonData RightHPD => (_roundLevel == RoundLevel.Third || _roundLevel == RoundLevel.Fifth || _roundLevel == RoundLevel.Sixth || ChooseDisguised)
        ? DisguisedPersonData(_RightHPD)
        : _RightHPD;

    public TeamBuilder.HitPersonData LeftHPD => (_roundLevel == RoundLevel.Third || _roundLevel == RoundLevel.Fifth || _roundLevel == RoundLevel.Sixth || !ChooseDisguised)
        ? DisguisedPersonData(_LeftHPD)
        : _LeftHPD;

    void Start()
    {
        _goldGuiltText.text = $"Gold : {Gold}\nGuilt : {GuiltyPercent}%";
        _tollForkText.text = $"Toll\n{Toll}\n\nFork\n{(int)_roundLevel}-{_problemCount}";

        if (_inventoryManager != null) _inventoryManager.InventoryActive = true;
        if (_shopManager != null) _shopManager.shop.SetActive(false);
        if (_shopAlertImage != null) _shopAlertImage.SetActive(false);
        if (_shopCountdownText != null) _shopCountdownText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_roundLevel == RoundLevel.ToEnd)
        {
            HandleGameOver(true);
            return;
        }

        MainValue();

        if (_confirm)
        {
            _confirm = false;
            StartCoroutine(StartForkSequence());
        }
    }

    private void MainValue()
    {
        if (DecidedMainValue) return;

        _RightHPD = teamBuilder.TeamBuild();
        _LeftHPD = teamBuilder.TeamBuild();

        if (_RightHPD.SelectRole != _LeftHPD.SelectRole)
        {
            DecidedMainValue = true;
            ChooseDisguised = Random.value < 0.5f;
            _RightHPD = InitializePersonData(_RightHPD);
            _LeftHPD = InitializePersonData(_LeftHPD);
        }
    }

    private TeamBuilder.HitPersonData InitializePersonData(TeamBuilder.HitPersonData data)
    {
        data.PersonnumToShow = data.Personnum;
        data.TotalInheritanceToShow = data.TotalInheritance;
        data.TotalGuiltyPercentToShow = data.TotalGuiltyPercent;
        return data;
    }

    private TeamBuilder.HitPersonData DisguisedPersonData(TeamBuilder.HitPersonData data)
    {
        data = InitializePersonData(data);
        switch (_roundLevel)
        {
            case RoundLevel.First: return data;
            case RoundLevel.Second:
            case RoundLevel.Third:
                data.SelectRole = BE_DISGUISED_STRING;
                data.TotalInheritanceToShow = BE_DISGUISED_INT;
                return data;
            case RoundLevel.Fourth:
            case RoundLevel.Fifth:
            case RoundLevel.Sixth:
                data.SelectRole = BE_DISGUISED_STRING;
                data.PersonnumToShow = BE_DISGUISED_INT;
                data.TotalInheritanceToShow = BE_DISGUISED_INT;
                data.TotalGuiltyPercentToShow = BE_DISGUISED_INT;
                return data;
            default: return data;
        }
    }

    private IEnumerator StartForkSequence()
    {
        yield return new WaitForSeconds(GetGoldGuilty);
        MainGame();
    }

    private void MainGame()
    {
        // Assign totals based on train direction
        if (trainController._lastDirection == TrainController.Direction.Right)
        {
            _TotalGuiltyPercent = _RightHPD.TotalGuiltyPercent;
            _TotalInheritance = _RightHPD.TotalInheritance;
        }
        else
        {
            _TotalGuiltyPercent = _LeftHPD.TotalGuiltyPercent;
            _TotalInheritance = _LeftHPD.TotalInheritance;
        }

        // Add totals
        Gold += _TotalInheritance;
        GuiltyPercent += _TotalGuiltyPercent;
        ResultData.TotalGuiltyPercentEarned += _TotalGuiltyPercent;
        ResultData.TotleGoldEarned += _TotalInheritance;

        _problemCount++;

        // Update UI
        _goldGuiltText.text = $"Gold : {Gold}\nGuilt : {GuiltyPercent}%";
        _tollForkText.text = $"Toll\n{Toll}\n\nFork\n{(int)_roundLevel}-{_problemCount}";

        if (GuiltyPercent >= 100)
        {
            HandleGameOver(false);
            return;
        }

        // Toll & salary logic
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

        // Immediate round change
        ChangeRound();

        // Trigger shop sequence (with countdown)
        StartCoroutine(ShopSequenceCoroutine());

        DecidedMainValue = false;
    }

    private void ChangeRound()
    {
        _roundLevel++;
        if (_roundLevel > RoundLevel.Sixth) _roundLevel = RoundLevel.ToEnd;

        Toll = TollArray[(int)_roundLevel];
        Debug.Log($"Round changed to: {_roundLevel}");
    }

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
    }

    private void HandleGameOver(bool isGameClear)
    {
        _speedzero = true;
        AudioManager.Instance.StopAllBGM();
        AudioManager.Instance.StopAllSE();
        ResultData.RoundLevel = _roundLevel;
        fade.FadeIn(2, () => SceneManager.LoadScene("ResultScene"));

        Debug.Log(isGameClear ? "ゲームクリア" : "ゲームオーバー");
    }
}

public static class ResultData
{
    public static int TotleRoyalKilledNum = 0;
    public static int TotleNobleKilledNum = 0;
    public static int TotleCitizenKilledNum = 0;
    public static int TotlePrisonerKilledNum = 0;
    public static int TotleGoldEarned = 0;
    public static int TotalGuiltyPercentEarned = 0;
    public static GameManager.RoundLevel RoundLevel = GameManager.RoundLevel.First;
    public static int problemCount = 0;
    public static int TotalTollPayed = 0;
}