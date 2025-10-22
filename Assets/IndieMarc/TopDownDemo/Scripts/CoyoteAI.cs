using System.Collections;
using UnityEngine;

public class CoyoteChase : MonoBehaviour
{
    [Header("Chase Settings")]
    public float chaseRange = 15f;
    public float eatDistance = 1.5f;
    public float moveSpeed = 5f;

    private Transform player;
    private HungerBar hungerBar; // Reference to player's hunger

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

        // Chase player
        if (distance <= chaseRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            // If close enough, start eating
            if (distance <= eatDistance && hungerBar != null)
                hungerBar.SetBeingEaten(true);
            else if (hungerBar != null)
                hungerBar.SetBeingEaten(false);
        }
        else if (hungerBar != null)
        {
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
