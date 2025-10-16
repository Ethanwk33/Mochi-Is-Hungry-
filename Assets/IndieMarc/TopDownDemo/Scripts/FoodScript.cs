using UnityEngine;

public class FoodItem : MonoBehaviour
{
    [Tooltip("How long before the food respawns after being collected.")]
    public float respawnTime = 5f;

    [Tooltip("Tag of the object that can collect the food (e.g. 'Player').")]
    public string collectorTag = "Player";

    private SpriteRenderer spriteRenderer;
    private Collider2D itemCollider;

    private void Awake()
    {
        // Cache components for enabling/disabling
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that touched the food has the right tag
        if (other.CompareTag(collectorTag))
        {
            Collect();
        }
    }

    private void Collect()
    {
        // Hide the food and disable its collider
        spriteRenderer.enabled = false;
        itemCollider.enabled = false;

        // Start the respawn timer
        Invoke(nameof(Respawn), respawnTime);
    }

    private void Respawn()
    {
        // Show the food again and re-enable its collider
        spriteRenderer.enabled = true;
        itemCollider.enabled = true;
    }
}
