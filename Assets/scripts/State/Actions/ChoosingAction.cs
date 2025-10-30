using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/ChoosingAction",fileName = "ChoosingAction")]
public class ChoosingAction : PhaseActionSO
{   
    private TrainController _trainController;
    public override IEnumerator Execute(GameContext gameContext)
    {   
        _trainController = gameContext.trainController;
        yield return new WaitForSeconds(10f);
        gameContext.LastTrainDirection = _trainController.lastDirection;
    }
}