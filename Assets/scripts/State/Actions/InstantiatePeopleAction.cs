using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/InstantiatePeopleAction",fileName = "InstantiatePeopleAction")]
public class InstantiatePeopleAction : PhaseActionSO
{   
    private SpawnPeopleFactory spawnPeopleFactory;
    public override IEnumerator Execute(GameContext gameContext)
    {   
        spawnPeopleFactory = gameContext.spawnPeopleFactory;
        gameContext.TeamAPeoplePrefabs = InstantiatePeopleInPosition(gameContext.TeamA, gameContext.SpawnPointTeamA);
        gameContext.TeamBPeoplePrefabs = InstantiatePeopleInPosition(gameContext.TeamB, gameContext.SpawnPointTeamB);
        yield break;
    }

    private List<GameObject> InstantiatePeopleInPosition(HitPersonData data, Transform transform)
    {
        var peoples = spawnPeopleFactory.SpawnPeoples(data);
        foreach (var p in peoples)
        {
            var pos = Random.insideUnitSphere * 6;
            pos.y = 0;
            var vector3pos = transform.position + pos;
            var rotation = transform.rotation;
            p.transform.SetPositionAndRotation(vector3pos, rotation);
        }
        return peoples;
    }
}