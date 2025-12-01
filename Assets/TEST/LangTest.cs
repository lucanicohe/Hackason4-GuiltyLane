using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LangTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tmp;
    [SerializeField] private LanguageController _langController;
    private int _index = 1;
    private int _idOffset = 1000;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Test();
            Debug.Log("(- O -)");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeENG();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            ChangeJP();
        }
    }

    void ChangeENG()
    {
        _langController.SwitchLang(LanguageType.ENG);
    }

    void ChangeJP()
    {
        _langController.SwitchLang(LanguageType.JP);
    }


    void Test()
    {
        _tmp.text = CSVRender.Instance.TryGetTxtByID(_idOffset + _index);
        if (_index < 3)
            _index++;
        else 
            _index = 1;
    }
    
    
}
