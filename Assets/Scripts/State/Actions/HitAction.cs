using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/HitAction",fileName = "HitAction")]
public class HitAction : PhaseActionSO
{   
    public override IEnumerator Execute(GameContext gameContext)
    {   
        //Handle People hit
        yield break;
    }
}