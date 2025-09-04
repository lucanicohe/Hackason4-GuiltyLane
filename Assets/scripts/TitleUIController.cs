using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIController : MonoBehaviour
{
    [SerializeField] private Fade fade;
    public void LoadMainGameScene(string sceneName)
    {
        fade.FadeIn(2.0f, () =>
        {UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName); });
    }
}
