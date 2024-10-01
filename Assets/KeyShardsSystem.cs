using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyShardsSystem : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int currentScore;
    [SerializeField] GameObject[] uiImages;

    [SerializeField] int triggerScore; // Score at which behavior is triggered
    bool doorOpend;

    AudioManager audioManager;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        doorOpend = false;
        UpdateScoreText();
    }

    public void IncreaseScore(int amount)
    {
        currentScore += amount;
        UpdateScoreText();
        CheckScore();
    }

    public void DecreaseScore(int amount)
    {
        currentScore -= amount;
        UpdateScoreText();
        CheckScore();
    }

    private void UpdateScoreText()
    {
        scoreText.text = currentScore.ToString() + "/" + triggerScore;
    }

    private void CheckScore()
    {
        if (currentScore >= triggerScore)
        {
            audioManager.PlaySound("win");
            TriggerBehavior();
        }
        else
        {
            audioManager.PlaySound("collect");
        }
    }

    private void TriggerBehavior()
    {
        uiImages[0].SetActive(false);
        uiImages[1].SetActive(true);
        doorOpend = true;
    }

    public bool GetDoorOpened()
    {
        return doorOpend;
    }
}
