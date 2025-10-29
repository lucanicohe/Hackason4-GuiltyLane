using System.Collections;
using UnityEngine;

public class OpenInventoryAction : PhaseActionSO
{   
    [SerializeField] private Shop _shop;
    public override IEnumerator Execute(GameContext gameContext)
    {
        _shop.SetShop(false);
        _shop.SetInventory(true);
        yield return null;
    }
}