using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneKillController : MonoBehaviour
{
    [SerializeField] private GameObject _bloodPrefab;
    [SerializeField] private AudioSource _killsound; 
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _killsound.Play(); 

        
        Destroy(gameObject);
    }
}
