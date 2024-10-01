using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrapsManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int currentScore;

    int triggerScore; // Score at which behavior is triggered
    int enemiesNum;
    GameManager gameManager;

    void Start()
    {
        triggerScore = GameObject.FindGameObjectsWithTag("trap").Length;
        currentScore = triggerScore;
        gameManager = FindObjectOfType<GameManager>();
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
        if (currentScore <= 0)
        {
            Invoke("TriggerBehavior", 2f);
        }
    }

    private void TriggerBehavior()
    {
        enemiesNum = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemiesNum > 0)
        {
            gameManager.StartLose();
            Debug.Log("you lost");
        }
    }
}
