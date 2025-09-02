using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _introImage;

    [SerializeField] private GameObject _audio;
    [SerializeField] private AudioSource _introsound;



    private bool _menuOpen = false;

    void Start()
    {
        _introImage.SetActive(false);
        
        StartCoroutine(Intro());
    }

  

    IEnumerator Intro()
    {
        yield return new WaitForSecondsRealtime(9f);
        Time.timeScale = 0;
        
        _introsound.Play(); 
        _audio.SetActive(false); 
        _introImage.SetActive(true); 
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1;
        
        _audio.SetActive(true); 
        _introImage.SetActive(false);
    }

   
}