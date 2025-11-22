using UnityEngine;

[CreateAssetMenu(menuName = "NewItem/Guarantee")]
public class Guarantee : ItemSOBase
{   
    /*
     * Guarantee – Locked-In Supply（保証：確保された供給）
     * 投資が枯渇することはありません。
     * 次の裁判では、必ず囚人犠牲者が登場します。
     */
    public override void UseItem(GameContext context)
    {
        context.IsGuaranteeEffect =  true;
    }
}