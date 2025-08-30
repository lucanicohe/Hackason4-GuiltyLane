using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour 
{
    [SerializeField]
    public Fade fade;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            fade.FadeIn(1f);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            fade.FadeOut(1f);
        }
    }
}