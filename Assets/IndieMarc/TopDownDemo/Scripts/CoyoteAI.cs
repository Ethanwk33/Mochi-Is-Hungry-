using UnityEngine;

public class CoyoteChase : MonoBehaviour
{
    [Header("Chase Settings")]
    public float chaseRange = 15f;
    public float eatDistance = 1.5f;

    [Header("Speed Settings")]
    public float maxSpeed = 8f;   
    public float minSpeed = 2f;   

    private Transform player;
    private HungerBar hungerBar;
    private float currentSpeed;
    private bool isEating = false;

    void Start()
    {
        //finds the player by tag
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
        //calculates the distance to the player
        float distance = Vector3.Distance(transform.position, player.position);

        // adjusts the coyote speed based upon mochi's hunger level
        float hungerPercent = (hungerBar != null) ? hungerBar.GetHungerPercent() : 1f;
        currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, hungerPercent);

        // only chases if within a specified range
        if (distance <= chaseRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
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
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, eatDistance);
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
