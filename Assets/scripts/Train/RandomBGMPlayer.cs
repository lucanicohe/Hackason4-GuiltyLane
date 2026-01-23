using UnityEngine;

public class RandomBGMPlayer : MonoBehaviour
{
    public AudioClip[] bgmClips;

    private AudioSource audioSource;
    private int currentClipIndex = 0;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (audioSource == null || bgmClips == null || bgmClips.Length == 0)
            return;

        PlayCurrentClip();
    }

    void Update()
    {
        // Press R to switch to next song
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayNextClip();
            return;
        }

        // Auto-play next when finished
        if (!audioSource.isPlaying)
        {
            PlayNextClip();
        }
    }

    void PlayCurrentClip()
    {
        audioSource.clip = bgmClips[currentClipIndex];
        audioSource.loop = false;
        audioSource.Play();
    }

    void PlayNextClip()
    {
        currentClipIndex++;

        if (currentClipIndex >= bgmClips.Length)
            currentClipIndex = 0;

        PlayCurrentClip();
    }
}