using UnityEngine;

public class RandomBGMPlayer : MonoBehaviour
{
    public AudioClip[] bgmClips;

    private AudioSource audioSource;
    private int lastClipIndex = -1;

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
        
        

        // Auto-play next when finished
        if (!audioSource.isPlaying)
        {
            PlayRandomClip();
        }
    }

    void PlayRandomClip()
    {
        if (bgmClips.Length == 1)
        {
            audioSource.clip = bgmClips[0];
            audioSource.Play();
            return;
        }

        int nextIndex;
        do
        {
            nextIndex = Random.Range(0, bgmClips.Length);
        }
        while (nextIndex == lastClipIndex);

        lastClipIndex = nextIndex;

        audioSource.clip = bgmClips[nextIndex];
        audioSource.loop = false;
        audioSource.Play();
    }
}