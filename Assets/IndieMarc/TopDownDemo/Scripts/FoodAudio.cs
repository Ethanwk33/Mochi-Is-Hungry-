using UnityEngine;
using IndieMarc.TopDown;

public class FoodAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip eatSound; // Assign your unique food sound here

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player (Mochi) touches the food
        if (other.CompareTag("Player"))
        {
            PlayEatSound();
            Destroy(gameObject); // Safe to destroy immediately
        }
    }

    void PlayEatSound()
    {
        if (eatSound != null)
        {
            AudioSource.PlayClipAtPoint(eatSound, transform.position, 4.0f);
        }
        else
        {
            Debug.LogWarning("Eat sound not assigned on " + gameObject.name);
        }
    }
}
