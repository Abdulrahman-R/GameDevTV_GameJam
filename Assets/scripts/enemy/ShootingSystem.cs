using UnityEngine;


public enum ProjectileType
{
    Fire,
    Ice,
    Stun

}
public class ShootingSystem : MonoBehaviour
{
    public ProjectileType projectileType;
    ProjectilePool projectilePool;
    [SerializeField] Transform firePoint;
    [SerializeField] bool useTrackingProjectiles = false;
    [SerializeField] float speed = 10f;
    [SerializeField] float lifeTime = 2f;
    [SerializeField] int damage;

    private void Start()
    {
        projectilePool = GetComponent<ProjectilePool>();
    }
   public void Shoot(PlayerStats playerStats)
    {
        GameObject projectile = projectilePool.GetPooledObject();
        if (projectile != null)
        {
            projectile.transform.position = firePoint.position;
            projectile.transform.rotation = firePoint.rotation;
            projectile.SetActive(true);

            // Set the projectile type
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.isTracking = useTrackingProjectiles;
            projectileScript.lifeTime = lifeTime;
            projectileScript.speed = speed;
            projectileScript.damage = damage;
            projectileScript.projectileType = projectileType.ToString();
            // Find a target (for simplicity, assume target is the player; adjust as needed)
            GameObject target = playerStats.gameObject;
            if (target != null)
            {
                projectileScript.SetTarget(target.transform);
            }
        }
    }
}