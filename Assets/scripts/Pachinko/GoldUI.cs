using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    void Update()
    {
        goldText.text = $"おかね: {GameManager.Instance.Gold}";
    }
}