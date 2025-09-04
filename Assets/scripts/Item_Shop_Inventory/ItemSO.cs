using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemSO")]
public class ItemSO : ScriptableObject
{
    [SerializeField, Header("Item Data")] public Sprite itemSprite;
    [SerializeField] public string itemName;
    [TextArea] public string itemDescription;
    [SerializeField] public int price;
    [SerializeField] public SelectItemEffect selectItemEffect = new SelectItemEffect();
    [SerializeField] public bool permanent;

    public void UseItem()
    {
        if (selectItemEffect == SelectItemEffect.OfficialIdentityHint)
        {
            Debug.Log($"Player used {selectItemEffect}");
            /*
                選択肢の中から1人の犠牲者を明らかにする。

                （ 「1回のみ使用可能、※使用後は再購入が必要です。」と書いてあったが、
                面倒であれば無しでよいし、この内容が本当に必要か要相談。
            */
        }
        else if (selectItemEffect == SelectItemEffect.OfficialNumberHint)
        {
            Debug.Log($"Player used {selectItemEffect}");
            /*
                選択肢の中から1人の犠牲者の番号(人数？)を明らかにする
                （1回のみ使用可能 <-- 同上）
            */
        }
        else if (selectItemEffect == SelectItemEffect.Deal)
        {
            Debug.Log($"Player used {selectItemEffect}");
            /*
                囚人(身分)は囚人(身分)の人数分だけ遺産が増える(永続)
            */
        }
        else if (selectItemEffect == SelectItemEffect.InheritanceSystem)
        {
            Debug.Log($"Player used {selectItemEffect}");
            /*
                囚人(身分)のシンボル：囚人(身分)の遺産と罪率が1.5倍増加する(永続)
            */
        }
        else if (selectItemEffect == SelectItemEffect.Guarantee)
        {
            Debug.Log($"Player used {selectItemEffect}");
            /*
                次のラウンドで必ず囚人(身分)がでる。
            */
        }
        else if (selectItemEffect == SelectItemEffect.DefenseAttorney)
        {
            Debug.Log($"Player used {selectItemEffect}");
            /*
                プロの弁護を確保して取り分を最大化します。次の裁判で得られる相続は20%増加します。

                次のラウンドで得られる遺産が20%増える
            */
        }
        else if (selectItemEffect == SelectItemEffect.FinancialLawyer)
        {
            Debug.Log($"Player used {selectItemEffect}");
            /*
                エリート法律チームが資産を厳重に書類で保護します。
                相続が増えた場合、その後条件が変わっても減ることはありません。（恒久効果）

                （手錠：全ての身分の遺産が(一時的に増えた場合でも)減らなくなる(永続)）
            */
        }
        else if (selectItemEffect == SelectItemEffect.FinancialExpert)
        {
            Debug.Log($"Player used {selectItemEffect}");
            /*
                虫眼鏡：次のラウンドで得られる遺産が20%増える
            */
        }
        else if (selectItemEffect == SelectItemEffect.BlessingOfThePriest)
        {
            Debug.Log($"Player used {selectItemEffect}");
            /*
                次のラウンドでは罪率が半減される
            */
        }
        else if (selectItemEffect == SelectItemEffect.ChurchsBlessing)
        {
            Debug.Log($"Player used {selectItemEffect}");
            /*
                選択によって公の罪悪感が50%以上急上昇する場合、教会が介入し罪悪感を0にリセットします。（1回のみ使用）
            */
        }
        else if (selectItemEffect == SelectItemEffect.PapalAmnesty)
        {
            Debug.Log($"Player used {selectItemEffect}");
            /*
                教皇の恩赦により、現在の罪悪感が10%減少します。
            */
        }
        else if (selectItemEffect == SelectItemEffect.FamilyDeal)
        {
            Debug.Log($"Player used {selectItemEffect}");
            /*
                犠牲者に前払いして沈黙と相続を確保します。生存者も自分の取り分を渡し、
                選ばれなかった犠牲者からはゴールドを得られます。ただし、公の罪悪感は10%増加します。（恒久効果）
            */
        }
        else if (selectItemEffect == SelectItemEffect.Collector)
        {
            Debug.Log($"Player used {selectItemEffect}");
            /*
                コレクターが犠牲者から1コインずつ絞り取ります。
                犯罪率が高いほど、相続から得られる金額も増えます。（相続増加は犯罪率に依存。恒久効果）
            */
        }
        else if (selectItemEffect == SelectItemEffect.Investment)
        {
            Debug.Log($"Player used {selectItemEffect}");
            /*
                危険な事業に参加します：相続が5倍になる確率は10%です。（1回のみ使用）
            */
        }
        else if (selectItemEffect == SelectItemEffect.GoldLoan)
        {
            Debug.Log($"Player used {selectItemEffect}");
            /*
                怪しい利率で迅速なローンを受けます。購入価格の0.8〜1.5倍のゴールドを得られます。（1回のみ使用）
            */
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
