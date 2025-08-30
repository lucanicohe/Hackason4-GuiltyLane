using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class DominoStraight : MonoBehaviour
{
    [SerializeField] private GameObject _straightPrefab;
    [SerializeField] private GameObject _forkPrefab;
    [SerializeField] private Transform _spawnPointStraight;
    [SerializeField] private Transform _spawnPointFork;
    [SerializeField] private float _despawnDistance = 25f;

    [SerializeField] private Sprite _newSprite;    
    [SerializeField] private Sprite _changeSprite; 
    [SerializeField] private Sprite _defaultSprite; 

    private bool _spawnedNext;
    private static int _globalStraightCount;
    private Transform _player;
    private SpriteRenderer _uiSprite;

    private TMP_Text[] _numberTexts; 

    private void Start()
    {
        // find player
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) _player = p.transform;

        // find UI sprite
        GameObject uiObj = GameObject.FindGameObjectWithTag("UIImage");
        if (uiObj != null)
        {
            _uiSprite = uiObj.GetComponent<SpriteRenderer>();
            _uiSprite.sprite = _defaultSprite; 
        }

        // find TMP number texts
        GameObject[] numberObjects = GameObject.FindGameObjectsWithTag("Numbers");
        _numberTexts = new TMP_Text[numberObjects.Length];
        for (int i = 0; i < numberObjects.Length; i++)
        {
            _numberTexts[i] = numberObjects[i].GetComponent<TMP_Text>();
            _numberTexts[i].enabled = false; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_spawnedNext || !other.CompareTag("Player")) return;

        _uiSprite.sprite = _defaultSprite;

        if (_globalStraightCount < 2)
        {
            Instantiate(_straightPrefab, _spawnPointStraight.position, Quaternion.identity);
            _globalStraightCount++;
        }
        else
        {
            Instantiate(_forkPrefab, _spawnPointFork.position, Quaternion.identity);
            _globalStraightCount = 0;

            if (_uiSprite != null)
            {
                StartCoroutine(AlertSequence());
            }
        }

        other.transform.rotation = Quaternion.identity;
        _spawnedNext = true;
    }

    private IEnumerator AlertSequence()
    {
        _uiSprite.sprite = _newSprite; 
        yield return new WaitForSeconds(1f); 
        _uiSprite.sprite = _changeSprite;

        
        foreach (var t in _numberTexts)
        {
            t.enabled = true;
        }
    }

    private void Update()
    {
        if (_player == null) return;

        if (_player.position.z - transform.position.z > _despawnDistance)
        {
            Destroy(gameObject);
        }
    }
}