using System.Collections;
using UnityEngine;

public class SoundtrackController : MonoBehaviour
{
    public AudioClip soundtrack1;
    public AudioClip soundtrack2;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.3f;
        // Start playing a random soundtrack
        PlayRandomSoundtrack();
    }

    void PlayRandomSoundtrack()
    {
        // Randomly select between soundtrack1 and soundtrack2
        AudioClip selectedSoundtrack = Random.Range(0f, 1f) < 0.5f ? soundtrack1 : soundtrack2;

        // Set the selected soundtrack to the AudioSource
        audioSource.clip = selectedSoundtrack;

        // Subscribe to the OnAudioClipFinished event to detect when the current soundtrack finishes
        audioSource.loop = false;
        audioSource.Play();
        StartCoroutine(WaitForSoundtrackCompletion());
    }

    IEnumerator WaitForSoundtrackCompletion()
    {
        // Wait until the current soundtrack finishes playing
        yield return new WaitForSeconds(audioSource.clip.length);

        // Play the next random soundtrack
        PlayRandomSoundtrack();
    }
}
