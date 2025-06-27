using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5f;
    public EnemyStats stats;
    [SerializeField] protected Animator animator = null;
    public bool isFacingRight = false;
    public bool canMove = true;
    public bool goBackwards = false;
    [SerializeField] private RoomGenerator roomGenerator;

    private List<PathfindingNode> currentPath;
    private int currentPathIndex = 0;
    private Vector2Int lastPlayerGridPos;
    public float repathThreshold = 0.1f;
    public float colliderMargin = 0.9f;
    public float standoffDistance = 0f;
    protected Rigidbody2D rb;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        roomGenerator = FindFirstObjectByType<RoomGenerator>();
        rb = GetComponent<Rigidbody2D>();

        if (GetComponent<BoxCollider2D>() != null)
        {
            Debug.Log("has box collider");
            ResizeBoxCollider();
        }

        if (GetComponent<CircleCollider2D>() != null)
        {
            ResizeCircleCollider();
        }
    }
    float GetCellSize() {
        return 1f / roomGenerator.gridResolution;
    }
    void ResizeBoxCollider()
    {
        BoxCollider2D boxCol = findCollisionCollider();
        if (boxCol != null)
        {
            float cellSize = GetCellSize();
            boxCol.size = new Vector2(cellSize * colliderMargin, cellSize * colliderMargin);
        }
        else if (boxCol == null)
        {
            Debug.LogError("Collision collider not found!");
        }
    }
    void ResizeCircleCollider()
    {
        CircleCollider2D cirCol = findCircleCollisionCollider();
        if (cirCol != null)
        {
            float cellSize = GetCellSize();
            cirCol.radius = cellSize * colliderMargin;
        }
        else if (cirCol == null)
        {
            Debug.LogError("Collision collider not found!");
        }
    }
    BoxCollider2D findCollisionCollider()
    {
        BoxCollider2D boxCollider = null;
        foreach (var col in GetComponents<BoxCollider2D>())
        {
            if (!col.isTrigger)
            {
                boxCollider = col;
                break;
            }
        }
        
        return boxCollider;
    }

    CircleCollider2D findCircleCollisionCollider()
    {
        CircleCollider2D circleCollider = null;
        foreach (var col in GetComponents<CircleCollider2D>())
        {
            if (!col.isTrigger)
            {
                circleCollider = col;
                break;
            }
        }
        return circleCollider;
    }
    public bool CanSeePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        Vector2 rayOrigin = (Vector2)transform.position + new Vector2(0f, 0.1f);
        LayerMask obstacleMask = LayerMask.GetMask("Obstacle");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleMask);

        Debug.DrawRay(rayOrigin, direction * distance, hit.collider == null ? Color.green : Color.red);
        return hit.collider == null;
    }
    /*
    void OnDrawGizmos()
    {
        if (Application.isPlaying && player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, player.position);
            Vector2 rayOrigin = (Vector2)transform.position + new Vector2(0f, 0.1f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(rayOrigin, rayOrigin + direction * distance);
        }
    }
    */
    protected bool InRange()
    {
        return (GetDistance() <= detectionRadius || (GetDistance() >= detectionRadius && stats.wasHitByPlayer)) && canMove;
    }
    protected float GetDistance() {
        return Vector2.Distance(transform.position, player.position);
    }
    public virtual void UpdateMovement()
    {
        if (player == null) return;

        if (InRange())
        {
            if (CanSeePlayer())
            {
                BeelineTowardsPlayer();
            }
            else
            {
                Pathfind();
                animator.SetBool("isRunning", true);
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
            currentPath = null;
        }
    }
    protected void Pathfind()
    {
        Vector2Int currentPlayerGridPos = GetNearestWalkableCell(player.position);

        // Check if need to recalculate path
        if (currentPath == null || currentPath.Count == 0 || Vector2Int.Distance(currentPlayerGridPos, lastPlayerGridPos) > repathThreshold)
        {
            Vector2Int startPos = WorldToGridPosition(transform.position);
            if (!IsWalkable(startPos)) return;
            currentPath = FindPath(startPos, currentPlayerGridPos);
            currentPathIndex = 0;
            lastPlayerGridPos = currentPlayerGridPos;
        }

        MoveAlongPath();
    }
    protected Vector3 Direction()
    {
        return (player.position - transform.position).normalized;
    }
    protected void BeelineTowardsPlayer()
    {
        transform.position += Direction() * stats.getMovementSpeed() * Time.deltaTime;

        HandleSpriteDirection(Direction());
        animator.SetBool("isRunning", true);
    }

    protected Vector2Int WorldToGridPosition(Vector3 worldPos) {
        int x = Mathf.FloorToInt(worldPos.x * roomGenerator.gridResolution);
        int y = Mathf.FloorToInt(worldPos.y * roomGenerator.gridResolution);
        return new Vector2Int(x, y);
    }
    public List<PathfindingNode> FindPath(Vector2Int startPos, Vector2Int targetPos)
    {
        var grid = roomGenerator.grid;

        foreach (var node in grid)
        {
            node.startCost = float.MaxValue;
            node.hCost = 0;
            node.parent = null;
        }

        PathfindingNode startNode = grid[startPos.x, startPos.y];
        PathfindingNode targetNode = grid[targetPos.x, targetPos.y];

        List<PathfindingNode> openSet = new() { startNode };
        HashSet<PathfindingNode> closedSet = new();

        startNode.startCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);
        startNode.parent = null;

        while (openSet.Count > 0)
        {
            PathfindingNode currentNode = openSet.OrderBy(n => n.startCost + n.hCost).First();

            if (currentNode == targetNode) return RetracePath(startNode, targetNode);

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            foreach (PathfindingNode neighbor in GetNeighbors(currentNode, grid))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;

                float tempStart = currentNode.startCost + GetDistance(currentNode, neighbor);
                if (tempStart < neighbor.startCost || !openSet.Contains(neighbor))
                {
                    neighbor.startCost = tempStart;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }
        return null;
    }
    Vector2Int GetNearestWalkableCell(Vector3 worldPos)
    {
        Vector2Int gridPos = WorldToGridPosition(worldPos);
        if (IsWalkable(gridPos)) return gridPos;

        // Simple fallback: search 3x3 neighboring subcells (you can expand this)
        int[] dx = { 0, 1, -1, 0, 0, 1, 1, -1, -1 };
        int[] dy = { 0, 0, 0, 1, -1, 1, -1, 1, -1 };

        for (int i = 0; i < dx.Length; i++)
        {
            Vector2Int neighbor = new Vector2Int(gridPos.x + dx[i], gridPos.y + dy[i]);
            if (IsWalkable(neighbor)) return neighbor;
        }

        // As last resort, just return original snapped position
        return gridPos;
    }

    protected bool IsWalkable(Vector2Int gridPos)
    {
        if (gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= roomGenerator.grid.GetLength(0) || gridPos.y >= roomGenerator.grid.GetLength(1))
            return false;

        return roomGenerator.grid[gridPos.x, gridPos.y].walkable;
    }
    protected void MoveAlongPath()
    {
        if (currentPath == null || currentPathIndex >= currentPath.Count) return;

        Vector3 targetWorldPos = new Vector3((currentPath[currentPathIndex].x + 0.5f) / roomGenerator.gridResolution, (currentPath[currentPathIndex].y + 0.5f) / roomGenerator.gridResolution, 0);
        Vector3 direction = (targetWorldPos - transform.position).normalized;
        
        //if near player, hone in on exact location
        if (currentPathIndex == currentPath.Count - 1)
        {
            targetWorldPos = new Vector3(player.position.x, player.position.y, 0);
        }
        // Move towards current path node
        transform.position += direction * stats.getMovementSpeed() * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetWorldPos) < 0.1f)
        {
            currentPathIndex++;
        }

        HandleSpriteDirection(direction);
    }

    protected void HandleSpriteDirection(Vector3 direction)
    {
        if (!goBackwards)
        {
            float flip = (isFacingRight ? 1 : -1);
            transform.localScale = new Vector3(
                (direction.x > 0 ? 1 : -1) * flip,
                1,
                1
            );
        }
        else
        {
            float flip = (isFacingRight ? 1 : -1);
            transform.localScale = new Vector3(
                (direction.x > 0 ? 1 : -1) * flip,
                1,
                1
            );
        }
    }
    float GetDistance(PathfindingNode a, PathfindingNode b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        int straight = Mathf.Abs(dx - dy);
        int diagonal = Mathf.Min(dx, dy);

        return 1.4f * diagonal + 1f * straight;
    }

    List<PathfindingNode> RetracePath(PathfindingNode start, PathfindingNode end)
    {
        List<PathfindingNode> path = new();
        PathfindingNode current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }

    List<PathfindingNode> GetNeighbors(PathfindingNode node, PathfindingNode[,] grid)
    {
        List<PathfindingNode> neighbors = new();
        int[] dx = { -1, 1, 0, 0, -1, -1, 1, 1 };
        int[] dy = { 0, 0, -1, 1, -1, 1, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int nx = node.x + dx[i];
            int ny = node.y + dy[i];
            if (nx >= 0 && ny >= 0 && nx < grid.GetLength(0) && ny < grid.GetLength(1))
            {
                neighbors.Add(grid[nx, ny]);
            }
        }
        return neighbors;
    }

