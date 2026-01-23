using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenParticle : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem fireWorkParticle;
    [SerializeField]
    private ParticleSystem goldenShawerParticle;

    // test
    //
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         IEnumerator ie = PlayGoldenShawerParticleCor();
    //         StartCoroutine(ie);
    //     }
    // }

    // 5回パーティクルを出すのを0.3秒間隔で2回行うコルーチン
    public IEnumerator PlayFireWorkParticleCor()
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

    // 150個のパーティクルを0.1秒間隔で20回出す
    public IEnumerator PlayGoldenShawerParticleCor()
    {
        for (int i = 0; i < 20; i++)
        {
            goldenShawerParticle.Emit(150);
            yield return new WaitForSeconds(0.1f);
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
