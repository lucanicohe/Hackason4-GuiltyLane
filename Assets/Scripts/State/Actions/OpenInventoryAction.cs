using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/OpenInventoryAction",fileName = "OpenInventoryAction")]
public class OpenInventoryAction : PhaseActionSO
{   
    public override IEnumerator Execute(GameContext gameContext)
    {
        gameContext.Shop.SetShop(false);
        gameContext.Shop.SetInventory(true);
        yield return null;
    }
}