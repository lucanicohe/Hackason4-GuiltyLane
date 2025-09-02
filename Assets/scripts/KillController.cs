using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillController : MonoBehaviour
{
    [SerializeField] private GameObject bloodPrefab;
    [SerializeField] private float _destroyTime = 30.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (bloodPrefab != null)
        {
            Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySE(AudioManager.SEType.hit);
            switch (Random.Range(0, 2))
            {
                case 0:
                    AudioManager.Instance.PlaySE(AudioManager.SEType.悲鳴);
                    break;
                case 1:
                    AudioManager.Instance.PlaySE(AudioManager.SEType.悲鳴_2);
                    break;
                case 2:
                    AudioManager.Instance.PlaySE(AudioManager.SEType.悲鳴_3);
                    break;
            }
        }
        Destroy(gameObject);
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