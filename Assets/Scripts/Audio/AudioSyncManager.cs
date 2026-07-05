using UnityEngine;

/// <summary>
/// Synchronizes audio playback with animation timing.
/// Implements lip-sync by syncing audio duration to animation clip length.
/// </summary>
public class AudioSyncManager : MonoBehaviour
{
    private AudioSource audioSource;
    private Animator petAnimator;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        petAnimator = GetComponentInParent<Animator>();
    }

    public void PlayDialogueWithSync(AudioClip clip)
    {
        if (audioSource == null || petAnimator == null)
            return;

        // Calculate animation duration from audio clip length
        float duration = clip.length;

        // Trigger talk animation
        petAnimator.SetTrigger("talk");

        // Sync animation speed to audio duration
        petAnimator.SetFloat("TalkDuration", duration);

        // Play audio
        audioSource.PlayOneShot(clip);

        Debug.Log($"[AudioSyncManager] Playing dialogue - duration: {duration}s");
    }

    public void StopDialogue()
    {
        if (audioSource != null)
            audioSource.Stop();

        if (petAnimator != null)
            petAnimator.SetTrigger("idle");
    }

    public float GetCurrentAudioPosition()
    {
        return audioSource != null ? audioSource.time : 0f;
    }
}
