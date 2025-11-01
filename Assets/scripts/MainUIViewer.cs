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
    [SerializeField] private TextMeshProUGUI roundTurnText; 
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
            .Subscribe(v =>
            {
                goldText.text = v.ToString();
            })
            .AddTo(_disposable);
        gameContext.GuiltyPercent
            .Subscribe(v =>
            {
                guiltyText.text = v.ToString();
            })
            .AddTo(_disposable);
        gameContext.Toll
            .Subscribe(v =>
            {
                tollText.text = v.ToString();
            })
            .AddTo(_disposable);
        gameContext.Round.CombineLatest(
                gameContext.Turn,
                (round, turn) => $"{(int)round}-{(int)turn}"
            )
            .Subscribe(text => roundTurnText.text = text)
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
        sb.AppendLine("processing...");
        sb.AppendLine("Role: " + data.SelectRole);
        sb.AppendLine("Members: " + data.Personnum);
        sb.AppendLine("Inheritance: " + data.TotalInheritance);
        sb.AppendLine("Guilty %: " + data.TotalGuiltyPercent);
        sb.AppendLine("Choice Required...");
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

    public void UpdateTrainNormalText()
    {
        trackText.color = Color.green;
        trackText.text = "Status: normal\nTrack: straight\nContinue driving";
        teamRightTexts.enabled = false;
        teamLeftTexts.enabled = false;
    }

}
