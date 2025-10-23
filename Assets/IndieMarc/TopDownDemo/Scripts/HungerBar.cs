using UnityEngine;
using UnityEngine.UI;
using TMPro;
using IndieMarc.TopDown;

public class HungerBar : MonoBehaviour
{
    [Header("Hunger Settings")]
    public float maxHunger = 100f;
    public float hungerDepletionRate = 2f;
    public float hungerLossFromCoyote = 10f;

    [Header("UI References")]
    public Slider hungerSlider;
    public TextMeshProUGUI hungerText; 

    private float currentHunger;
    private bool isEating = false;
    public static HungerBar instance;
    void Start()
    {
        currentHunger = maxHunger;

        if (hungerSlider != null)
            hungerSlider.maxValue = maxHunger;
    }

    void Update()
    {
        // Drain hunger over time
        currentHunger -= hungerDepletionRate * Time.deltaTime;

        // Drain faster when being eaten
        if (isEating)
            currentHunger -= hungerLossFromCoyote * Time.deltaTime;

        // Clamp to valid range
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);

        // Update UI
        if (hungerSlider != null)
            hungerSlider.value = currentHunger;

        if (hungerText != null)
        {
            hungerText.text = $"Hunger: {Mathf.RoundToInt(currentHunger)}";

            float percent = currentHunger / maxHunger;
            if (percent > 0.6f)
                hungerText.color = Color.green;
            else if (percent > 0.3f)
                hungerText.color = Color.yellow;
            else
                hungerText.color = Color.red;
        }

        // Death condition
        if (currentHunger <= 0)
            Die();
    }

    public void SetBeingEaten(bool eating)
    {
        isEating = eating;
    }

    public void AddHunger(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);

        if (hungerSlider != null)
            hungerSlider.value = currentHunger;

        if (hungerText != null)
            hungerText.text = $"Hunger: {Mathf.RoundToInt(currentHunger)}";
    }

    public float GetHungerPercent()
    {
        return currentHunger / maxHunger;
    }

    void Die()
    {
        Debug.Log("Mochi has died of hunger!");

        // Disable movement or control scripts instead of the whole GameObject
        var movement = GetComponent<PlayerControls>();
        if (movement != null)
            movement.enabled = false;

        // Optionally trigger a death animation or game over screen
    }
}
