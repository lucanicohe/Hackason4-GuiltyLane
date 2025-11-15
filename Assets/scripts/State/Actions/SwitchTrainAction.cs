using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/SwitchTrainAction",fileName = "SwitchTrainAction")]
public class SwitchTrainAction : PhaseActionSO
{   
    public override IEnumerator Execute(GameContext gameContext)
    {
        //move train to left or right s
        
        yield return null;
    }

    private void SwitchTrain()
    {
       
    }
}