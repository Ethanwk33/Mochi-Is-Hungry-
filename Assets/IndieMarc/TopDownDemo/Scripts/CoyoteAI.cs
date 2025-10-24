using UnityEngine;

public class CoyoteChase : MonoBehaviour
{
    [Header("Chase Settings")]
    public float chaseRange = 15f; // distance for when the coyote starts chasing
    public float eatDistance = 1.5f; // distance for eating activation

    [Header("Speed Settings")]
    public float maxSpeed = 8f;   // coyote speed when the player is full ( max hunger)
    public float minSpeed = 2f;   // coyote speed when the player is starving

    private Transform player; // player designation
    private HungerBar hungerBar;  // player hungerbar reference
    private float currentSpeed; // coyote's current movement speed
    private bool isEating = false; // flag to enable and disable eating mechanic

    void Start()
    {
        //finds the player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            //caching the player and hungerbar references
            player = playerObj.transform;
            hungerBar = playerObj.GetComponent<HungerBar>();
            // validates the presence of a hungerbar
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
        //calculates the distance to the player
        float distance = Vector3.Distance(transform.position, player.position);

        // adjusts the coyote speed based upon mochi's hunger level
        float hungerPercent = (hungerBar != null) ? hungerBar.GetHungerPercent() : 1f;
        currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, hungerPercent);

        // only chases if within a specified range
        if (distance <= chaseRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            //moves the coyote toward the player based upon its calculated direction and speed
            transform.position += direction * currentSpeed * Time.deltaTime;
            //rotates towards the player
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }

        // determines if the coyote is close enough to eat
        isEating = (distance <= eatDistance);

        // updates the hungerbar
        if (hungerBar != null)
            hungerBar.SetBeingEaten(isEating);

        //Debug.Log($"Coyote isEating: {isEating}");
    }
    //visualizing the chase and eat ranges in the scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange); // chase range indicator

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, eatDistance); // eat range indicator
    }
    // triggers the eating logic
    private void OnTriggerEnter2D(Collider2D other)
    {
        //checks if tagged as player, if it is, toggled setbeingeaten 
        if (other.CompareTag("Player"))
        {
            HungerBar hunger = other.GetComponent<HungerBar>(); 
            if (hunger != null)
                hunger.SetBeingEaten(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //checks if tagged as player, if it is, toggled setbeingeaten 
        if (other.CompareTag("Player"))
        {
            HungerBar hunger = other.GetComponent<HungerBar>();
            if (hunger != null)
                hunger.SetBeingEaten(false);
        }
    }

}
