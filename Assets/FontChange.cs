using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontChange : MonoBehaviour
{
    [Header("差し替えたいフォント")]
    [SerializeField] private TMP_FontAsset newFont;

    [ContextMenu("Change All TMP Fonts InScene")]
    
    private void ChangeAllTMPFonts()
    {
        if (newFont == null)
        {
            Debug.LogWarning("フォントが指定されてない");
            return;
        }

        //非アクティブなオブジェクトも含めて全て取得
        TMP_Text[] tmpTexts = FindObjectsOfType<TMP_Text>(true);
        int changedCount = 0;

        foreach (TMP_Text text in tmpTexts)
        {
            text.font = newFont;
            changedCount++;
        }
        
        Debug.Log($"✅ {changedCount}個のTextMeshProのフォントを「{newFont.name}」に変更しました。");
    }
}
