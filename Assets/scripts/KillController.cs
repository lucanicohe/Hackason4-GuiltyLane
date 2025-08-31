using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillController : MonoBehaviour
{
    [SerializeField] private GameObject bloodPrefab;
    [SerializeField] private float _destroyTime = 30.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;


        if (bloodPrefab != null)
            Instantiate(bloodPrefab, transform.position, Quaternion.identity);


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