using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UniRx;
using System;

public class CenterPocket : MonoBehaviour
{
    public bool _isEnterCenterPocket = false;

    private Subject<bool> _enterCenterPocket = new();
    public IObservable<bool> _onEnter => _enterCenterPocket;

    void Update()
    {
        
    }

    // CenterPoketに入ったときにする
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.name == "PachinkoBall")
        {
            Destroy(collisionInfo.gameObject);
            _isEnterCenterPocket = true;
            _enterCenterPocket.OnNext(_isEnterCenterPocket);

            if(_isEnterCenterPocket)
            _isEnterCenterPocket = false;
        }
    }
}
