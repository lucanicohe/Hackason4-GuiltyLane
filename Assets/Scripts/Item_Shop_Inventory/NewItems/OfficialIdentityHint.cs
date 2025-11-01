using UnityEngine;

[CreateAssetMenu(menuName = "NewItem/OfficialIdentityHint")]
public class OfficialIdentityHint : ItemSOBase
{   
    /*
     * Official Identity Hint（公式身元ヒント）
     * レムリア政府は教会の許可により、公式ルートを通じて洞察を得ることができます：
     * 選択肢の中から1人の犠牲者を明らかにする（1回のみ使用可能）。※使用後は再購入が必要です。
     */
    public override void UseItem(GameContext context)
    {
        //ItemEffect
    }
}