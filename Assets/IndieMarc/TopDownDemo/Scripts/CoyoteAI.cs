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
    private HungerBar hungerBar; // Reference to Mochi’s hunger
    private float currentSpeed;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            hungerBar = playerObj.GetComponent<HungerBar>();
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

        // Adjust speed dynamically based on hunger percentage
        float hungerPercent = (hungerBar != null) ? hungerBar.GetHungerPercent() : 1f;
        currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, hungerPercent);

        // Start chasing
        if (distance <= chaseRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * currentSpeed * Time.deltaTime;

            // Rotate smoothly toward player
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
            if (hungerBar != null)
            {
                if (distance <= eatDistance)
                    hungerBar.SetBeingEaten(true);
                else
                    hungerBar.SetBeingEaten(false);
            }
        }
        else
        {
            // Stop eating when player is out of range
            if (hungerBar != null)
                hungerBar.SetBeingEaten(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, eatDistance);
    }
}
