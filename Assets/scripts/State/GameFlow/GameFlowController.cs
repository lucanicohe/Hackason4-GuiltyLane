using System;
using System.Collections;
using UnityEngine;

public class GameFlowController : Singleton<GameFlowController>
{
    [SerializeField] private GamePhaseSO startPhase;
    [SerializeField] private MainUIViewer mainUI;
    [SerializeField] private SpawnPeopleFactory spawnPeopleFactory;
    [SerializeField] private Shop Shop;
    [SerializeField] private TeamBuilder TeamBuilder;
    [SerializeField] private NewTrainController trainController;
    [SerializeField] private Transform spawnPointTeamA;
    [SerializeField] private Transform spawnPointTeamB;
    
    private GamePhaseSO currentPhase;
    private Coroutine _flowCoroutine;
    private GameContext gameContext;

    private void Start()
    {
        gameContext = InitislizeContext(new GameContext());
        _flowCoroutine = StartCoroutine(RunGameFlow());
    }

    private void OnEnable()
    {
        trainController.OnTrainEnterGameFlowTrigger += EnterGameFlowTrigger;
    }
    
    private void OnDisable()
    {
        trainController.OnTrainEnterGameFlowTrigger -= EnterGameFlowTrigger;
    }

    private IEnumerator RunGameFlow()
    {
        currentPhase = startPhase;

        while (currentPhase != null)
        {
            Debug.Log($"=== Start Phase: {currentPhase.phaseName} ===");
            
            yield return StartCoroutine(RunPhase(currentPhase));

            currentPhase = currentPhase.nextPhase;
        }
    }

    private IEnumerator RunPhase(GamePhaseSO phase)
    {
        phase.isExitTriggerEnter = false;
        foreach (var action in phase.actions)
        {
            yield return StartCoroutine(action.Execute(gameContext));
        }

        yield return new WaitUntil(() => phase.isExitTriggerEnter); //TODO ADD TRIGGER 
    }
    
    public void StopGameFlow(string text)
    {
        if (_flowCoroutine != null)
            StopCoroutine(_flowCoroutine);
        Debug.Log($"=== End Game {text} ===");
        //End Game 
    }

    private GameContext InitislizeContext(GameContext context)
    {
        mainUI.Bind(context);
        context.spawnPeopleFactory =  spawnPeopleFactory;
        context.Shop = Shop;
        Shop.BindShop(context);
        context.teamBuilder = TeamBuilder;
        context.mainUIViewer  = mainUI;
        context.NewTrainController = trainController;
        context.SpawnPointTeamA = spawnPointTeamA;
        context.SpawnPointTeamB = spawnPointTeamB;
        return context;
    }
    
    private void EnterGameFlowTrigger(string triggerTag)
    {
        if (currentPhase.phaseName.Equals(triggerTag))
        {
            currentPhase.isExitTriggerEnter = true;
        }
    }
}