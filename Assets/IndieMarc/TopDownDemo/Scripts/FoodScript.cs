using UnityEngine;
using IndieMarc.TopDown;

public class FoodItem : MonoBehaviour
{
    [Header("Food Settings")]
    [Tooltip("How much hunger this food restores when eaten.")]
    public float hungerRestoreAmount = 5f;

    [Tooltip("How long before the food respawns after being collected.")]
    public float respawnTime = 5f;

    [Tooltip("Tag of the object that can collect the food (e.g. 'Player').")]
    public string collectorTag = "Player";

    [Tooltip("Prevent this food from being carried by the player.")]
    public bool cannotBeCarried = true;

    private SpriteRenderer spriteRenderer;
    private Collider2D itemCollider;
    private Vector3 originalPosition;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<Collider2D>();
        originalPosition = transform.position;

        if (cannotBeCarried)
        {
            CarryItem carry = GetComponent<CarryItem>();
            if (carry != null)
                carry.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Touched: " + other.name);

        HungerBar hungerBar = other.GetComponentInParent<HungerBar>();
        if (hungerBar == null)
            hungerBar = other.GetComponentInChildren<HungerBar>();

        Debug.Log("Found HungerBar? " + (hungerBar != null));

        if (other.CompareTag(collectorTag))
        {
            if (hungerBar != null)
            {
                hungerBar.AddHunger(hungerRestoreAmount);
                Debug.Log($"{gameObject.name} eaten! Restored {hungerRestoreAmount} hunger.");
            }

            Collect();
        }
    }


    private void Collect()
    {
        spriteRenderer.enabled = false;
        itemCollider.enabled = false;
        Invoke(nameof(Respawn), respawnTime);
    }

    private void Respawn()
    {
        transform.position = originalPosition;
        spriteRenderer.enabled = true;
        itemCollider.enabled = true;
    }
}
