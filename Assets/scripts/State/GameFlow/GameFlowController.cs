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
    
    private GamePhaseSO currentPhase;
    private Coroutine _flowCoroutine;
    private GameContext gameContext;

    private void Start()
    {
        gameContext = InitislizeContext(new GameContext());
        _flowCoroutine = StartCoroutine(RunGameFlow());
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
        foreach (var action in phase.actions)
        {
            yield return StartCoroutine(action.Execute(gameContext));
        }
        yield return new WaitForSeconds(phase.duration);
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
        return context;
    }
}