using UnityEngine;

[CreateAssetMenu(menuName = "NewItem/Collector")]
public class Collector : ItemSOBase
{   
    /*
     * Collector – Pressure Tactics（コレクター：圧力戦術）
     * コレクターが犠牲者から1コインずつ絞り取ります。犯罪率が高いほど、相続から得られる金額も増えます。（相続増加は犯罪率に依存。恒久効果）
     */
    
    public override void UseItem(GameContext context)
    {
        //ItemEffect
    }
}