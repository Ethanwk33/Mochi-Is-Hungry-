using System.Collections;
using UnityEngine;

public class CoyoteChase : MonoBehaviour
{
    [Header("Chase Settings")]
    public float chaseRange = 15f;       // Distance at which the coyote starts chasing
    public float eatDistance = 1.5f;     // Distance at which the coyote "eats" the player
    public float eatCooldown = 5f;       // Time before the coyote can eat again
    public float moveSpeed = 5f;         // Movement speed of the coyote

    private Transform player;
    private float lastEatTime = -Mathf.Infinity;

    void Start()
    {
        lastEatTime = Time.time; // Prevent immediate eating

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
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

        // Start chasing if in range
        if (distance <= chaseRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;

            // Move toward the player
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Rotate to face the player if direction is valid
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            // If close enough, "eat" the player
            if (distance <= eatDistance && Time.time - lastEatTime >= eatCooldown)
            {
                EatPlayer();
                lastEatTime = Time.time;
            }
        }
    }

    void EatPlayer()
    {
        Debug.Log("Coyote has eaten the player!");
        player.gameObject.SetActive(false);

   
    }

    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, eatDistance);
    }
}
