using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoFork : MonoBehaviour
{
    [SerializeField] private GameObject _straightPrefab;
    [SerializeField] private Transform _spawnPointLeft;
    [SerializeField] private Transform _spawnPointRight;

    private bool _spawnedNext;

    private void OnTriggerEnter(Collider other)
    {
        if (_spawnedNext || !other.CompareTag("Player")) return;

        TrainController train = other.GetComponent<TrainController>();
        if (train == null) return;

        Transform spawnPoint = train._lastDirection == TrainController.Direction.Left 
                               ? _spawnPointLeft 
                               : _spawnPointRight;

        Instantiate(_straightPrefab, spawnPoint.position, Quaternion.identity);

        if (train._lastDirection == TrainController.Direction.Left)
            other.transform.Rotate(0f, -15f, 0f);

        _spawnedNext = true;
    }
}