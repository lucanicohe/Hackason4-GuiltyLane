using System.Collections;
using UnityEngine;

public class GameFlowController : Singleton<GameFlowController>
{
    [SerializeField] private GamePhaseSO startPhase;
    [SerializeField] private MainUIViewer mainUI;
    
    private GamePhaseSO currentPhase;
    private Coroutine _flowCoroutine;
    private GameContext gameContext = new GameContext();

    private void Start()
    {
        _flowCoroutine = StartCoroutine(RunGameFlow());
        mainUI.Bind(gameContext);
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
    
    public void StopGameFlow()
    {
        if (_flowCoroutine != null)
            StopCoroutine(_flowCoroutine);

        //End Game 
    }
}