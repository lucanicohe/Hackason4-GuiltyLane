using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    public Fade fade;

    private System.Action titleAction;
    private System.Action mainGameAction;

    private void Start()
    {
        titleAction = () => SceneManager.LoadScene("TitleScene");
        mainGameAction = () => SceneManager.LoadScene("SampleScene");
    }

    public void FadeInToTitleScene()
    {
        fade.FadeIn(2.0f, titleAction);
    }
    
    public void FadeInToMainGameScene()
    {
        fade.FadeIn(2.0f, mainGameAction);
    }
}
