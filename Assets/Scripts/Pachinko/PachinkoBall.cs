using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PachinkoBall : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D _rb;
    // [SerializeField,Range(0f, 50f)]
    public float _speed = 20f;
    float _randomOffsetX = 0.1f;

    // 玉に初速をつける
    void Start()
    {
        float offsetX = Random.Range(-_randomOffsetX, _randomOffsetX);

        Vector3 velocity = new Vector3(offsetX, 1, 0);
        _rb.AddForce(velocity * _speed, ForceMode2D.Impulse);
    }

    // 当たったら消える
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.name == "yuka")
        {
            Destroy(this.gameObject);
        }
    }
}
