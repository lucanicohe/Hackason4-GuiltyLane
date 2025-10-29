using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class UpDateChoosingUIAction : PhaseActionSO
{   
    [FormerlySerializedAs("mainUIManager")] [SerializeField] private MainUIViewer mainUIViewer;
    public override IEnumerator Execute(GameContext gameContext)
    {
        //update UI 
        mainUIViewer.UpdateTrackText(gameContext.TeamA, gameContext.TeamB);
        yield return null;
    }
}