using System.Collections;
using UnityEngine;
using TMPro;

public class DominoStraight : MonoBehaviour
{
    [SerializeField] private GameObject _straightPrefab;
    [SerializeField] private GameObject _forkPrefab;
    [SerializeField] private Transform _spawnPointStraight;
    [SerializeField] private Transform _spawnPointFork;
    [SerializeField] private float _despawnDistance = 25f;
    [SerializeField] private Waypoint _straightWaypoint; 
    

    [Header("Team Spawn Points")]
    [SerializeField] private Transform _spawnPointRightTeam;
    [SerializeField] private Transform _spawnPointLeftTeam;
    
    private bool _spawnedNext;
    private static int _globalStraightCount;
    private Transform _player;
    private TeamBuilder _teamBuilder;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _teamBuilder = FindObjectOfType<TeamBuilder>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_spawnedNext || !other.CompareTag("Player")) return;

        // _trackText.color = Color.green;
        // _trackText.text = "Status: normal\nTrack: straight\nContinue driving";

        // ===== SET ROTATION WAYPOINT ONCE =====
        var train = other.GetComponent<TrainController>();
        if (train != null && _straightWaypoint != null)
        {
            train.SetRotationWaypoint(_straightWaypoint);
        }
        // ======================================

        _spawnedNext = true;

        if (_globalStraightCount < 2)
        {
            Instantiate(_straightPrefab, _spawnPointStraight.position, Quaternion.identity);
            _globalStraightCount++;
        }
        else
        {
            StartCoroutine(MainUIManager.Instance.AlertSequence());
        }
    }
    private void Update()
    {
        if (_player.position.z - transform.position.z > _despawnDistance)
            Destroy(gameObject);
    }
}