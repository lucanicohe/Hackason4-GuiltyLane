using System;
using UnityEngine;

public class StateManager : MonoBehaviour
{ 
    public StateMachine  stateMachine = new StateMachine();
    public MovingState MovingState = new MovingState();
    public ChoosingState ChoosingState = new ChoosingState();
    public HitState HitState = new HitState();
    public ShoppingState ShoppingState = new ShoppingState();
    public InventoryState InventoryState = new InventoryState();
    private void Awake()
    {
        stateMachine.Initialize(MovingState);
    }

    public void Update()
    {
        stateMachine.Execute();
    }
}