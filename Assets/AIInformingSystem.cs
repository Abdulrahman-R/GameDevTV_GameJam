using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInformingSystem : MonoBehaviour
{
    AIChasingSystem aIChasingSystem;
    EnemyStats myStats;

    private void Start()
    {
        aIChasingSystem = transform.parent.GetComponent<AIChasingSystem>();
        myStats = transform.parent.GetComponent<EnemyStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision) // if there us any problem try onTriggerStay2D :)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.GetComponent<AIChasingSystem>().player == null && aIChasingSystem.player != null)
            {
                myStats.StartUpdatingStatus(1);
                collision.GetComponent<AIChasingSystem>().ForceChasing(aIChasingSystem.player);
            }
        }
    }
}
