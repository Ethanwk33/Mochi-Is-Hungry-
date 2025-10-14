using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class CoyoteAI : MonoBehaviour
{
    public Transform player;         
    public float moveSpeed = 3f;     

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogError("CoyoteAI: No Player found with tag 'Player'");
        }

        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 targetPos = rb.position + direction * moveSpeed * Time.fixedDeltaTime;


        rb.MovePosition(targetPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Coyote caught the cat!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
