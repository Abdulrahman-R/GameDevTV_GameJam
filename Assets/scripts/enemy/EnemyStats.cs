using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pathfinding;
using UnityEngine.UI;


public enum UnitType
{
    Soldier,
    Swordsman,
    SlimeSoldier,
    SlimeSoldierMinion,
    FireSorcerer,
    IceSorcerer,
    Summoner

}

public class EnemyStats : MonoBehaviour
{

    public UnitType unitType;

    string type;
    [SerializeField] int health;
    [SerializeField] int damage;
    [SerializeField] float attackDistance;
    [SerializeField] GameObject[] slimeSoldierMinions;
    [HideInInspector] public bool canAttack;

    //negative effect configuration
    AIPath aIPath;
    [SerializeField] TextMeshProUGUI timerText;
    private bool iceffectActive = false;
    private bool isStuned = false;
    private float slowEffDuration = 5f; // Default duration of the negative effect in seconds
    private float stunEffDuration = 2f;
    private float timer = 0f;
    float moveSpeed;

    [SerializeField] Image statusImage;
    [SerializeField] Sprite[] statusImages;
    bool statBeingUpdated;
    AIChasingSystem aIChasing;

    Animator gfxAnimator;
    SpriteRenderer gfxRenderer;

    [SerializeField] Sprite[] gFXStats;

   [SerializeField] Transform firPoint;

    [SerializeField] GameObject shard;

    AudioManager audioManager;
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        aIPath = GetComponent<AIPath>();
        moveSpeed = aIPath.maxSpeed;
        type = unitType.ToString();
        canAttack = true;
        Debug.Log(type);
        statBeingUpdated = false;
        aIChasing = GetComponent<AIChasingSystem>();
        gfxAnimator = transform.GetChild(0).GetComponent<Animator>();
        gfxRenderer = transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        if (iceffectActive)
        {
            timer -= Time.deltaTime;
            UpdateTimerText();

            if (timer <= 0)
            {
                EndAllEffects();
            }
        }
        else if (isStuned)
        {
            timer -= Time.deltaTime;
            UpdateTimerText();

            if (timer <= 0)
            {
                EndAllEffects();
            }
        }

        
        if (!aIPath.reachedDestination && aIChasing.aIDestination.target !=null)
        {
            //Debug.Log("happend moving");
            gfxAnimator.SetBool("move", true);
        }
        else
        {
            gfxAnimator.SetBool("move", false);
        }

        

        if (aIChasing.aIDestination.target != null)
        {
            Vector3 targetPosition = aIChasing.aIDestination.target.position;
            Vector3 currentPosition = transform.position;

            // Check if the target is to the right or left
            if (targetPosition.x > currentPosition.x && gfxRenderer.flipX)
            {
                gfxRenderer.flipX = false; // Face right
                firPoint.localPosition = new Vector3(-0.188f, firPoint.localPosition.y, firPoint.localPosition.z);
            }
            else if (targetPosition.x < currentPosition.x && !gfxRenderer.flipX)
            {
                gfxRenderer.flipX = true; // Face left
                firPoint.localPosition = new Vector3(0.188f, firPoint.localPosition.y, firPoint.localPosition.z);
            }
        }

    }
    public int GetDamage()
    {
        return damage;
    }

    public float GetAttackDistance()
    {
        return attackDistance;
    }

    public void TakeDamage()
    {
        switch (type)
        {
            case "Soldier":
                audioManager.PlaySound("attacked");
                Instantiate(shard, new Vector3(transform.position.x, transform.position.y, -11f), Quaternion.identity);
                Destroy(gameObject);
                break;
            case "Swordsman":
                audioManager.PlaySound("attacked");
                Instantiate(shard, new Vector3(transform.position.x, transform.position.y, -11f), Quaternion.identity);
                Destroy(gameObject);
                break;
            case "SlimeSoldier":
                audioManager.PlaySound("attacked");
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                for (int i = 0; i < slimeSoldierMinions.Length; i++)
                {
                    slimeSoldierMinions[i].transform.parent = null;
                    slimeSoldierMinions[i].SetActive(true);
                }
                Destroy(gameObject);

                break;
            case "SlimeSoldierMinion":
                audioManager.PlaySound("attacked");
                Instantiate(shard, new Vector3(transform.position.x, transform.position.y, -11f), Quaternion.identity);
                Destroy(gameObject);
                break;
            case "FireSorcerer":
                audioManager.PlaySound("attacked");
                Instantiate(shard, new Vector3(transform.position.x, transform.position.y, -11f), Quaternion.identity);
                Destroy(gameObject);
                break;
            case "IceSorcerer":
                audioManager.PlaySound("attacked");
                Instantiate(shard, new Vector3(transform.position.x, transform.position.y, -11f), Quaternion.identity);
                Destroy(gameObject);
                break;
            case "Summoner":
                audioManager.PlaySound("attacked");
                Instantiate(shard, new Vector3(transform.position.x, transform.position.y, -11f), Quaternion.identity);
                Destroy(gameObject);
                break;
        }
    }

    public void TakeNegativeEffect(string type)
    {
        audioManager.PlaySound("attacked");
        switch (type)
        {
            case "Ice":
                if (isStuned) return;
                ApplyIceEffect(5f);
                break;
            case "Stun":
                if (isStuned) return;
                EndAllEffects();
                ApplyStunEffect();
                break;
        }
    }



    // This method is called to end all negative effects
    private void EndAllEffects()
    {
        iceffectActive = false;
        isStuned = false;
        canAttack = true;
        timer = 0f;
        aIPath.maxSpeed = moveSpeed;
        aIPath.canMove = true;
        timerText.enabled = false;
        gfxRenderer.sprite = gFXStats[0];

    }

    public void ApplyStunEffect()
    {
        aIPath.canMove = false;
        canAttack = false;
        isStuned = true;
        timer = stunEffDuration;
        timerText.enabled = true;
        gfxRenderer.sprite = gFXStats[2];

        // cameraShake.ShakeIt(0.05f, 0.5f);

        // Ensure the timer does not exceed the effect duration, i don't think it is important at all
        if (timer < 0)
        {
            timer = 0;
        }
    }

    public void ApplyIceEffect(float additionalDuration = 0f)
    {

           // cameraShake.ShakeIt(0.05f, 0.5f);

            aIPath.maxSpeed = (moveSpeed * 0.5f);
            iceffectActive = true;
            timer = slowEffDuration;
            timerText.enabled = true;
            gfxRenderer.sprite = gFXStats[1];

        // Ensure the timer does not exceed the effect duration
        if (timer < 0)
        {
            timer = 0;
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.Max(0, timer).ToString("F1") + "s";
        }
    }


    public void StartUpdatingStatus(int statusIndex)
    {
        StartCoroutine(UpdateStatus(statusIndex));
    }

    IEnumerator UpdateStatus(int statusIndex)
    {
        if(aIChasing.player != null)
        {
            if (!statBeingUpdated)
            {
                statBeingUpdated = true;
                statusImage.sprite = statusImages[statusIndex];
                statusImage.enabled = true;
                yield return new WaitForSeconds(0.5f);

                statBeingUpdated = false;
                statusImage.enabled = false;
            }
            else
            {
                while (statBeingUpdated)
                {
                    yield return null;
                }

                StartUpdatingStatus(statusIndex);
            }
        }
        else
        {
            statBeingUpdated = true;
            statusImage.sprite = statusImages[statusIndex];
            statusImage.enabled = true;
            yield return new WaitForSeconds(0.5f);

            statBeingUpdated = false;
            statusImage.enabled = false;
        }
     
      
    }
}
