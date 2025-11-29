using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/CloseInventory",fileName = "CloseInventory")]
public class CloseInventory : PhaseActionSO{
    
    private MainUIViewer _uiViewer;
    public override IEnumerator Execute(GameContext gameContext)
    {
        _uiViewer = gameContext.mainUIViewer;
        _uiViewer.UpdateTrainNormalText();
        yield break;
    }
}