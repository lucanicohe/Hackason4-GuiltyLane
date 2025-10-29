using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/InstantiatePeopleAction",fileName = "InstantiatePeopleAction")]
public class InstantiatePeopleAction : PhaseActionSO
{   
    private SpawnPeopleFactory spawnPeopleFactory;
    public override IEnumerator Execute(GameContext gameContext)
    {   
        spawnPeopleFactory = gameContext.spawnPeopleFactory;
        InstantiatePoepleInPostion(gameContext.TeamA, Vector3.zero);
        InstantiatePoepleInPostion(gameContext.TeamB, Vector3.zero);
        yield return null;
    }

    private void InstantiatePoepleInPostion(HitPersonData data, Vector3 position)
    {
        var peoples = spawnPeopleFactory.SpawnPeoples(data);
        foreach (var p in peoples)
        {
            var pos = Random.insideUnitSphere;
            pos.y = 0;
            p.transform.position = position;
        }
    }
}