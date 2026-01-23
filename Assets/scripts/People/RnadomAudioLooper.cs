using UnityEngine;

public class SequentialAudioLooper : MonoBehaviour
{
    public AudioClip[] clips;

    [Range(0f, 180f)]
    public float frontAngle = 60f;

    private AudioSource audioSource;
    private Transform listener;

    private static int nextClipIndex = 0;
    private int assignedClipIndex;

    void Start()
{
    audioSource = GetComponent<AudioSource>();
    audioSource.spatialBlend = 1f;
    audioSource.loop = true;

    AudioListener l = FindObjectOfType<AudioListener>();
    if (l != null)
        listener = l.transform;

    if (clips == null || clips.Length == 0)
        return;

    AssignUniqueClipSequential();

    // Per-instance pitch variation
    audioSource.pitch = Random.Range(0.8f, 1.1f);

    PlayWithRandomOffset();
}

    void Update()
    {
        HandleCone();
    }

    void AssignUniqueClipSequential()
    {
        assignedClipIndex = nextClipIndex % clips.Length;
        nextClipIndex++;
    }

    void PlayWithRandomOffset()
    {
        audioSource.pitch = Random.Range(0.9f, 1.0f);
        audioSource.clip = clips[assignedClipIndex];

        double dspNow = AudioSettings.dspTime;
        double offset = Random.Range(0f, audioSource.clip.length);

        audioSource.time = (float)offset;
        audioSource.PlayScheduled(dspNow + Random.Range(0.0f, 0.5f));
    }

    void HandleCone()
    {
        if (listener == null) return;

        Vector3 toListener = (listener.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, toListener);

        audioSource.mute = angle > frontAngle * 0.5f;
    }
}