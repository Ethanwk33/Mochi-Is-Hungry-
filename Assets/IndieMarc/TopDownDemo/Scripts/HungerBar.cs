using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;   
using IndieMarc.TopDown;

public class HungerBar : MonoBehaviour
{
    [Header("Hunger Settings")]
    public float maxHunger = 100f; // max hunger value mochi can have
    public float hungerDepletionRate = 2f; // passive hunger loss rate
    public float hungerLossFromCoyote = 10f; // hunger loss when the coyote is eating mochi

    [Header("UI References")]
    public Slider hungerSlider; // references UI slider for visual updating
    public TextMeshProUGUI hungerText; // referencecs the hunger text for dynamic updating of hunger amount

    private float currentHunger; // current hunger level
    private bool isEating = false; // indicates if mochi is being eaten
    private bool isDead = false; // flag to prevent multiple death and unwanted looping

    void Start()
    {
        //intializing the hunger to max hunger upon starting
        currentHunger = maxHunger;

        //setting the slider max value
        if (hungerSlider != null)
            hungerSlider.maxValue = maxHunger;
    }

    void Update()
    {
        if (isDead) return; // stopping the logic after death

        // draining the hunger over a cerain time
        currentHunger -= hungerDepletionRate * Time.deltaTime;

        // eating mechanic, draining faster when being eaten
        if (isEating)
            currentHunger -= hungerLossFromCoyote * Time.deltaTime;

        // keeping the hunger to stay between 0 and max
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);

        // updating hunger text and color based upon the current hunger level
        if (hungerSlider != null)
            hungerSlider.value = currentHunger;

        if (hungerText != null)
        {
            hungerText.text = $"Hunger: {Mathf.RoundToInt(currentHunger)}";

            float percent = currentHunger / maxHunger;
            if (percent > 0.6f)
                hungerText.color = Color.green; // hunger is green, meaning mochi is not hungry
            else if (percent > 0.3f)
                hungerText.color = Color.yellow; // hunger is yellow, meaning mochi is hungry
            else
                hungerText.color = Color.red; // hunger is red, meaning mochi is starving and close to death
        }

        // triggering death if the hunger reaches 0
        if (currentHunger <= 0)
            Die();
    }
    // called to notify if the coyote is eating mochi
    public void SetBeingEaten(bool eating)
    {
        isEating = eating;
    }
    //called for when food is collected in order to restore hunger points
    public void AddHunger(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        //updating the UI
        if (hungerSlider != null)
            hungerSlider.value = currentHunger;

        if (hungerText != null)
            hungerText.text = $"Hunger: {Mathf.RoundToInt(currentHunger)}";
    }
    //returns the hunger percentage
    public float GetHungerPercent()
    {
        return currentHunger / maxHunger;
    }
    //player death due to starvation
    void Die()
    {
        if (isDead) return; // prevents multiple calls
        isDead = true;

        Debug.Log("Mochi has died of hunger!");

        //disables movement and controls when death occurs
        var movement = GetComponent<PlayerControls>();
        if (movement != null)
            movement.enabled = false;

        //restarts the scene after 2 seconds
        Invoke(nameof(RestartScene), 2f); 
    }
    //reloads the scene to restart automatically
    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
