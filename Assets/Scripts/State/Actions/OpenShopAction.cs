using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/OpenShopAction",fileName = "OpenShopAction")]
public class OpenShopAction : PhaseActionSO
{   
    public override IEnumerator Execute(GameContext gameContext)
    {
        yield return null;
    }
}