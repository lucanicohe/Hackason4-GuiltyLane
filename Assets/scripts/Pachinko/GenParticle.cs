using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenParticle : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem fireWorkParticle;

    // test
    //
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         IEnumerator ie = PlaySlotParticleCor();
    //         StartCoroutine(ie);
    //     }
    // }

    // 5回パーティクルを出すのを0.3秒間隔で2回行うコルーチン
    public IEnumerator PlaySlotParticleCor()
    {
        for (int i = 0; i < 5; i++)
        {
            GenerateFireWorkParticle();
        }

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < 5; i++)
        {
            GenerateFireWorkParticle();
        }

        yield break;
    }

    // パーティクルの生成
    private void GenerateFireWorkParticle()
    {
        // shape項目の取得
        ParticleSystem.ShapeModule shape = fireWorkParticle.shape;

        // 飛ばす角度の決定
        Vector3 rot = shape.rotation;
        rot.y = Random.Range(-30, 31);

        shape.rotation = rot;

        // パーティクルを飛ばす
        fireWorkParticle.Emit(1);
    }
}
