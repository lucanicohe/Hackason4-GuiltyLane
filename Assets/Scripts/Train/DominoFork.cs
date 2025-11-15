using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoFork : MonoBehaviour
{
    [SerializeField] private GameObject _straightPrefab;
    [SerializeField] private Transform _spawnPointLeft;
    [SerializeField] private Transform _spawnPointRight;
    [SerializeField] private AudioSource _screamRight; 
    [SerializeField] private AudioSource _screamLeft;

    // ===== ADD WAYPOINT CHAINS =====
    [Header("Waypoints")]
    [SerializeField] private Waypoint _leftWaypointStart; // First of 3 left waypoints
    [SerializeField] private Waypoint _rightWaypointStart; // First of 3 right waypoints
    

    private bool _spawnedNext;

    private void OnTriggerEnter(Collider other)
    {
        if (_spawnedNext || !other.CompareTag("Player")) return;
        if(!other.TryGetComponent<TrainController>(out var train))return;
        
        if (train.lastDirection == Direction.Left)
        {   
            Instantiate(_straightPrefab,
                _spawnPointLeft.position,
                Quaternion.identity);
            _screamLeft.Play();
            // Set waypoint chain for left curve (will auto-switch through 3 points)
            if (_leftWaypointStart != null)
                train.SetRotationWaypoint(_leftWaypointStart);
        }
        else
        {   
            Instantiate(_straightPrefab,
                _spawnPointRight.position,
                Quaternion.identity);
            _screamRight.Play();
            // Set waypoint chain for right curve
            if (_rightWaypointStart != null)
                train.SetRotationWaypoint(_rightWaypointStart);
        }
        _spawnedNext = true;
    }
    private void OnTriggerExit(Collider other) => Destroy(gameObject);
}
