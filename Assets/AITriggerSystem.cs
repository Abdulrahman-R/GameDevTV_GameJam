using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITriggerSystem : MonoBehaviour
{
    AIChasingSystem aIChasingSystem;
    EnemyStats myStats;
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        aIChasingSystem = transform.parent.transform.parent.GetComponent<AIChasingSystem>();
        myStats = transform.parent.transform.parent.GetComponent<EnemyStats>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!collision.GetComponent<PlayerStats>().GetHidden())
            {
                audioManager.PlaySound("huh");
                aIChasingSystem.player = collision.gameObject;
                aIChasingSystem.aIDestination.target = aIChasingSystem.player.transform;
                collision.gameObject.GetComponent<PlayerStats>().UpdateChasingCounter(1);
                aIChasingSystem.aIInformingSystem.gameObject.SetActive(true);
                myStats.StartUpdatingStatus(0);
                gameObject.SetActive(false);
            }

        }
    }
}
