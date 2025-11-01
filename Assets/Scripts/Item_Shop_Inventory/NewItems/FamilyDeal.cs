using UnityEngine;

[CreateAssetMenu(menuName = "NewItem/FamilyDeal")]
public class FamilyDeal : ItemSOBase
{   
    /*
     * Family Deal – Blood Money（ファミリー契約：血の代金）
     * 犠牲者に前払いして沈黙と相続を確保します。生存者も自分の取り分を渡し、選ばれなかった犠牲者からはゴールドを得られます。
     * ただし、公の罪悪感は10%増加します。（恒久効果）
     */
    
    public override void UseItem(GameContext context)
    {
        //ItemEffect
    }
}