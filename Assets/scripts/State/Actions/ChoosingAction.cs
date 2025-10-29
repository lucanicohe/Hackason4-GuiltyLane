using System.Collections;
using UnityEngine;

public class ChoosingAction : PhaseActionSO
{   
    [SerializeField]private TrainController trainController;
    private float _timer = 30;
    public override IEnumerator Execute(GameContext gameContext)
    {
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            gameContext.LastTrainDirection = trainController.lastDirection;
            yield return null;
        }
    }
}