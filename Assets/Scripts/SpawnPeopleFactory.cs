using System.Collections.Generic;
using UnityEngine;

public class SpawnPeopleFactory : MonoBehaviour
{   
    [Header("Team Prefabs")]
    [SerializeField] private GameObject _prisonerPrefab;
    [SerializeField] private GameObject _citizenPrefab;
    [SerializeField] private GameObject _royalPrefab;
    [SerializeField] private GameObject _noblePrefab;
    [SerializeField] private GameObject _ConfirmfalsePrefab;
    
    public List<GameObject> SpawnTeamNearPoint(HitPersonData team, Transform spawnPoint)
    {
        float offsetRadius = 5f;
        Vector3 basePos = spawnPoint.position;
        List<GameObject> spawnPeople = new(); 

        GameObject prefabToSpawn = team.RoleID switch
        {
            0 => _royalPrefab,
            1 => _noblePrefab,
            2 => _citizenPrefab,
            3 => _prisonerPrefab,
            _ => null
        };
        if (prefabToSpawn == null) return null;

        for (var i = 0; i < team.Personnum; i++)
        {   
            var go = Instantiate(prefabToSpawn);
            spawnPeople.Add(go);
        }
        return spawnPeople;

        // for (int i = 0; i < team.Personnum; i++)
        // {
        //     Vector3 randomOffset = new Vector3(
        //         Random.Range(-offsetRadius, offsetRadius),
        //         0f,
        //         Random.Range(-offsetRadius, offsetRadius)
        //     );
        //
        //     Instantiate(prefabToSpawn, basePos + randomOffset, Quaternion.identity);
        //     if (i == team.Personnum -1)
        //     {
        //         Instantiate(_ConfirmfalsePrefab, basePos, Quaternion.identity);
        //     }
        // }
    }
}