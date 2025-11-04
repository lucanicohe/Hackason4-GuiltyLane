using UnityEngine;

public class DespawnController : MonoBehaviour
{
    [SerializeField] private GameObject _bloodPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Instantiate(_bloodPrefab, transform.position, Quaternion.identity);
    }
}