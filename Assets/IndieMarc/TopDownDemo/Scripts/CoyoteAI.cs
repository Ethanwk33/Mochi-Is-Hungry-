using UnityEngine;

public class CoyoteChase : MonoBehaviour
{
    [Header("Chase Settings")]
    public float chaseRange = 15f;
    public float eatDistance = 1.5f;

    [Header("Speed Settings")]
    public float maxSpeed = 8f;   // Speed when Mochi is full
    public float minSpeed = 2f;   // Speed when Mochi is starving

    private Transform player;
    private HungerBar hungerBar;
    private float currentSpeed;
    private bool isEating = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            hungerBar = playerObj.GetComponent<HungerBar>();

            if (hungerBar == null)
                Debug.LogError("HungerBar not found on Player!");
            else
                Debug.Log("HungerBar found on Player!");
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Player' found in the scene.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Adjust coyote speed dynamically based on Mochi’s hunger
        float hungerPercent = (hungerBar != null) ? hungerBar.GetHungerPercent() : 1f;
        currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, hungerPercent);

        // Only chase if within range
        if (distance <= chaseRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * currentSpeed * Time.deltaTime;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }

        // Distance-based eating logic
        isEating = (distance <= eatDistance);

        // Update HungerBar
        if (hungerBar != null)
            hungerBar.SetBeingEaten(isEating);

        //Debug.Log($"Coyote isEating: {isEating}");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, eatDistance);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HungerBar hunger = other.GetComponent<HungerBar>();
            if (hunger != null)
                hunger.SetBeingEaten(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HungerBar hunger = other.GetComponent<HungerBar>();
            if (hunger != null)
                hunger.SetBeingEaten(false);
        }
    }

}
