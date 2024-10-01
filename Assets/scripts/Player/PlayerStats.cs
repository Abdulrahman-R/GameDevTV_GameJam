using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    HealthTracker healthTracker;
    CameraShake cameraShake;
    [SerializeField] float moveSpeed = 5f;
    
    PlayerMovement playerMovement;
    [SerializeField] int health;
    [SerializeField] GameObject bloodEffect;

    //negative effect configuration
    [HideInInspector] public bool effectApplied;
    private bool iceEffectActive = false;
    private bool isStuned = false;
    private float iceEffDuration = 5f; // Default duration of the negative effect in seconds
    private float stunEffDuration = 2f;
    private float timer = 0f;

    bool hidden;
    bool shadesLocked;
    int chasingCounter;
    [SerializeField] Sprite[] statusImages;
    [SerializeField]Image statusImage;

    public Sprite[] playerGFXSats;
    SpriteRenderer playerGFXRenderer;

    ShadesController shadesController;


    InteractionSystem interactionSystem;

    GameManager gameManager;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        effectApplied = false;
        playerMovement = GetComponent<PlayerMovement>();
        cameraShake = FindObjectOfType<CameraShake>();
        healthTracker = GetComponent<HealthTracker>();
        interactionSystem = GetComponent<InteractionSystem>();

        shadesController = FindObjectOfType<ShadesController>();
        hidden = false;
        shadesLocked = false;
        playerGFXRenderer = transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        if (iceEffectActive)
        {
            timer -= Time.deltaTime;
            UpdateTimerText();

            if (timer <= 0)
            {
                EndAllEffects();
            }
        } else if (isStuned)
        {
            timer -= Time.deltaTime;
            UpdateTimerText();

            if (timer <= 0)
            {
                EndAllEffects();
            }
        }

        if(chasingCounter > 0)
        {
            hidden = false;

            if(!statusImage.enabled)
            {
                Debug.Log("happend 298329");
                statusImage.sprite = statusImages[1];
                statusImage.enabled = true;
            }
           
            if (!shadesLocked)
            {
                shadesLocked = true;
                shadesController.LockAllShades();
            }

        }
        else
        {
            if (!hidden && statusImage.enabled)
            {
                statusImage.enabled = false;
            }

            if (shadesLocked)
            {
                shadesLocked = false;
                shadesController.UnlockAllShades();
            }
        }

        if (hidden)
        {
            statusImage.sprite = statusImages[0];
            statusImage.enabled = true;
        }
        else
        {
            if(chasingCounter == 0)
            statusImage.enabled = false;
        }
    }
    public void TakeMeleDamage(int damage)
    {
        TakeDamage(damage);
    }

    public void TakeRangeDamage(int damage, string type)
    {
        switch (type)
        {
            case "Fire":
                TakeDamage(damage);
                break;
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






    void TakeDamage(int damage)
    {
        health -= damage;
        healthTracker.DecreaseHealth(damage);
        audioManager.PlaySound("attacked");

        if (interactionSystem.GetIsPicked())
        {
            interactionSystem.Release();
        }

        if (health <= 0)
        {

            gameManager.StartLose();
        }
        else
        {
            CinemachineShake.Instance.ShakeCamera(5f, 5f, 1f);
        }
    }






    public void ApplyIceEffect(float additionalDuration = 0f)
    {
        audioManager.PlaySound("attacked");
        if (iceEffectActive)
        {
            // Extend the timer if the effect is already active
            timer += additionalDuration; // Subtract to increase the remaining time

            if (interactionSystem.GetIsPicked())
            {
                interactionSystem.Release();
            }
            CinemachineShake.Instance.ShakeCamera(5f, 5f, 1f);
        }
        else
        {
            if (interactionSystem.GetIsPicked())
            {
                interactionSystem.Release();
            }
            CinemachineShake.Instance.ShakeCamera(5f, 5f, 1f);

            playerMovement.moveSpeed = (moveSpeed * 0.5f);
            effectApplied = true;
            iceEffectActive = true;
            timer = iceEffDuration;
            timerText.enabled = true;
            playerGFXRenderer.sprite = playerGFXSats[2];
            
        }

        // Ensure the timer does not exceed the effect duration
        if (timer < 0)
        {
            timer = 0;
        }
    }


    // This method is called to end all negative effects
    private void EndAllEffects()
    {
        effectApplied = false;
        iceEffectActive = false;
        isStuned = false;
        timer = 0f;
        playerMovement.moveSpeed = moveSpeed;
        playerMovement.canMove = true;
        timerText.enabled = false;
        playerGFXRenderer.sprite = playerGFXSats[0];

    }

    public void ApplyStunEffect()
    {
        audioManager.PlaySound("attacked");
        playerMovement.canMove = false;
        effectApplied = true;
        isStuned = true;
        timer = stunEffDuration;
        timerText.enabled = true;

        if (interactionSystem.GetIsPicked())
        {
            interactionSystem.Release();
        }
        CinemachineShake.Instance.ShakeCamera(5f, 5f, 1f);
        playerGFXRenderer.sprite = playerGFXSats[3];

        // Ensure the timer does not exceed the effect duration, i don't think it is important at all
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


    public void ChangePlayerSpeed(float percentage)
    {
        playerMovement.moveSpeed = moveSpeed * percentage;
    }

    public float GetSpeed()
    {
        return moveSpeed;
    }


    public bool GetHidden()
    {
        return hidden;
    }

    public void SetHidden(bool newValue)
    {
        hidden = newValue;
    }

    public int GetChasingCounter()
    {
        return chasingCounter;
    }

    public void UpdateChasingCounter(int updateValue)
    {
        if(updateValue < 0)
        {
            if(GetChasingCounter() > 0)
            chasingCounter += updateValue;
        }
        else
        {
            chasingCounter += updateValue;
        }
    }

    public void DesstroyPlayer()
    {
        CinemachineShake.Instance.ShakeCamera(5f, 5f, 1f);
        Instantiate(bloodEffect, new Vector3(transform.position.x, transform.position.y, -11f), Quaternion.identity);
        Destroy(gameObject);
    }
}
