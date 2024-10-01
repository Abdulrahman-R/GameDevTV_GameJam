using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIChasingSystem : MonoBehaviour
{
    EnemyCombat myCombat;
    EnemyStats myStats;
    [HideInInspector]public AIDestinationSetter aIDestination;
    AIPath aIPath;

    [HideInInspector]public GameObject player;
    public Transform[] patrolPoints; // Array of patrol points
    [SerializeField] GameObject patrolPointsHolder;
    [SerializeField] float chasingDistance = 0.1f;
    float playerStopDistance;
    float patrollingDistance = 0.8f;

    private int currentPointIndex = 0;
    private Transform targetPoint;
    [HideInInspector] public Transform currTraget;
    private bool isReversing = false;

    FOVController fOVController;
    [HideInInspector] public AIInformingSystem aIInformingSystem;

    AudioManager audioManager;
   // SpriteRenderer gfxRenderer;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        myCombat = GetComponent<EnemyCombat>();
        myStats = GetComponent<EnemyStats>();
        fOVController = GetComponent<FOVController>();
        patrolPointsHolder.transform.parent = null;
        aIDestination = GetComponent<AIDestinationSetter>();
        aIInformingSystem = transform.GetChild(4).gameObject.GetComponent<AIInformingSystem>();// it gonna be 4 because the potrol points.parent = null on run time
        aIPath = GetComponent<AIPath>();
        player = null;
        playerStopDistance = aIPath.endReachedDistance;
      //  gfxRenderer = transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>();
        SetNextPoint();
    }

    void Update()
    {
        ChasePlayer();
        Patrol();
    }


    void Patrol()
    {
        if (player != null) return;
        if (targetPoint == null)
            return;

        aIPath.endReachedDistance = patrollingDistance;
        aIDestination.target = targetPoint;

        fOVController.UpdateTarget(targetPoint);

        // Check if the object has reached the target point
        if (Vector2.Distance(transform.position, targetPoint.position) <= patrollingDistance)
        {
            SetNextPoint();
        }
    }

    // may need refaining 
    void SetNextPoint()
    {
        if (patrolPoints.Length == 0 || patrolPoints.Length == 1)// may cause mistakes
            return;

        // Set the next patrol point
        targetPoint = patrolPoints[currentPointIndex];

        // Update the index for the next point
        if (isReversing)
        {
            currentPointIndex--;
            if (currentPointIndex < 0)
            {
                currentPointIndex = 1;
                isReversing = false;
            }
        }
        else
        {
            currentPointIndex++;
            if (currentPointIndex >= patrolPoints.Length)
            {
                currentPointIndex = patrolPoints.Length - 2;
                isReversing = true;
            }
        }
    }

   

    void ChasePlayer()
    {
        if (player == null) return;

        aIPath.endReachedDistance = playerStopDistance;

        //remeber order matters
        if (Vector2.Distance(transform.position, player.transform.position) <= myStats.GetAttackDistance())
        {
            if (player != null)
            {
                if(myStats.canAttack)
                myCombat.Attack(player.GetComponent<PlayerStats>());
            }
                
        }

        if (Vector2.Distance(transform.position, player.transform.position) >= chasingDistance)
        {
            player.GetComponent<PlayerStats>().UpdateChasingCounter(-1);
            fOVController.fovPivot.transform.GetChild(0).gameObject.SetActive(true);
            aIInformingSystem.gameObject.SetActive(false);
            player = null;
            aIDestination.target = null;
            myStats.StartUpdatingStatus(2);
            SetNextPoint();
        }
    }

    public void ForceChasing(GameObject player)
    {
        if (this.player != null) return;
        audioManager.PlaySound("huh");
        this.player = player;
        aIDestination.target = player.transform;
        player.GetComponent<PlayerStats>().UpdateChasingCounter(1);
        fOVController.fovPivot.transform.GetChild(0).gameObject.SetActive(false);
        aIInformingSystem.gameObject.SetActive(true);
        myStats.StartUpdatingStatus(0);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ForceChasing(collision.gameObject);
        }
    }
}