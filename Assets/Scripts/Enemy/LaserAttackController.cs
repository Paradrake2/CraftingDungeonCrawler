using System.Collections;
using UnityEngine;

public class LaserAttackController : MonoBehaviour
{
    public float telegraphDuration = 0.5f;
    public float beamDuration = 0.3f;
    public float beamDamage;
    public Transform player;
    public Vector3 pos;
    public System.Action onBeamFinished;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    public void Initialize(float damage, GameObject previewBeamPrefab, GameObject damageBeamPrefab, Transform playerT, Vector3 position)
    {
        beamDamage = damage;
        player = playerT;
        pos = position;
        FireLaser(previewBeamPrefab, damageBeamPrefab);
    }

    public void FireLaser(GameObject previewBeamPrefab, GameObject damageBeamPrefab)
    {
        StartCoroutine(FireLaserRoutine(previewBeamPrefab, damageBeamPrefab));
    }
    IEnumerator FireLaserRoutine(GameObject previewBeamPrefab, GameObject damageBeamPrefab)
    {
        Vector2 targetPos = player.position;
        Vector2 startPos = pos;
        Vector2 direction = (targetPos - startPos).normalized;

        // Create preview line
        GameObject preview = Instantiate(previewBeamPrefab, startPos, Quaternion.identity);
        SetBeamDirection(preview, direction);

        yield return new WaitForSeconds(telegraphDuration);
        Destroy(preview);

        GameObject beam = Instantiate(damageBeamPrefab, startPos, Quaternion.identity);
        SetBeamDirection(beam, direction);

        yield return new WaitForSeconds(beamDuration);
        Destroy(beam);
        onBeamFinished?.Invoke();
    }

    void SetBeamDirection(GameObject beam, Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        beam.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>()?.TakeDamage(beamDamage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
