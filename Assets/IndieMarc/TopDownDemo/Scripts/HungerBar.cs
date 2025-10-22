using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    [Header("Hunger Settings")]
    public float maxHunger = 100f;
    public float hungerDepletionRate = 2f; // per second
    public float hungerLossFromCoyote = 10f; // per second when being eaten

    [Header("References")]
    public Slider hungerSlider; // Assign your UI Slider here

    private float currentHunger;
    private bool isBeingEaten = false;

    void Start()
    {
        currentHunger = maxHunger;
        if (hungerSlider != null)
            hungerSlider.maxValue = maxHunger;
    }

    void Update()
    {
        // Hunger drains over time
        currentHunger -= hungerDepletionRate * Time.deltaTime;

        // Extra drain if being eaten
        if (isBeingEaten)
            currentHunger -= hungerLossFromCoyote * Time.deltaTime;

        // Clamp value
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);

        // Update UI
        if (hungerSlider != null)
            hungerSlider.value = currentHunger;

        // Handle death
        if (currentHunger <= 0)
            Die();
    }

    public void SetBeingEaten(bool eating)
    {
        isBeingEaten = eating;
    }

    void Die()
    {
        Debug.Log("Mochi has died of hunger!");
        // Disable player or trigger death animation
        gameObject.SetActive(false);
    }
    public void AddHunger(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);

        if (hungerSlider != null)
            hungerSlider.value = currentHunger;
    }
    public float GetHungerPercent()
    {
        return currentHunger / maxHunger;
    }


}
