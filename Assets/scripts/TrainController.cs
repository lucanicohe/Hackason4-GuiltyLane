using UnityEngine;

public class TrainController : MonoBehaviour
{
    [Header("Train Settings")]
    [SerializeField] private float _speed = 30f;

    [Header("Track Settings")]
    [SerializeField] private GameObject _straightPrefab;
    [SerializeField] private float _sectionLength = 216f; // distance between sections

    private GameObject _lastSpawnedSection;

    private void Start()
    {
        // Spawn the first section manually
        _lastSpawnedSection = Instantiate(_straightPrefab, new Vector3(1.77f, 2.51f, 239.4f), Quaternion.identity);
    }

    private void Update()
    {
        // Move train forward
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    // This will be called by the collider at the end of each track section
    public void SpawnNextSection()
    {
        if (_lastSpawnedSection == null) return;

        Vector3 nextPos = _lastSpawnedSection.transform.position + new Vector3(0, 0, _sectionLength);
        _lastSpawnedSection = Instantiate(_straightPrefab, nextPos, Quaternion.identity);
    }
}