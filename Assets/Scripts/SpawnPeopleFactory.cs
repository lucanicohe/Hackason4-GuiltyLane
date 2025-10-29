using System.Collections.Generic;
using UnityEngine;

public class SpawnPeopleFactory : MonoBehaviour
{   
    public List<GameObject> SpawnPeoples(HitPersonData data)
    {
        List<GameObject> spawnPeople = new(); 
        var prefabToSpawn = data.Prefab;
        for (var i = 0; i < data.Personnum; i++)
        {   
            var go = Instantiate(prefabToSpawn);
            spawnPeople.Add(go);
        }
        return spawnPeople;
    }
}