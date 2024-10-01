using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTracker : MonoBehaviour
{
    [SerializeField] GameObject[] healthImages;

    // Decrease health by a specified amount of damage
    public void DecreaseHealth(int damage)
    {
        for (int i = healthImages.Length - 1; damage > 0 && i >= 0; i--)
        {
            if (healthImages[i].activeInHierarchy)
            {
                healthImages[i].SetActive(false);
                damage--;
            }
        }
    }

    // Increase health by a specified amount of healing
    public void IncreaseHealth(int healing)
    {
        for (int i = 0; healing > 0 && i < healthImages.Length; i++)
        {
            if (!healthImages[i].activeInHierarchy)
            {
                healthImages[i].SetActive(true);
                healing--;
            }
        }
    }

    // Reset health to full
    public void ResetHealth()
    {
        for (int i = 0; i < healthImages.Length; i++)
        {
            healthImages[i].SetActive(true);
        }
    }

    // Get current health value
    public int GetCurrentHealth()
    {
        int currentHealth = 0;
        for (int i = 0; i < healthImages.Length; i++)
        {
            if (healthImages[i].activeInHierarchy)
            {
                currentHealth++;
            }
        }
        return currentHealth;
    }

    // Get max health value
    public int GetMaxHealth()
    {
        return healthImages.Length;
    }
}
