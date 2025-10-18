using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class MainUIManager : Singleton<MainUIManager>
{   
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _goldGuiltText;
    [SerializeField] private TextMeshProUGUI _tollForkText;
    [SerializeField] private TextMeshProUGUI _trackText;
    [SerializeField] private TextMeshProUGUI _teamRightTexts;
    [SerializeField] private TextMeshProUGUI _teamLeftTexts;
    
    [Header("Audios")]
    [SerializeField] private AudioSource _typingAudio;
    [SerializeField] private AudioSource _typingEndAudio;

    private void Start()
    {
        _trackText.color = Color.green;
        _trackText.text = "Status: normal\nTrack: straight\nContinue driving";
        
        _teamRightTexts.enabled = false;
        _teamLeftTexts.enabled = false;
    }

    public IEnumerator AlertSequence()
    {
        _trackText.color = Color.red;
        _trackText.text = "Status: alert\nTrack: fork\nStop train";
        yield return new WaitForSeconds(2f);
        _trackText.text = "";
        
        var teamA = GameManager.Instance.RightHPD;
        var teamB = GameManager.Instance.LeftHPD;
        StringBuilder teamAText = new StringBuilder();
        StringBuilder teamBText = new StringBuilder();

        teamAText.AppendLine("processing...\n\nRIGHT\nRole: " + teamA.SelectRoleToShow);
        teamAText.AppendLine("Members: " + (teamA.PersonnumToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamA.PersonnumToShow) );
        teamAText.AppendLine("Inheritance: " + (teamA.TotalInheritanceToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamA.TotalInheritanceToShow));
        teamAText.AppendLine("Guilty %: " + (teamA.TotalGuiltyPercentToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamA.TotalGuiltyPercentToShow));
        teamAText.AppendLine("user choice required...");

        teamBText.AppendLine("processing...\n\nLEFT\nRole: " + teamB.SelectRoleToShow);
        teamBText.AppendLine("Members: " + (teamB.PersonnumToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamB.PersonnumToShow));
        teamBText.AppendLine("Inheritance: " + (teamB.TotalInheritanceToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamB.TotalInheritanceToShow));
        teamBText.AppendLine("Guilty %: " + (teamB.TotalGuiltyPercentToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamB.TotalGuiltyPercentToShow));
        teamBText.AppendLine("nuser choice required...");

        
        _teamRightTexts.enabled = true;
        _teamLeftTexts.enabled = true;
        StartCoroutine(TypewriterEffect(_teamRightTexts, teamAText.ToString(), 0.09f));
        StartCoroutine(TypewriterEffect(_teamLeftTexts, teamBText.ToString(), 0.09f));
    }

    private IEnumerator TypewriterEffect(TextMeshProUGUI textComponent, string fullText, float delay)
    {
        textComponent.text = "";
        
        foreach (var c in fullText)
        {
            textComponent.text += c;
            _typingAudio?.PlayOneShot(_typingAudio.clip);
            yield return new WaitForSeconds(delay);
        }
        _typingEndAudio?.PlayOneShot(_typingEndAudio.clip);
    }

}
