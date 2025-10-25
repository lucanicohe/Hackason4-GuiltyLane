public class InventoryState : IState
{   
    private Shop _shop;
    public InventoryState(Shop shop)
    {
        _shop = shop;
    }
    public void Enter()
    {   
        //open Inventory
        _shop.SetInventory(true);
    }

    public void Exit()
    {
        //close Inventory
        _shop.SetInventory(false);
    }

    public void Execute()
    {
        //Inventory behaver
    }
}