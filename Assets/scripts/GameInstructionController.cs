using UnityEngine;

public class GameInstructionController : MonoBehaviour
{
    [SerializeField] private GameObject InstructionImage;
    void Awake()
    {
        Time.timeScale = 0;
    }

    public void StartMainGame()
    {
        Time.timeScale = 1;
        InstructionImage.SetActive(false);
    }
}
