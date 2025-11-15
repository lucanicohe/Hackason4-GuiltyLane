using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/DestroyPeopleAction",fileName = "DestroyPeopleAction")]
public class DestroyPeopleAction : PhaseActionSO
{   
    
    public override IEnumerator Execute(GameContext gameContext)
    {
        DestoryPeople(gameContext.TeamAPeoplePrefabs);
        DestoryPeople(gameContext.TeamBPeoplePrefabs);
        yield break;
    }

    private void DestoryPeople(List<GameObject> peoples)
    {
        foreach (var p in peoples)
        {
            Destroy(p);
        }
        peoples.Clear();
    }
    
}