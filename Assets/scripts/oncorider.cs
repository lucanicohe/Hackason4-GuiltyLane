using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oncorider : MonoBehaviour
{
    [SerializeField] private float _destroyTime = 30f;
    void OnCollisionEnter(Collision collision)
    {
        GameManager.Instance._confirm = false;
        Destroy(this.gameObject);
    }

    void Update()
    {
        _destroyTime -= Time.deltaTime;
        if (_destroyTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
