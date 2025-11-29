using UnityEngine;

[CreateAssetMenu(menuName = "NewItem/Investment")]
public class Investment : ItemSOBase
{   
    /*
     * Investment – High-Risk Gamble（投資：ハイリスク賭け）
     * 危険な事業に参加します：相続が5倍になる確率は10%です。（1回のみ使用）
     */
    public override void UseItem(GameContext context)
    {
        var v = UnityEngine.Random.Range(0, 100);
        if (v <= 10)
        {
            context.IsInvestmentEffect = true;
            Debug.Log("Investment success");
        }
        else
        {
            Debug.Log("Investment failed");
        }
    }
}