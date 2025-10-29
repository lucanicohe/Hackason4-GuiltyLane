using System.Collections;
using System.Text;
using TMPro;
using UniRx;
using UnityEngine;

public class MainUIViewer : Singleton<MainUIViewer>
{   
    
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI goldGuiltText;
    [SerializeField] private TextMeshProUGUI tollForkText; 
    [SerializeField] private TextMeshProUGUI trackText;
    [SerializeField] private TextMeshProUGUI teamRightTexts;
    [SerializeField] private TextMeshProUGUI teamLeftTexts;
    
    [Header("Audios")]
    [SerializeField] private AudioSource typingAudio;
    [SerializeField] private AudioSource typingEndAudio;
    
    private CompositeDisposable _disposable = new();
    private void Start()
    {
        UpdateTrainNormalText();
    }

    public void Bind(GameContext gameContext)
    {
        gameContext.GoldGuiltySum
            .Subscribe(value =>
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Gold : " + value.gold);
                sb.AppendLine("Guilty : " + value.guilty);
                goldGuiltText.text = sb.ToString();
            }).AddTo(_disposable);
        gameContext.TollForkSum
            .Subscribe(value =>
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Toll : " + value.toll);
                sb.AppendLine("Round : " + value.Round + "-" + value.Turn);
                tollForkText.text = sb.ToString();
            }).AddTo(_disposable);
    }
    
    public void UpdateTrackText(HitPersonData teamLeft, HitPersonData teamRight)
    {
        StartCoroutine(AlertSequence(teamLeft, teamRight));
    }
    private IEnumerator AlertSequence(HitPersonData teamLeft, HitPersonData teamRight)
    {
        trackText.color = Color.red;
        trackText.text = "Status: alert\nTrack: fork\nStop train";
        yield return new WaitForSeconds(2f);
        trackText.text = "";
        
        StringBuilder teamLeftText = new StringBuilder();
        StringBuilder teamRightText = new StringBuilder();

        teamLeftText.AppendLine("processing...\n\nRIGHT\nRole: " + teamLeft.SelectRoleToShow);
        teamLeftText.AppendLine("Members: " + teamLeft.PersonnumToShow);
        teamLeftText.AppendLine("Inheritance: " + teamLeft.TotalInheritanceToShow);
        teamLeftText.AppendLine("Guilty %: " + teamLeft.TotalGuiltyPercentToShow);
        teamLeftText.AppendLine("user choice required...");

        teamRightText.AppendLine("processing...\n\nLEFT\nRole: " + teamRight.SelectRoleToShow);
        teamRightText.AppendLine("Members: " + teamRight.PersonnumToShow);
        teamRightText.AppendLine("Inheritance: " + teamRight.TotalInheritanceToShow);
        teamRightText.AppendLine("Guilty %: " + teamRight.TotalGuiltyPercentToShow);
        teamRightText.AppendLine("user choice required...");

        
        teamRightTexts.enabled = true;
        teamLeftTexts.enabled = true;
        StartCoroutine(TypewriterEffect(teamRightTexts, teamLeftText.ToString(), 0.09f));
        StartCoroutine(TypewriterEffect(teamLeftTexts, teamRightText.ToString(), 0.09f));
    }

    private IEnumerator TypewriterEffect(TextMeshProUGUI textComponent, string fullText, float delay)
    {
        textComponent.text = "";
        
        foreach (var c in fullText)
        {
            textComponent.text += c;
            typingAudio?.PlayOneShot(typingAudio.clip);
            yield return new WaitForSeconds(delay);
        }
        typingEndAudio?.PlayOneShot(typingEndAudio.clip);
    }

    private void UpdateTrainNormalText()
    {
        trackText.color = Color.green;
        trackText.text = "Status: normal\nTrack: straight\nContinue driving";
        
        teamRightTexts.enabled = false;
        teamLeftTexts.enabled = false;
    }

}