/*
    void MoveAlongPath(List<PathfindingNode> path)
    {
        Vector3 targetWorldPos = new Vector3(path[0].x+0.5f, path[0].y+0.5f, 0);
        Vector3 direction = (targetWorldPos - transform.position).normalized;


        // some of the sprites are facing different directions
        if (!goBackwards)
        {
            if (isFacingRight)
            {
                if (direction.x > 0) transform.localScale = new Vector3(1, 1, 1);
                else if (direction.x < 0) transform.localScale = new Vector3(-1, 1, 1);
                transform.position += direction * stats.getMovementSpeed() * Time.deltaTime;
            }
            else if (!isFacingRight)
            {
                if (direction.x > 0) transform.localScale = new Vector3(-1, 1, 1);
                else if (direction.x < 0) transform.localScale = new Vector3(1, 1, 1);
                transform.position += direction * stats.getMovementSpeed() * Time.deltaTime;
            }
        }
        else
        {
            if (isFacingRight)
            {
                if (direction.x > 0) transform.localScale = new Vector3(1, 1, 1);
                else if (direction.x < 0) transform.localScale = new Vector3(-1, 1, 1);
                transform.position += -direction * stats.getMovementSpeed() * Time.deltaTime;
            }
            else if (!isFacingRight)
            {
                if (direction.x > 0) transform.localScale = new Vector3(-1, 1, 1);
                else if (direction.x < 0) transform.localScale = new Vector3(1, 1, 1);
                transform.position += -direction * stats.getMovementSpeed() * Time.deltaTime;
            }
        }
    }
*/
}
