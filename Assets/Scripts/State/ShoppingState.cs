using System.Collections;
using TMPro;
using UnityEngine;

public class ShoppingState : IState
{   
    private Shop _shop;

    public ShoppingState(Shop shop)
    {
        _shop = shop;
    }
    public void Enter()
    {
        //open shop
        _shop.SetShop(true);
        _shop.SetInventory(false);
    }

    public void Exit()
    {
        //close shop 
        _shop.SetShop(false);
    }

    public void Execute()
    {
        //Count Down Shop time 
    }
}