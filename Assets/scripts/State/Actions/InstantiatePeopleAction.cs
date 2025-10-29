using System.Collections;
using UnityEngine;

public class InstantiatePeopleAction : PhaseActionSO
{   
    [SerializeField] private Vector3 rightPosition;
    [SerializeField] private Vector3 leftPosition;
    [SerializeField] private SpawnPeopleFactory  spawnPeopleFactory;
    public override IEnumerator Execute(GameContext gameContext)
    {
        InstantiatePoepleInPostion(gameContext.TeamA.Value, rightPosition);
        InstantiatePoepleInPostion(gameContext.TeamB.Value, leftPosition);
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