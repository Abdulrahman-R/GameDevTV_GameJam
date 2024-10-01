using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class EnemyCombat : MonoBehaviour
{
    [SerializeField] bool isRanger;
    public float attackSpeed = 2f;
    private float attackCooldown = 0f;
    [SerializeField] float attackDelay;


  // [SerializeField] SpriteRenderer renderer;
    EnemyStats myStats;
    ShootingSystem shootingSystem;

    Animator gFXanimator;
    SpriteRenderer gFXRenderer;

    AudioManager audioManager;
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();

        myStats = GetComponent<EnemyStats>();
        shootingSystem = GetComponent<ShootingSystem>();

        gFXanimator = transform.GetChild(0).transform.GetComponent<Animator>();
        gFXRenderer = transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;
    }
    public void Attack (PlayerStats playerStats)
    {
        if (attackCooldown <= 0f)
        {
            if (!isRanger)
            {
                StartCoroutine(DoMeleDamage(playerStats, attackDelay));
            }
            else
            {
                StartCoroutine(DoRangeDamage(playerStats, attackDelay));
            }
            
            attackCooldown = 1f / attackSpeed;
        }
        
    }

    IEnumerator DoMeleDamage (PlayerStats playerStats, float delay)
    {
        //  Color currColor = renderer.color;
        //  renderer.color = Color.red;
        //here you do the attack animation if any
        gFXanimator.SetTrigger("attack");
        audioManager.PlaySound("attack");
        yield return new WaitForSeconds(delay);
        // you need to duble check the distance here to give time to the player to escape 
        if(playerStats != null) // double check that the player is still alive
        {
            if (Vector2.Distance(transform.position, playerStats.transform.position) <= myStats.GetAttackDistance())
            {
                playerStats.TakeMeleDamage(myStats.GetDamage());
            }
        }
       
       // renderer.color = currColor;
    }

    IEnumerator DoRangeDamage(PlayerStats playerStats, float delay)
    {
        // Color currColor = renderer.color;
        // renderer.color = Color.red;
        //here you do the attack animation if any

        
        gFXanimator.SetTrigger("attack");
        audioManager.PlaySound("attack");
        yield return new WaitForSeconds(delay);

        if(playerStats != null)
        {
            shootingSystem.Shoot(playerStats);
        }
       // renderer.color = currColor;
    }
}
