using UnityEngine;

public class RandomBGMPlayer : MonoBehaviour
{
    public AudioClip[] bgmClips;

    private AudioSource audioSource;
    private int _lastIndex = -1;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (audioSource == null || bgmClips == null || bgmClips.Length == 0)
            return;

        PlayRandomClip();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayRandomClip();
            return;
        }

        if (!audioSource.isPlaying)
        {
            PlayRandomClip();
        }
    }

    void PlayRandomClip()
    {
        if (bgmClips.Length == 1)
            return;

        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, bgmClips.Length);
        }
        while (randomIndex == _lastIndex);

        _lastIndex = randomIndex;

        audioSource.clip = bgmClips[randomIndex];
        audioSource.loop = false;
        audioSource.Play();
    }
}