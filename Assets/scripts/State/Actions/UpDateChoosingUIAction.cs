using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class UpDateChoosingUIAction : PhaseActionSO
{   
    public override IEnumerator Execute(GameContext gameContext)
    {
        //update UI 
        // mainUIViewer.UpdateTrackText(gameContext.TeamA, gameContext.TeamB);
        yield break;
    }
}