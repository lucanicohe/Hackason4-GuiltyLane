using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/UpDateChoosingUIAction",fileName = "UpDateChoosingUIAction")]
public class UpDateChoosingUIAction : PhaseActionSO
{   
    public override IEnumerator Execute(GameContext gameContext)
    {
        //update UI 
        yield return gameContext.mainUIViewer.UpdateTrainText();
        gameContext.mainUIViewer.SetUpdateTrainText(gameContext.TeamA, gameContext.TeamB);
    }
}