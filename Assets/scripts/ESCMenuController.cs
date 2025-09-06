using UnityEngine;

public class ESCMenuController : MonoBehaviour
{
    [SerializeField] private GameObject escMenu;
    private bool escMenuToggled = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {   
            Debug.Log("ESC pressed");
            Time.timeScale = escMenuToggled? 1 : 0;
            escMenuToggled = !escMenuToggled;
            escMenu.SetActive(escMenuToggled);
        }
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void BackToMainGame()
    {
        escMenu.SetActive(false);
        escMenuToggled = false;
        Time.timeScale = 1;
    }

    public void SaveUserData()
    {

    }
    
    public void LoadUserData()
    {
        
    }
}
