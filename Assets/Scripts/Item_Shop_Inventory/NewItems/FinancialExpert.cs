using UnityEngine;

[CreateAssetMenu(menuName = "NewItem/FinancialExpert")]
public class FinancialExpert : ItemSOBase
{   
    /*
     * Financial Expert – Cost Reduction（財務専門家：費用削減）
     * 専門家が抜け穴や費用を削減します。次の裁判後の手数料が20%減少します。
     */
    
    public override void UseItem(GameContext context)
    {
        context.IsFinancialExpert =  true;
    }
}