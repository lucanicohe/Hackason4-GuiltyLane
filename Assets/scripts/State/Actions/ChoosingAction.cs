using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/ChoosingAction",fileName = "ChoosingAction")]
public class ChoosingAction : PhaseActionSO
{   
    public override IEnumerator Execute(GameContext gameContext)
    {
        yield break;
    }
}