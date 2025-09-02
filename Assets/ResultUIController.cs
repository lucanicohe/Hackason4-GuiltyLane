
using TMPro;
using UnityEngine;

public class ResultUIController : MonoBehaviour
{
    [SerializeField] TMP_Text ResultDataText;
    void Start()
    {
        ResultDataText.text = "Your Data\n";
        ResultDataText.text += $"Round:\t\t\t{ResultData.RoundLevel}-{ResultData.problemCount}\n";
        ResultDataText.text += $"Royal Killed:\t\t{ResultData.TotleRoyalKilledNum}\n";
        ResultDataText.text += $"Citizen Killed:\t\t{ResultData.TotleCitizenKilledNum}\n";
        ResultDataText.text += $"Noble Killed:\t\t{ResultData.TotleNobleKilledNum}\n";
        ResultDataText.text += $"Prisoner Killed:\t\t{ResultData.TotlePrisonerKilledNum}\n";
        ResultDataText.text += $"Gold Earned:\t\t{ResultData.TotleGoldEarned}\n";
        ResultDataText.text += $"GuiltyPercent Earned:\t{ResultData.TotalGuiltyPercentEarned}\n";
        ResultDataText.text += $"Toll Payed:\t\t{ResultData.TotalTollPayed}\n";
    }
}
