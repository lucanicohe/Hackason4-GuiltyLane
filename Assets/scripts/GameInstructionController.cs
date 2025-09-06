using System.Collections;
using UnityEngine;

public class GameInstructionController : MonoBehaviour
{
    [SerializeField] private GameObject _instructionImage;
    [SerializeField] private AudioSource _audio;
  
    [SerializeField] private GameObject _introImage;

    void Start()
    {

        _instructionImage.SetActive(true);
        Time.timeScale = 0f;
    }

    public void StartMainGame()
    {
        Time.timeScale = 1f;
        _instructionImage.SetActive(false);
        StartCoroutine(Intro());
    }

    public IEnumerator Intro()
    {
       
        
        



        _introImage.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _audio.Play(); 
        


        _introImage.SetActive(false);
        
    }
}