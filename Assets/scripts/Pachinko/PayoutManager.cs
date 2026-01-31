using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using System;

public class PayoutManager : MonoBehaviour
{
    [Header("Display Texts")]
    [SerializeField] private TextMeshProUGUI _ballAmountText;
    [SerializeField] private TextMeshProUGUI _guiltText;
    [SerializeField] private TextMeshProUGUI _moneyText;

    [Header("Money Exchange Buttons")]
    [SerializeField] private Button _smallMoneyButton; // 10 balls -> +1 money
    [SerializeField] private Button _largeMoneyButton; // 100 balls -> +10 money

    [Header("Guilt Reduction Buttons")]
    [SerializeField] private Button _smallGuiltButton; // 10 balls -> -1% guilt
    [SerializeField] private Button _largeGuiltButton; // 100 balls -> -10% guilt

    void Start()
    {
        if (_smallMoneyButton != null)
        {
            _smallMoneyButton.OnClickAsObservable()
                .Where(_ => GameManager.Instance != null && GameManager.Instance.Balls >= 10)
                .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
                .Subscribe(_ => ExchangeForMoney(10, 1))
                .AddTo(this);
        }

        if (_largeMoneyButton != null)
        {
            _largeMoneyButton.OnClickAsObservable()
                .Where(_ => GameManager.Instance != null && GameManager.Instance.Balls >= 100)
                .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
                .Subscribe(_ => ExchangeForMoney(100, 10))
                .AddTo(this);
        }

        if (_smallGuiltButton != null)
        {
            _smallGuiltButton.OnClickAsObservable()
                .Where(_ => GameManager.Instance != null && GameManager.Instance.Balls >= 10 && GameManager.Instance.GuiltyPercent > 0)
                .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
                .Subscribe(_ => ExchangeForGuiltReduction(10, 1))
                .AddTo(this);
        }

        if (_largeGuiltButton != null)
        {
            _largeGuiltButton.OnClickAsObservable()
                .Where(_ => GameManager.Instance != null && GameManager.Instance.Balls >= 100 && GameManager.Instance.GuiltyPercent > 0)
                .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
                .Subscribe(_ => ExchangeForGuiltReduction(100, 10))
                .AddTo(this);
        }

        UpdateDisplay();
    }

    private void Update()
    {
        UpdateDisplay();
        UpdateButtonStates();
    }

    private void UpdateDisplay()
    {
        if (GameManager.Instance == null) return;

        if (_ballAmountText != null)
            _ballAmountText.text = $"たま : {GameManager.Instance.Balls}";

        if (_guiltText != null)
            _guiltText.text = $"つみ : {GameManager.Instance.GuiltyPercent}%";

        if (_moneyText != null)
            _moneyText.text = $"おかね : {GameManager.Instance.Gold}";
    }

    private void UpdateButtonStates()
    {
        if (GameManager.Instance == null) return;

        int currentBalls = GameManager.Instance.Balls;
        int currentGuilt = GameManager.Instance.GuiltyPercent;

        if (_smallMoneyButton != null)
            _smallMoneyButton.interactable = currentBalls >= 10;

        if (_largeMoneyButton != null)
            _largeMoneyButton.interactable = currentBalls >= 100;

        if (_smallGuiltButton != null)
            _smallGuiltButton.interactable = currentBalls >= 10 && currentGuilt > 0;

        if (_largeGuiltButton != null)
            _largeGuiltButton.interactable = currentBalls >= 100 && currentGuilt > 0;
    }

    private void ExchangeForMoney(int ballCost, int moneyGain)
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.Balls < ballCost)
        {
            Debug.LogWarning($"Not enough balls! Need {ballCost}, have {GameManager.Instance.Balls}");
            return;
        }

        int ballsBefore = GameManager.Instance.Balls;
        int goldBefore = GameManager.Instance.Gold;

        GameManager.Instance.Balls -= ballCost;
        GameManager.Instance.Gold += moneyGain;

        Debug.Log($"Exchanged {ballCost} balls for {moneyGain} money. Balls: {ballsBefore}->{GameManager.Instance.Balls}, Gold: {goldBefore}->{GameManager.Instance.Gold}");
    }

    private void ExchangeForGuiltReduction(int ballCost, int guiltReduction)
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.Balls < ballCost)
        {
            Debug.LogWarning($"Not enough balls! Need {ballCost}, have {GameManager.Instance.Balls}");
            return;
        }

        if (GameManager.Instance.GuiltyPercent <= 0)
        {
            Debug.LogWarning("Guilt is already at 0");
            return;
        }

        int ballsBefore = GameManager.Instance.Balls;
        int guiltBefore = GameManager.Instance.GuiltyPercent;

        GameManager.Instance.Balls -= ballCost;
        GameManager.Instance.GuiltyPercent = Mathf.Max(0, GameManager.Instance.GuiltyPercent - guiltReduction);

        Debug.Log($"Exchanged {ballCost} balls for {guiltReduction}% guilt reduction. Balls: {ballsBefore}->{GameManager.Instance.Balls}, Guilt: {guiltBefore}->{GameManager.Instance.GuiltyPercent}");
    }
}