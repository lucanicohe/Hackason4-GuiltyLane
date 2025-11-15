using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/OpenShopAction",fileName = "OpenShopAction")]
public class OpenShopAction : PhaseActionSO
{   
    public override IEnumerator Execute(GameContext gameContext)
    {
        //open
        gameContext.Shop.SetShop(true);
        gameContext.Shop.SetInventory(false);
        yield return null;
    }
}