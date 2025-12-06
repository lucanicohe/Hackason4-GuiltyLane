using UnityEngine;

public class DespawnController : MonoBehaviour
{
    [SerializeField] private GameObject _bloodPrefab;
    [SerializeField] private AudioSource _boooom;

    //sprites for blood
    [SerializeField] private Sprite[] _bloodSprites;
    private int _currentSpriteIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // instantiate blood prefab as before
        Instantiate(_bloodPrefab, transform.position, Quaternion.identity);
        _boooom.Play();

        //  the blood object
        GameObject bloodObj = GameObject.FindGameObjectWithTag("Blood");
        if (bloodObj == null) return;

        var sr = bloodObj.GetComponent<SpriteRenderer>();
        if (sr == null || _bloodSprites == null || _bloodSprites.Length == 0) return;

        // sprite index
        _currentSpriteIndex = (_currentSpriteIndex + 1) % _bloodSprites.Length;
        sr.sprite = _bloodSprites[_currentSpriteIndex];
    }
}