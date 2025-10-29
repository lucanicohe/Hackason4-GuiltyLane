using System;
using System.Collections;
using System.Text;
using TMPro;
using UniRx;
using UnityEngine;

public class MainUIViewer : Singleton<MainUIViewer>
{   
    
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI guiltyText;
    [SerializeField] private TextMeshProUGUI roundText; 
    [SerializeField] private TextMeshProUGUI turnText; 
    [SerializeField] private TextMeshProUGUI tollText; 
    [SerializeField] private TextMeshProUGUI trackText;
    [SerializeField] private TextMeshProUGUI teamRightTexts;
    [SerializeField] private TextMeshProUGUI teamLeftTexts;
    
    // [Header("Audios")]
    // [SerializeField] private AudioSource typingAudio;
    // [SerializeField] private AudioSource typingEndAudio;
    
    private CompositeDisposable _disposable = new();
    private void Start()
    {
        UpdateTrainNormalText();
    }

    public void Bind(GameContext gameContext)
    {
        gameContext.Gold
            .ObserveEveryValueChanged(v => v)
            .Subscribe(v =>
            {
                goldText.text = v.ToString();
            })
            .AddTo(_disposable);
        gameContext.GuiltyPercent
            .ObserveEveryValueChanged(v => v)
            .Subscribe(v =>
            {
                guiltyText.text = v.ToString();
            })
            .AddTo(_disposable);
        gameContext.Round
            .ObserveEveryValueChanged(v => v)
            .Subscribe(v =>
            {
                roundText.text = v.ToString();
            })
            .AddTo(_disposable);
        gameContext.Turn
            .ObserveEveryValueChanged(v => v)
            .Subscribe(v =>
            {
                turnText.text = v.ToString();
            })
            .AddTo(_disposable);
        gameContext.Toll
            .ObserveEveryValueChanged(v => v)
            .Subscribe(v =>
            {
                tollText.text = v.ToString();
            })
            .AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Dispose();
    }

    public void SetUpdateTrainText(HitPersonData rightData, HitPersonData leftData)
    {
        AlertSequence(rightData, teamRightTexts);
        AlertSequence(leftData, teamLeftTexts);
    }
    
    private void AlertSequence(HitPersonData data,TextMeshProUGUI text)
    {
        text.enabled = true;
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("processing...\n\nRIGHT\nRole: " + data.SelectRoleToShow);
        sb.AppendLine("Members: " + data.PersonnumToShow);
        sb.AppendLine("Inheritance: " + data.TotalInheritanceToShow);
        sb.AppendLine("Guilty %: " + data.TotalGuiltyPercentToShow);
        sb.AppendLine("user choice required...");
        StartCoroutine(TypewriterEffect(text, sb.ToString(), 0.09f));
    }

    private IEnumerator TypewriterEffect(TextMeshProUGUI textComponent, string fullText, float delay)
    {
        textComponent.text = "";
        
        foreach (var c in fullText)
        {
            textComponent.text += c;
            // typingAudio?.PlayOneShot(typingAudio.clip);
            yield return new WaitForSeconds(delay);
        }
        // typingEndAudio?.PlayOneShot(typingEndAudio.clip);
    }

    public IEnumerator UpdateTrainText()
    {
        trackText.color = Color.red;
        trackText.text = "Status: alert\nTrack: fork\nStop train";
        yield return new WaitForSeconds(2f);
        trackText.text = "";
    }

    private void UpdateTrainNormalText()
    {
        trackText.color = Color.green;
        trackText.text = "Status: normal\nTrack: straight\nContinue driving";
        teamRightTexts.enabled = false;
        teamLeftTexts.enabled = false;
    }

}
