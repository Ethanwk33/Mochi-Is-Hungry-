using UnityEngine;
using IndieMarc.TopDown;

public class FoodAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip eatSound; // unique food sounds for the script attatchment

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
            AudioSource.PlayClipAtPoint(eatSound, transform.position, 4.0f); // playing the eating sound with extra volume
        }
        //else
        //{
            //Debug.LogWarning("Eat sound not assigned on " + gameObject.name);
        //}
    }
}
