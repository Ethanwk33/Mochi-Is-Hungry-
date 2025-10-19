using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CoyoteChase : MonoBehaviour
{
    [Header("Chase Settings")]
    public float chaseRange = 15f;       // Distance at which the coyote starts chasing
    public float eatDistance = 1.5f;     // Distance at which the coyote "eats" the player
    public float eatCooldown = 5f;       // Time before the coyote can eat again

    [Header("NavMesh Settings")]
    public float navMeshSearchRadius = 10f; // How far to search for NavMesh at spawn

    private Transform player;
    private NavMeshAgent agent;
    private float lastEatTime = -Mathf.Infinity;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // Disable agent so Unity doesn't try to initialize it before NavMesh is ready
        if (agent != null)
        {
            agent.enabled = false;
        }
    }

    IEnumerator Start()
    {
        // Wait one frame to allow NavMeshSurface to build (if it's runtime)
        yield return null;

        // Snap to NavMesh if possible before enabling the agent
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, navMeshSearchRadius, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            agent.enabled = true;
        }
        else
        {
            Debug.LogError($"Coyote '{name}' could not find NavMesh within {navMeshSearchRadius} units of spawn.");
            yield break;
        }

        // Find player by tag
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
        // If agent isn't ready or no player, do nothing
        if (player == null || agent == null || !agent.enabled || !agent.isOnNavMesh)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Start chasing if in range
        if (distance <= chaseRange)
        {
            agent.SetDestination(player.position);

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

        // Optional: Add animation, sound, effects, game over, etc.
    }

    void OnDrawGizmosSelected()
    {
        // Visualize chase and eat ranges in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, eatDistance);
    }
}
