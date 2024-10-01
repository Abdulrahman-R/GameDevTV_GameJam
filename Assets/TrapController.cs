using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TrapType
{
    Spikes,
    Fire,
    Ice,
    Poison,
    Stun,
    StageSpike
}

public enum TrapMode
{
    SingleTarget,
    MultipleTargets
}

[System.Serializable]
public class TrapData
{
    public string trapType;
    public int targetNum;
    public GameObject particleEffect;
    public string[] trapTargets;
}
public class TrapController : MonoBehaviour
{
    TrapsManager trapsManager;
    public TrapData[] traps;

    public TrapType trapType;

    public TrapMode trapMode;

    [SerializeField] float duration = 3f; // Duration of the timer in seconds
    private bool isRunning = false;
    private float timeRemaining;
    [SerializeField] TextMeshProUGUI timerText; // Reference to the TextMeshPro text component


    private void Start()
    {
        trapsManager = FindObjectOfType<TrapsManager>();
        if (trapMode == TrapMode.MultipleTargets)
        {
            StartTimer(duration);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy") || collision.CompareTag("Player"))
        {
            if(trapMode == TrapMode.SingleTarget)
            {
                DoSingleDamage(collision.gameObject, collision.tag);

            }
        }
    }


    void DoSingleDamage(GameObject target,string tag)
    {
        string trapType = this.trapType.ToString();

        if(tag == "Enemy")
        {
            string targetType = target.GetComponent<EnemyStats>().unitType.ToString();
            switch (trapType)
            {
                case "Spikes":
                    if (CheckTarget(targetType, traps[0].trapTargets))
                    {
                        target.GetComponent<EnemyStats>().TakeDamage();
                        //you can make a partical effect
                        Instantiate(traps[0].particleEffect, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
                        trapsManager.DecreaseScore(1);
                        Destroy(gameObject);
                    }
                    else
                    {
                        //you can make a partical effect
                        Instantiate(traps[0].particleEffect, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
                        trapsManager.DecreaseScore(1);
                        Destroy(gameObject);
                    }
  
                    break;
                case "Fire":
                    if (CheckTarget(targetType, traps[1].trapTargets))
                    {
                        Instantiate(traps[1].particleEffect, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
                        target.GetComponent<EnemyStats>().TakeDamage();
                        //you can make a partical effect
                        trapsManager.DecreaseScore(1);
                        Destroy(gameObject);
                    }
                    else if (targetType == "FireSorcerer")
                    {
                        Instantiate(traps[1].particleEffect, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
                        trapsManager.DecreaseScore(1);
                        Destroy(gameObject);
                    }
                    else
                    {
                        Instantiate(traps[1].particleEffect, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
                        trapsManager.DecreaseScore(1);
                        Destroy(gameObject);
                    }
                    break;

                case "Ice":
                    Debug.Log("icy");
                    if (CheckTarget(targetType, traps[2].trapTargets))
                    {
                        Instantiate(traps[2].particleEffect, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
                        Debug.Log("case1");
                        target.GetComponent<EnemyStats>().TakeDamage();
                        //you can make a partical effect
                        trapsManager.DecreaseScore(1);
                        Destroy(gameObject);
                    }
                    else if(targetType == "IceSorcerer")
                    {
                        Debug.Log("case2");
                        Instantiate(traps[2].particleEffect, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
                        trapsManager.DecreaseScore(1);
                        Destroy(gameObject);
                    }
                    else
                    {
                        Debug.Log("case3");
                        Instantiate(traps[2].particleEffect, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
                        target.GetComponent<EnemyStats>().TakeNegativeEffect("Ice");
                        //you can make a partical effect
                        trapsManager.DecreaseScore(1);
                        Destroy(gameObject);
                    }
                    break;
                case "StageSpike":
                  
                    break;
            }
        } else if (tag == "Player")
        {
            switch (trapType)
            {
                case "Spikes":
                    Instantiate(traps[0].particleEffect, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
                    target.GetComponent<PlayerStats>().TakeMeleDamage(1);
                    //you can make a partical effect
                    trapsManager.DecreaseScore(1);
                    Destroy(gameObject);
                    break;
                case "Fire":
                    Instantiate(traps[1].particleEffect, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
                    target.GetComponent<PlayerStats>().TakeMeleDamage(1);
                    //you can make a partical effect
                    trapsManager.DecreaseScore(1);
                    Destroy(gameObject);
                    break;
                case "Ice":
                    Instantiate(traps[2].particleEffect, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
                    target.GetComponent<PlayerStats>().TakeRangeDamage(0,"Ice");
                    //you can make a partical effect
                    trapsManager.DecreaseScore(1);
                    Destroy(gameObject);
                    break;
                case "StageSpike":

                    Instantiate(traps[5].particleEffect, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
                    target.GetComponent<PlayerStats>().TakeMeleDamage(1);
                    Destroy(gameObject);
                    break;
            }
        }
      
    }

    void DoMultipleDamages()
    {
        string trapType = this.trapType.ToString();
        Collider2D[] hitColliders = CastCircle(new Vector2(transform.position.x, transform.position.y), 2f);
        switch (trapType)
        {
            case "Poison":
                int targetNum = traps[3].targetNum;

                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].CompareTag("Enemy") && targetNum > 0)
                    {
                        hitColliders[i].GetComponent<EnemyStats>().TakeDamage();
                        targetNum--;
                    }

                    if (hitColliders[i].CompareTag("Player"))
                    {
                        hitColliders[i].GetComponent<PlayerStats>().TakeMeleDamage(2);
                    }
                }
                Instantiate(traps[3].particleEffect, new Vector3 (transform.position.x,transform.position.y,-10f), Quaternion.identity);
                trapsManager.DecreaseScore(1);
                Destroy(gameObject);
            break;
            case "Stun":


                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].CompareTag("Enemy"))
                    {
                        if (hitColliders[i].GetComponent<EnemyStats>().unitType.ToString() != "Summoner")
                            hitColliders[i].GetComponent<EnemyStats>().TakeNegativeEffect("Stun");
                    }

                    if (hitColliders[i].CompareTag("Player"))
                    {
                        hitColliders[i].GetComponent<PlayerStats>().TakeRangeDamage(0, "Stun");
                    }
                }
                Instantiate(traps[4].particleEffect, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
                trapsManager.DecreaseScore(1);
                Destroy(gameObject);
                break;
        }
    }



    public bool CheckTarget(string target, string[] trapTargets)
    {
        foreach (string trapTarget in trapTargets)
        {
            if (trapTarget == target)
            {
                return true;
            }
        }
        return false;
    }


    public Collider2D[] CastCircle(Vector2 center, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        return hitColliders;
    }


    void Update()
    {
        if (isRunning)
        {
            timeRemaining -= Time.deltaTime;

        
            UpdateTimerText(timeRemaining);

            if (timeRemaining <= 0)
            {
                isRunning = false;
                TimerFinished();
            }
        }
    }

    public void StartTimer(float duration)
    {
        timerText.enabled = true;
        this.duration = duration;
        timeRemaining = duration;
        isRunning = true;
        UpdateTimerText(timeRemaining); // Update the timer text at the start
    }

    private void TimerFinished()
    {
        DoMultipleDamages();
    }

    private void UpdateTimerText(float time)
    {
        if (timerText != null)
        {
            timerText.text = Mathf.Max(0, time).ToString("F1") + "s";
        }
    }
}
