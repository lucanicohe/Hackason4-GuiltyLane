using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class PachinkoBall : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D _rb;
    public float _speed = 20f;
    float _randomOffsetX = 0.1f;

    public bool _isEnterCenterPocket = false;

    private Subject<bool> _enterCenterPocket = new();
    public IObservable<bool> _onEnter => _enterCenterPocket;
    

    // 玉に初速をつける
    void Start()
    {
        float offsetX = UnityEngine.Random.Range(-_randomOffsetX, _randomOffsetX);

        Vector3 velocity = new Vector3(offsetX, 1, 0);
        _rb.AddForce(velocity * _speed, ForceMode2D.Impulse);
    }

    // 当たったら消える CenterPocketに当たったなら、抽選開始
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.name == "yuka")
        {
            Destroy(this.gameObject);
        }
        else if(collisionInfo.gameObject.name == "CenterPocket")
        {
            
            _isEnterCenterPocket = true;
            _enterCenterPocket.OnNext(_isEnterCenterPocket);

            if(_isEnterCenterPocket)
            _isEnterCenterPocket = false;

            Destroy(this.gameObject);
        }
    }
}
