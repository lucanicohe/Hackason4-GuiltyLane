using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SequentialAudioLooper : MonoBehaviour
{
    public AudioClip[] clips;

    [Range(0f, 180f)]
    public float frontAngle = 60f;

    private AudioSource audioSource;
    private Transform listener;

    private int currentClipIndex;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.loop = false; // REQUIRED for sequential playback

        AudioListener l = FindObjectOfType<AudioListener>();
        if (l != null)
            listener = l.transform;

        if (clips == null || clips.Length == 0)
            return;

        AssignInitialClipIndex();
        ApplyPitchVariation();
        PlayCurrentClipWithRandomOffset();
    }

    void Update()
    {
        HandleCone();
        HandleSequentialPlayback();
    }

    // Ensures different starting clips per instance
    void AssignInitialClipIndex()
    {
        currentClipIndex = Mathf.Abs(GetInstanceID()) % clips.Length;
        audioSource.clip = clips[currentClipIndex];
    }

    void ApplyPitchVariation()
    {
        audioSource.pitch = Random.Range(0.9f, 1f);
    }

    void PlayCurrentClipWithRandomOffset()
    {
        audioSource.time = Random.Range(0f, audioSource.clip.length);
        audioSource.Play();
    }

    // Moves to the next clip when the current one finishes
    void HandleSequentialPlayback()
    {
        if (audioSource.isPlaying)
            return;

        currentClipIndex = (currentClipIndex + 1) % clips.Length;
        audioSource.clip = clips[currentClipIndex];
        audioSource.Play();
    }

    void HandleCone()
    {
        if (listener == null)
            return;

        Vector3 toListener = (listener.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, toListener);

        audioSource.mute = angle > frontAngle * 0.5f;
    }
}