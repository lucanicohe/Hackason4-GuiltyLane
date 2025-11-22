using UnityEngine;

[CreateAssetMenu(menuName = "NewItem/BlessingOfThePriest")]
public class BlessingOfThePriest : ItemSOBase
{   
        /*
         * Blessing of the Priest – Divine Grace（司祭の祝福：神の恩寵）
         * 聖職者の慈悲を受けます。次の裁判で罪悪感が半減します。
         */
    public override void UseItem(GameContext context)
    {
        context.IsBlessingOfThePriestEffect =  true;
    }
}