using UnityEngine;

public class DespawnController : MonoBehaviour
{
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

        GameObject[] peoples = GameObject.FindGameObjectsWithTag("Peoples");
        foreach (var people in peoples)
        {
            Destroy(people);
        }
        Destroy(gameObject);
    }
}