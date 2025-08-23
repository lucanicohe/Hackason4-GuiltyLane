using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; } = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
