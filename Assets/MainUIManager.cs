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
        UpdateTrainNormalText();
    }

    public void UpdateTrackText(HitPersonData teamLeft, HitPersonData teamRight)
    {
        StartCoroutine(AlertSequence(teamLeft, teamRight));
    }

    private IEnumerator AlertSequence(HitPersonData teamLeft, HitPersonData teamRight)
    {
        _trackText.color = Color.red;
        _trackText.text = "Status: alert\nTrack: fork\nStop train";
        yield return new WaitForSeconds(2f);
        _trackText.text = "";
        
        StringBuilder teamLeftText = new StringBuilder();
        StringBuilder teamRightText = new StringBuilder();

        teamLeftText.AppendLine("processing...\n\nRIGHT\nRole: " + teamLeft.SelectRoleToShow);
        teamLeftText.AppendLine("Members: " + (teamLeft.PersonnumToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamLeft.PersonnumToShow) );
        teamLeftText.AppendLine("Inheritance: " + (teamLeft.TotalInheritanceToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamLeft.TotalInheritanceToShow));
        teamLeftText.AppendLine("Guilty %: " + (teamLeft.TotalGuiltyPercentToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamLeft.TotalGuiltyPercentToShow));
        teamLeftText.AppendLine("user choice required...");

        teamRightText.AppendLine("processing...\n\nLEFT\nRole: " + teamRight.SelectRoleToShow);
        teamRightText.AppendLine("Members: " + (teamRight.PersonnumToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamRight.PersonnumToShow));
        teamRightText.AppendLine("Inheritance: " + (teamRight.TotalInheritanceToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamRight.TotalInheritanceToShow));
        teamRightText.AppendLine("Guilty %: " + (teamRight.TotalGuiltyPercentToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamRight.TotalGuiltyPercentToShow));
        teamRightText.AppendLine("nuser choice required...");

        
        _teamRightTexts.enabled = true;
        _teamLeftTexts.enabled = true;
        StartCoroutine(TypewriterEffect(_teamRightTexts, teamLeftText.ToString(), 0.09f));
        StartCoroutine(TypewriterEffect(_teamLeftTexts, teamRightText.ToString(), 0.09f));
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

    public void UpdateTrainNormalText()
    {
        _trackText.color = Color.green;
        _trackText.text = "Status: normal\nTrack: straight\nContinue driving";
        
        _teamRightTexts.enabled = false;
        _teamLeftTexts.enabled = false;
    }

}
