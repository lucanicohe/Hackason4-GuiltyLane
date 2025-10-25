using System;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : Singleton<StateManager>
{ 
    //States
    public StateMachine  stateMachine = new StateMachine();
    public MovingState MovingState;
    public ChoosingState ChoosingState;
    public HitState HitState;
    public ShoppingState ShoppingState;
    public InventoryState InventoryState;
    
    //Factory
    [SerializeField] private SpawnPeopleFactory _spawnPeopleFactory;
    [SerializeField] private TeamBuilder _teamBuilder;
    [SerializeField] private Shop _shop;
    private Transform _spawnRightPoint;
    private Transform _spawnLeftPoint;
    
    //UI
    [SerializeField]private MainUIManager _mainUIManager;
    
    private List<GameObject> _spawnPeopleList;
    private void Awake()
    {
        MovingState = new MovingState();
        ChoosingState = new ChoosingState(_teamBuilder);
        HitState = new HitState(_spawnPeopleList,_mainUIManager);
        ShoppingState = new ShoppingState(_shop);
        InventoryState = new InventoryState(_shop);
        
        stateMachine.Initialize(MovingState);
    }

    private void OnEnable()
    {
        ChoosingState.OnTeamBuildFinished += OnTeamBuildFinished;
    }

    private void OnDisable()
    {
        ChoosingState.OnTeamBuildFinished -= OnTeamBuildFinished;
    }

    public void Update()
    {
        stateMachine.Execute();
    }

    public void SwitchStateToChoose()
    {
        stateMachine.TransitionTo(ChoosingState);
    }

    public void SwitchStateToHit()
    {   
        stateMachine.TransitionTo(HitState);
    }

    private void OnTeamBuildFinished(HitPersonData leftData,HitPersonData rightData)
    {
        var RList = _spawnPeopleFactory.SpawnTeamNearPoint(rightData,_spawnRightPoint);
        var LList = _spawnPeopleFactory.SpawnTeamNearPoint(leftData,_spawnLeftPoint);
        _spawnPeopleList.AddRange(RList);
        _spawnPeopleList.AddRange(LList);
        //Update UI 
        _mainUIManager.UpdateTrackText(leftData, rightData);
    }
}