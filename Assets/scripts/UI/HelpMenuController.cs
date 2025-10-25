using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject helpMenu1;

    [SerializeField]
    private GameObject helpMenu2;

    public void OpenHelpMenu()
    {
        helpMenu1.SetActive(true);
    }

    public void OpenNextPage()
    {
        helpMenu2.SetActive(true);
        helpMenu1.SetActive(false);
    }
    
    public void OpenPreviousPage()
    {
        helpMenu1.SetActive(true);
        helpMenu2.SetActive(false);
    }

    public void CloseHelpMenu()
    {
        helpMenu1.SetActive(false);
        helpMenu2.SetActive(false);
    }
}