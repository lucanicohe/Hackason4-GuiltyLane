using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillController : MonoBehaviour
{
    [SerializeField] private GameObject bloodPrefab; 

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        
        if (bloodPrefab != null)
            Instantiate(bloodPrefab, transform.position, Quaternion.identity);

        
        Destroy(gameObject);
    }
}