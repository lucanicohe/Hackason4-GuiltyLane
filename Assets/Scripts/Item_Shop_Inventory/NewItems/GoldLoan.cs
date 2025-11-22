using UnityEngine;

[CreateAssetMenu(menuName = "NewItem/GoldLoan")]
public class GoldLoan : ItemSOBase
{   
    /*
     * Gold Loan – Quick Cash（ゴールドローン：即金）
     * 怪しい利率で迅速なローンを受けます。購入価格の0.8〜1.5倍のゴールドを得られます。（1回のみ使用）
     */
    public override void UseItem(GameContext context)
    {
        var getMoney = (price * UnityEngine.Random.Range(80, 150)) /100;
        context.Gold.Value += getMoney;
    }
}