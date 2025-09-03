using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    [SerializeField, Header("Item Data")] public Sprite itemSprite;
    [SerializeField] public string itemName;
    [TextArea] public string itemDescription;
    [SerializeField] public int price;
    [SerializeField] public SelectItemEffect selectItemEffect = new SelectItemEffect();

    public void UseItem()
    {
        if (selectItemEffect == SelectItemEffect.OfficialIdentityHint)
        {
        }
    }

    public enum SelectItemEffect
    {
        // GOV.
        OfficialIdentityHint,
        OfficialNumberHint,
        // Prison
        Deal,
        InheritanceSystem,
        Guarantee,
        // Law
        DefenseAttorney,
        FinancialLawyer,
        FinancialExpert,
        // Church
        BlessingOfThePriest,
        ChurchsBlessing,
        PapalAmnesty,
        // Corp.
        FamilyDeal,
        Collector,
        Investment,
        GoldLoan,
    }
}
