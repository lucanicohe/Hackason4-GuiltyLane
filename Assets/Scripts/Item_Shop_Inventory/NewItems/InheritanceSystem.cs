using UnityEngine;

[CreateAssetMenu(menuName = "NewItem/InheritanceSystem")]
public class InheritanceSystem : ItemSOBase
{   
    /*
     *Inheritance System – Through the Bars（相続制度：鉄格子越しに）
     * 内部のコネクションにより、囚人犠牲者の相続は直接あなたに渡ります。
     * ただし、教会はこの行為を罪深いと非難しており、罪悪感が1.5倍に増加します。（恒久効果）
     */
    public override void UseItem(GameContext context)
    {
        context.IsInheritanceSystemEffect = true;
    }
}