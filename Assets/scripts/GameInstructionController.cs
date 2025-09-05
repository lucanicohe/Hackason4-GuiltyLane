using UnityEngine;

public class GameInstructionController : MonoBehaviour
{
    [SerializeField] private GameObject _instructionImage;

    void Start()
    {
        
        _instructionImage.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void StartMainGame()
    {
        Time.timeScale = 1f; 
        _instructionImage.SetActive(false);
    }
}