using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oncorider : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        GameManager.Instance._confirm = false;
        Destroy(this.gameObject);
    }
}
