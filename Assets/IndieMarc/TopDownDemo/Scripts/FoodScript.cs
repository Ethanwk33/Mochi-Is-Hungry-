using UnityEngine;
using IndieMarc.TopDown;

public class FoodItem : MonoBehaviour
{
    [Header("Food Settings")]
    [Tooltip("How much hunger this food restores when eaten.")]
    public float hungerRestoreAmount = 5f;

    [Tooltip("How long before the food respawns after being collected.")]
    public float respawnTime = 5f;

    [Tooltip("Tag of the object that can collect the food.")]
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

        if (cannotBeCarried) // option to disable the carryitem script for characters to not carry food items
        {
            CarryItem carry = GetComponent<CarryItem>();
            if (carry != null)
                carry.enabled = false;
        }
    }
    // triggered when a collider enters the object's collider zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        //checking if the collider has a designated collector
        if (other.CompareTag(collectorTag))
        {
            //attempting to access the hungerbar component on the collector
            HungerBar hungerBar = other.GetComponent<HungerBar>();
            if (hungerBar != null)
            {
                // restoring hunger
                hungerBar.AddHunger(hungerRestoreAmount);
                // logging for the console to make sure everything works
                Debug.Log($"{gameObject.name} eaten! Restored {hungerRestoreAmount} hunger.");
            }
            //collection logic
            Collect();

        }
    }
    // handles the collection of food items
    private void Collect()
    {
        //hides the food and disables the collider when its "eaten"
        spriteRenderer.enabled = false;
        itemCollider.enabled = false;
        // scheduling the respawn
        Invoke(nameof(Respawn), respawnTime);
    }

    private void Respawn()
    {
        //resetting the pposition and re-enabling visuals and the collider
        transform.position = originalPosition;
        spriteRenderer.enabled = true;
        itemCollider.enabled = true;
    }
}
