using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoFork : MonoBehaviour
{
    [SerializeField] private GameObject _straightPrefab;
    [SerializeField] private Transform _spawnPointLeft;
    [SerializeField] private Transform _spawnPointRight;
    [SerializeField] private AudioSource _screamRight; 
    [SerializeField] private AudioSource _screamLeft;

    // ===== ADD WAYPOINT CHAINS =====
    [Header("Waypoints")]
    [SerializeField] private Waypoint _leftWaypointStart; // First of 3 left waypoints
    [SerializeField] private Waypoint _rightWaypointStart; // First of 3 right waypoints
    

    private bool _spawnedNext;

    private void OnTriggerEnter(Collider other)
    {
        if (_spawnedNext || !other.CompareTag("Player")) return;

        var train = other.GetComponent<TrainController>();
        if (train == null) return;

        bool goLeft = train._lastDirection == TrainController.Direction.Left;

        Instantiate(_straightPrefab,
            goLeft ? _spawnPointLeft.position : _spawnPointRight.position,
            Quaternion.identity);

        if (goLeft)
        {
            _screamLeft.Play();
            // Set waypoint chain for left curve (will auto-switch through 3 points)
            if (_leftWaypointStart != null)
                train.SetRotationWaypoint(_leftWaypointStart);
        }
        else
        {
            _screamRight.Play();
            // Set waypoint chain for right curve
            if (_rightWaypointStart != null)
                train.SetRotationWaypoint(_rightWaypointStart);
        }

        GameManager.Instance._confirm = true;
        _spawnedNext = true;
    }

    private void OnTriggerExit(Collider other) => Destroy(gameObject);


    // private List<float> _angles = new List<float>();
    // private float _curveTime = 1f; // 各カーブにかける時間
    // private float _currentTime;
    // private float _endAngle;
    // private float _startAngle;
    // private float _currentAngle;
    // private Quaternion _baseRotation; // 元の角度

    // /// <summary>
    // /// 左カーブするためのコルーチン
    // /// </summary>
    // private IEnumerator LeftCurve(Transform train)
    // {
    //     // 左方向のカーブなのでマイナス方向にカーブ
    //     //0 
    //     //7.5 (-7.5deg) 
    //     //15 (-7.5deg) 
    //     //7.5 (+7.5deg)
    //     //0 (+7.5deg)
    //     _angles.Add(0);
    //     _angles.Add(-7.5f);
    //     _angles.Add(-15f);
    //     _angles.Add(-7.5f);
    //     _angles.Add(0);

    //     _baseRotation = train.rotation;

    //     for (int i = 0; i < _angles.Count - 1; i++)
    //     {
    //         _startAngle = _angles[i];
    //         _endAngle = _angles[i + 1];
    //         _currentTime = 0f;

    //         while (_currentTime < 1f)
    //         {
    //             _currentTime += Time.deltaTime / _curveTime;
    //             _currentAngle = Mathf.Lerp(_startAngle, _endAngle, _currentTime);
    //             train.rotation = _baseRotation * Quaternion.Euler(0f, _currentAngle, 0f);
    //             yield return null;
    //         }
    //     }

    //     // 元の角度に戻す
    //     train.rotation = _baseRotation;
    // }
}
