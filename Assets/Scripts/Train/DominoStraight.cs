using UnityEngine;
using UnityEngine.Events;

public class DominoStraight : MonoBehaviour
{
    [SerializeField] private GameObject _straightPrefab;
    [SerializeField] private GameObject _forkPrefab;
    [SerializeField] private Transform _spawnPointStraight;
    [SerializeField] private Transform _spawnPointFork;
    // [SerializeField] private float _despawnDistance = 25f;
    [SerializeField] private Waypoint _straightWaypoint; 
    

    [Header("Team Spawn Points")]
    [SerializeField] private Transform _spawnPointRightTeam;
    [SerializeField] private Transform _spawnPointLeftTeam;
    
    private bool _spawnedNext;
    private static int _globalStraightCount;

    private void OnTriggerEnter(Collider other)
    {
        if (_spawnedNext || !other.CompareTag("Player")) return;

        // ===== SET ROTATION WAYPOINT ONCE =====
        var train = other.GetComponent<TrainController>();
        if (train != null && _straightWaypoint != null)
        {
            train.SetRotationWaypoint(_straightWaypoint);
        }
        _spawnedNext = true;

        if (_globalStraightCount < 2)
        {
            Instantiate(_straightPrefab, _spawnPointStraight.position, Quaternion.identity);
            _globalStraightCount++;
        }
    }
}