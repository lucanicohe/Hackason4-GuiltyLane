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

    private bool _spawnedNext;

    private void OnTriggerEnter(Collider other)
    {
        if (_spawnedNext || !other.CompareTag("Player")) return;

        var train = other.GetComponent<TrainController>();
        if (train == null) return;

        bool goLeft = train._lastDirection == TrainController.Direction.Left;

        Instantiate(_straightPrefab, 
            goLeft ? _spawnPointLeft.position : _spawnPointRight.position, 
            Quaternion.identity);

        if (goLeft)
        {
            _screamLeft.Play();
            other.transform.Rotate(0f, -15f, 0f);
        }
        else
        {
            _screamRight.Play();
        }

        GameManager.Instance._confirm = true;
        _spawnedNext = true;
    }

    private void OnTriggerExit(Collider other) => Destroy(gameObject);
}