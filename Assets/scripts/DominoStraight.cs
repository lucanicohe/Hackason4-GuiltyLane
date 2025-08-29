using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DominoStraight : MonoBehaviour
{
    [SerializeField] private GameObject _straightPrefab;
    [SerializeField] private GameObject _forkPrefab;
    [SerializeField] private Transform _spawnPointStraight;
    [SerializeField] private Transform _spawnPointFork;
    [SerializeField] private float _despawnDistance = 25f;

    
    [SerializeField] private Sprite _newSprite;   

    private bool _spawnedNext;
    private static int _globalStraightCount;
    private Transform _player;
    private SpriteRenderer _uiSprite;

    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) _player = p.transform;


        GameObject uiObj = GameObject.FindGameObjectWithTag("UIImage");
        if (uiObj != null) _uiSprite = uiObj.GetComponent<SpriteRenderer>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_spawnedNext || !other.CompareTag("Player")) return;

        if (_globalStraightCount < 2)
        {
            Instantiate(_straightPrefab, _spawnPointStraight.position, Quaternion.identity);
            _globalStraightCount++;

           
            if (_globalStraightCount == 2)
            {
                _uiSprite.sprite = _newSprite;
            }
        }
        else
        {
            Instantiate(_forkPrefab, _spawnPointFork.position, Quaternion.identity);
            _globalStraightCount = 0;
        }

        other.transform.rotation = Quaternion.identity;
        _spawnedNext = true;
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