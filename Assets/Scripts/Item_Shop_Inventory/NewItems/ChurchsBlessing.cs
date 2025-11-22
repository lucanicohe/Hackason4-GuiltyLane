using UnityEngine;

[CreateAssetMenu(menuName = "NewItem/ChurchsBlessing")]
public class ChurchsBlessing : ItemSOBase
{
    /*
     * Church’s Blessing – Public Forgiveness（教会の祝福：公の許し）
     * 選択によって公の罪悪感が50%以上急上昇する場合、教会が介入し罪悪感を0にリセットします。（1回のみ使用）
     */
    
    public override void UseItem(GameContext context)
    {
        if (context.GuiltyPercent.Value >= 50)
        {
            context.GuiltyPercent.Value = 0;
        }
    }
}