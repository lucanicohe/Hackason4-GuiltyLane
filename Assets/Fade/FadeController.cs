using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField]
    private Fade fade;
    [SerializeField]
    private float _fadeTime = 1f;
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.O))
        // {
        //     fade.FadeIn(_fadeTime);
        // }

        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     fade.FadeOut(_fadeTime);
        // }
    }
}
