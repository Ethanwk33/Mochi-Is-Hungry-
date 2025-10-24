using UnityEngine;
using IndieMarc.TopDown;

public class FoodAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip eatSound;        // Assign your unique food sound here
    private AudioSource audioSource;

    void Awake()
    {
        // Add an AudioSource component if not already present
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player (Mochi) touches the food
        if (other.CompareTag("Player"))
        {
            PlayEatSound();
            // Optional: destroy the food after being eaten
            Destroy(gameObject);
        }
    }

    void PlayEatSound()
    {
        if (eatSound != null)
        {
            audioSource.PlayOneShot(eatSound);
        }
        else
        {
            Debug.LogWarning("Eat sound not assigned on " + gameObject.name);
        }
    }
}
