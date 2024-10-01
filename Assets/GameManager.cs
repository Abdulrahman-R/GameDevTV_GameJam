using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] float loseDelay = 2f; // Delay time for the lose method
    [SerializeField] float winDelay = 2f;  // Delay time for the win method

    PlayerStats playerStats;
    LevelLoader levelLoader;

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    // Public method to start the Lose coroutine
    public void StartLose()
    {
        StartCoroutine(Lose());
    }

    // Public method to start the Win coroutine
    public void StartWin()
    {
        StartCoroutine(Win());
    }

    // Coroutine for handling losing the game
    private IEnumerator Lose()
    {
        if (playerStats == null) yield return null;
        playerStats.DesstroyPlayer();
        yield return new WaitForSeconds(loseDelay);
        levelLoader.ReloadLevel();
    }

    // Coroutine for handling winning the game
    private IEnumerator Win()
    {
    
        yield return new WaitForSeconds(winDelay);
        levelLoader.LoadNextLevel();

    }
}
