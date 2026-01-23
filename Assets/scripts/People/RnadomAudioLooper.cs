using UnityEngine;

public class SequentialAudioLooper : MonoBehaviour
{
    public AudioClip[] clips;

    [Range(0f, 180f)]
    public float frontAngle = 60f;

    private AudioSource audioSource;
    private int clipIndex;
    private Transform listener;

    private static int instanceCounter = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // REQUIRED for cone logic
        audioSource.spatialBlend = 1f;

        AudioListener l = FindObjectOfType<AudioListener>();
        if (l != null)
            listener = l.transform;

        if (clips == null || clips.Length == 0)
            return;

        clipIndex = instanceCounter % clips.Length;
        instanceCounter++;

        PlayCurrentClip();
    }

    void Update()
    {
        HandleCone();

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

    // -------- CONE AUDIO ONLY --------
    void HandleCone()
    {
        if (listener == null) return;

        Vector3 toListener = (listener.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, toListener);

        audioSource.mute = angle > frontAngle * 0.5f;
    }
}