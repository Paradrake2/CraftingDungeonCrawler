using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5f;
    public EnemyStats stats;
    [SerializeField] private Animator animator = null;
    public bool isFacingRight = false;
    public bool canMove = true;
    public bool goBackwards = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;
        
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRadius && canMove)
        {
            MoveTowardsPlayer();
            animator.SetBool("isRunning", true);
        }
        else if (distance >= detectionRadius && stats.wasHitByPlayer == true && canMove)
        {
            MoveTowardsPlayer();
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }
    public void MoveTowardsPlayer() {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector2 directionS = player.position - transform.position;
        if (!goBackwards)
        {
            // some of the sprites are facing different directions
            if (isFacingRight)
            {
                if (directionS.x > 0) transform.localScale = new Vector3(1, 1, 1);
                else if (directionS.x < 0) transform.localScale = new Vector3(-1, 1, 1);
                transform.position += direction * stats.getMovementSpeed() * Time.deltaTime;
            }
            else if (!isFacingRight)
            {
                if (directionS.x > 0) transform.localScale = new Vector3(-1, 1, 1);
                else if (directionS.x < 0) transform.localScale = new Vector3(1, 1, 1);
                transform.position += direction * stats.getMovementSpeed() * Time.deltaTime;
            }
        }
        else
        {
            if (isFacingRight)
            {
                if (directionS.x > 0) transform.localScale = new Vector3(1, 1, 1);
                else if (directionS.x < 0) transform.localScale = new Vector3(-1, 1, 1);
                transform.position += -direction * stats.getMovementSpeed() * Time.deltaTime;
            }
            else if (!isFacingRight)
            {
                if (directionS.x > 0) transform.localScale = new Vector3(-1, 1, 1);
                else if (directionS.x < 0) transform.localScale = new Vector3(1, 1, 1);
                transform.position += -direction * stats.getMovementSpeed() * Time.deltaTime;
            }
        }
        
    }
}
