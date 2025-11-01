using UnityEngine;

[CreateAssetMenu(menuName = "NewItem/Deal")]
public class Deal : ItemSOBase
{   
    /*
     * Deal – Overcrowded Profits（契約：過密の利益）
     * 刑務所は溢れかえっており、悲劇をチャンスに変える方法を知っています。
     * 我々の刑務所プログラムでは、殺された囚人犠牲者1人につき、
     * その相続に加えてボーナスゴールドが入り、殺された囚人の人数分乗算されます。（恒久効果）
     */
    
    public override void UseItem(GameContext context)
    {
        //ItemEffect
    }
}