using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnController : MonoBehaviour
{
    [SerializeField] private float _destroyTime = 30.0f;
    [SerializeField] private GameObject _bloodPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Instantiate(_bloodPrefab, transform.position, Quaternion.identity);

        
        GameObject screamObj = GameObject.FindGameObjectWithTag("Scream");
        if (screamObj != null)
        {
            Destroy(screamObj);
        }

        Destroy(gameObject);
    }

    void Update()
    {
        _destroyTime -= Time.deltaTime;
        if (_destroyTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}