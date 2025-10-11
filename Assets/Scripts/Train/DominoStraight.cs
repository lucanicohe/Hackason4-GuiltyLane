using System.Collections;
using UnityEngine;
using TMPro;

public class DominoStraight : MonoBehaviour
{
    [SerializeField] private GameObject _straightPrefab;
    [SerializeField] private GameObject _forkPrefab;
    [SerializeField] private Transform _spawnPointStraight;
    [SerializeField] private Transform _spawnPointFork;
    [SerializeField] private float _despawnDistance = 25f;

    // ===== ADD THIS =====
    [SerializeField] private Waypoint _straightWaypoint; // Single waypoint for rotation
    // ====================

    [Header("Team Spawn Points")]
    [SerializeField] private Transform _spawnPointRightTeam;
    [SerializeField] private Transform _spawnPointLeftTeam;

    [Header("Team Prefabs")]
    [SerializeField] private GameObject _prisonerPrefab;
    [SerializeField] private GameObject _citizenPrefab;
    [SerializeField] private GameObject _royalPrefab;
    [SerializeField] private GameObject _noblePrefab;
    [SerializeField] private GameObject _ConfirmfalsePrefab;

    private AudioSource _typingAudio;
    private AudioSource _typingEndAudio;

    private bool _spawnedNext;
    private static int _globalStraightCount;
    private Transform _player;
    private TeamBuilder _teamBuilder;
    private TextMeshProUGUI[] _teamRightTexts;
    private TextMeshProUGUI[] _teamLeftTexts;
    private TextMeshProUGUI _trackText;

    private void Start()
    {
        _typingAudio = GameObject.FindGameObjectWithTag("Typing")?.GetComponent<AudioSource>();
        _typingEndAudio = GameObject.FindGameObjectWithTag("TypingEnd")?.GetComponent<AudioSource>();

        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _trackText = GameObject.FindGameObjectWithTag("TrackText")?.GetComponent<TextMeshProUGUI>();
        _trackText.color = Color.green;
        _trackText.text = "Status: normal\nTrack: straight\nContinue driving";

        var rightObjs = GameObject.FindGameObjectsWithTag("TeamRight");
        _teamRightTexts = new TextMeshProUGUI[rightObjs.Length];
        for (int i = 0; i < rightObjs.Length; i++)
        {
            _teamRightTexts[i] = rightObjs[i].GetComponent<TextMeshProUGUI>();
            _teamRightTexts[i].enabled = false;
        }

        var leftObjs = GameObject.FindGameObjectsWithTag("TeamLeft");
        _teamLeftTexts = new TextMeshProUGUI[leftObjs.Length];
        for (int i = 0; i < leftObjs.Length; i++)
        {
            _teamLeftTexts[i] = leftObjs[i].GetComponent<TextMeshProUGUI>();
            _teamLeftTexts[i].enabled = false;
        }

        _teamBuilder = FindObjectOfType<TeamBuilder>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_spawnedNext || !other.CompareTag("Player")) return;

        _trackText.color = Color.green;
        _trackText.text = "Status: normal\nTrack: straight\nContinue driving";

        // ===== SET ROTATION WAYPOINT ONCE =====
        var train = other.GetComponent<TrainController>();
        if (train != null && _straightWaypoint != null)
        {
            train.SetRotationWaypoint(_straightWaypoint);
        }
        // ======================================

        _spawnedNext = true;

        if (_globalStraightCount < 2)
        {
            Instantiate(_straightPrefab, _spawnPointStraight.position, Quaternion.identity);
            _globalStraightCount++;
        }
        else
        {
            StartCoroutine(AlertSequence());
        }
    }

    // ... rest of your code stays the same ...
    
    private IEnumerator AlertSequence()
    {
        _trackText.color = Color.red;
        _trackText.text = "Status: alert\nTrack: fork\nStop train";
        yield return new WaitForSeconds(2f);
        _trackText.text = "";

        Instantiate(_forkPrefab, _spawnPointFork.position, Quaternion.identity);
        _globalStraightCount = 0;

        if (_teamBuilder != null)
        {
            var teamA = GameManager.Instance.RightHPD;
            var teamB = GameManager.Instance.LeftHPD;

            string teamAText =
                "processing...\n\nRIGHT\nRole: " + teamA.SelectRoleToShow +
                "\nMembers: " + (teamA.PersonnumToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamA.PersonnumToShow) +
                "\nInheritance: " + (teamA.TotalInheritanceToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamA.TotalInheritanceToShow) +
                "\nGuilty %: " + (teamA.TotalGuiltyPercentToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamA.TotalGuiltyPercentToShow) +
                "\n\nuser choice required...";

            string teamBText =
                "processing...\n\nLEFT\nRole: " + teamB.SelectRoleToShow +
                "\nMembers: " + (teamB.PersonnumToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamB.PersonnumToShow) +
                "\nInheritance: " + (teamB.TotalInheritanceToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamB.TotalInheritanceToShow) +
                "\nGuilty %: " + (teamB.TotalGuiltyPercentToShow == GameManager.BE_DISGUISED_INT ? GameManager.BE_DISGUISED_STRING : teamB.TotalGuiltyPercentToShow) +
                "\n\nuser choice required...";

            foreach (var text in _teamRightTexts)
            {
                text.enabled = true;
                StartCoroutine(TypewriterEffect(text, teamAText, 0.09f));
            }

            foreach (var text in _teamLeftTexts)
            {
                text.enabled = true;
                StartCoroutine(TypewriterEffect(text, teamBText, 0.09f));
            }

            SpawnTeamNearPoint(teamA, _spawnPointRightTeam);
            SpawnTeamNearPoint(teamB, _spawnPointLeftTeam);
        }
    }

    private IEnumerator TypewriterEffect(TextMeshProUGUI textComponent, string fullText, float delay)
    {
        textComponent.text = "";
        
        foreach (char c in fullText)
        {
            textComponent.text += c;
            if (_typingAudio != null) 
                _typingAudio.Play(); 
            yield return new WaitForSeconds(delay);
        }
        if (_typingEndAudio != null) _typingEndAudio.Play();
    }

    private void Update()
    {
        if (_player.position.z - transform.position.z > _despawnDistance)
            Destroy(gameObject);
    }

    private void SpawnTeamNearPoint(TeamBuilder.HitPersonData team, Transform spawnPoint)
    {
        float offsetRadius = 5f;
        Vector3 basePos = spawnPoint.position;

        GameObject prefabToSpawn = team.RoleID switch
        {
            0 => _royalPrefab,
            1 => _noblePrefab,
            2 => _citizenPrefab,
            3 => _prisonerPrefab,
            _ => null
        };

        if (prefabToSpawn == null) return;

        for (int i = 0; i < team.Personnum; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-offsetRadius, offsetRadius),
                0f,
                Random.Range(-offsetRadius, offsetRadius)
            );

            Instantiate(prefabToSpawn, basePos + randomOffset, Quaternion.identity);
            if (i == team.Personnum -1)
            {
                Instantiate(_ConfirmfalsePrefab, basePos, Quaternion.identity);
            }
        }
    }
}