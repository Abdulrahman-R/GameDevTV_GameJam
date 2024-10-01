using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject attackEffect;
    public string projectileType;
    public float speed = 10f;
    public float lifeTime = 2f;
    public int damage;
    private Transform target;
    private Vector2 direction;
    public bool isTracking = false; // Set this to true for tracking projectiles

    TrailRenderer trailRenderer;

    SpriteRenderer gFX;

    private void Start()
    {
        trailRenderer = transform.GetChild(0).transform.GetChild(0).GetComponent<TrailRenderer>();
        gFX = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
        if (target != null && !isTracking)
        {
            direction = (target.position - transform.position).normalized;
        }
        Invoke("Disable", lifeTime); // Disable after a certain time to prevent endless projectiles
    }

    void Update()
    {
        if (isTracking && target != null)
        {
            // Tracking projectile: Move towards the target
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Check if the target is to the right or left of the projectile
            if (target.position.x > transform.position.x)
            {
                // Target is to the right
                gFX.flipX = true;
            }
            else if (target.position.x < transform.position.x)
            {
                // Target is to the left
                gFX.flipX = false;
            }
        }
        else
        {
            // Non-tracking projectile: Move in the set direction
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Assuming the target has the tag "Player"
        {
            collision.gameObject.GetComponent<PlayerStats>().TakeRangeDamage(damage,projectileType);
            if(attackEffect != null)
            {
                Instantiate(attackEffect, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
            }
            
            Disable();
        }
    }

    void Disable()
    {
        if(trailRenderer != null)
        {
            trailRenderer.Clear();
        }
       
        target = null;
        gameObject.SetActive(false);
    }
}
