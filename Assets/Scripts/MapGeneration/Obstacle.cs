using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float obstacleHealth;
    public float maxObstacleHealth;
    public Sprite[] sprites; // Damage progression
    public RoomGenerator roomGenerator;
    public int x;
    public int y;
    void Start()
    {

    }
    public void ObstacleDamage(float amount)
    {
        obstacleHealth -= amount;
        UpdateSprite();
    }

    void UpdateSprite()
    {
        if (sprites.Length > 0)
        {
            int index = Mathf.Clamp(Mathf.FloorToInt((1 - (obstacleHealth / maxObstacleHealth)) * sprites.Length), 0, sprites.Length - 1);
            GetComponent<SpriteRenderer>().sprite = sprites[index];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (obstacleHealth <= 0)
        {
            DestroyObstacle();
        }
    }
    void DestroyObstacle()
    {
        if (roomGenerator != null) roomGenerator.MakeWalkable(x, y);
        Destroy(gameObject);
    }
}
