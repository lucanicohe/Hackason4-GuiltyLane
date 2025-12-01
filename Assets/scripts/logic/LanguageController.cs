using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Unity.VisualScripting;

public class LanguageController : MonoBehaviour
{
    // public LanguageType CurrentLanguage { get; private set; }

    // ReactivePropertyで、現在の言語とその変更を外部に効果
    private readonly ReactiveProperty<LanguageType> _currentLanguage = new(LanguageType.JP);
    
    public IReadOnlyReactiveProperty<LanguageType> CurrentLanguage => _currentLanguage;

    public void SwitchLang(LanguageType newLanguage)
    {
        if (_currentLanguage.Value != newLanguage)
        {
            _currentLanguage.Value = newLanguage;
        }
    }

    void Update()
    {
        // デバッグ用
        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     var newLanguage = _currentLanguage.Value == LanguageType.ENG ? 
        //         LanguageType.JP : LanguageType.ENG;
        //     SwitchLang(newLanguage);
        // }
    }
}
