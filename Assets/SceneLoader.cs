using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadPachinkoScene()
    {
        SceneManager.LoadSceneAsync("Pachinko",LoadSceneMode.Additive);
    }

    public void UnLoadPachikoScene()
    {
        SceneManager.UnloadSceneAsync("Pachinko");
    }
}
