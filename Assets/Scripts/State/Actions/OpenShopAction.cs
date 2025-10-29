using System.Collections;
using UnityEngine;

public class OpenShopAction : PhaseActionSO
{   
    [SerializeField] private Shop _shop;
    public override IEnumerator Execute(GameContext gameContext)
    {
        //open
        _shop.SetShop(true);
        _shop.SetInventory(false);
        yield return null;
    }
}