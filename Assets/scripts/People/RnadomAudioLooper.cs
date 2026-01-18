using UnityEngine;

public class SequentialAudioLooper : MonoBehaviour
{
    public AudioClip[] clips;

    private AudioSource audioSource;
    private int clipIndex;

    // Global counter shared by all instances
    private static int instanceCounter = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (clips == null || clips.Length == 0)
            return;

        // Assign a unique start index per instance
        clipIndex = instanceCounter % clips.Length;
        instanceCounter++;

        PlayCurrentClip();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            clipIndex = (clipIndex + 1) % clips.Length;
            PlayCurrentClip();
        }
    }

    void PlayCurrentClip()
    {
        audioSource.clip = clips[clipIndex];
        audioSource.Play();
    }
}