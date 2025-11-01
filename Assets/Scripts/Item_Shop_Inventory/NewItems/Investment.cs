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
        //ItemEffect
    }
}