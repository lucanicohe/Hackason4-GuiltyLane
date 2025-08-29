using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightChoice : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image ChoiceLeftImage;
    [SerializeField]
    private UnityEngine.UI.Image ChoiceRightImage;
    [SerializeField]
    private UnityEngine.UI.Image ChoiceNothingImage;
    public void LeftHighlight()
    {
        ChoiceLeftImage.color = new Color32(255, 255, 127, 255);
        ChoiceRightImage.color = new Color32(255, 255, 255, 255);
        ChoiceNothingImage.color = new Color32(255, 255, 255, 255);
    }

    public void RightHighLight()
    {
        ChoiceRightImage.color = new Color32(255, 255, 127, 255);
        ChoiceLeftImage.color = new Color32(255, 255, 255, 255);
        ChoiceNothingImage.color = new Color32(255, 255, 255, 255);
    }

    public void NothingHighlight()
    {
        ChoiceNothingImage.color = new Color32(255, 255, 127, 255);
        ChoiceRightImage.color = new Color32(255, 255, 255, 255);
        ChoiceLeftImage.color = new Color32(255, 255, 255, 255);
    }


}
