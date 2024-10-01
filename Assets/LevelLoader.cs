using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
   public Animator animator;
    public float transitionTime;
    // Update is called once per frame

    [SerializeField] bool inMenu = false;
    [SerializeField] bool inLastScreen = false;
  
    void Update()
    {
        if(inMenu == true)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                LoadNextLevel();


            }


            if (Input.GetKeyDown(KeyCode.Q))
            {
                QuitGame();

            }
        }
        else if(inLastScreen == true)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                LoadNextLevel();


            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                LoadLevelByIndex(0);

            }
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.R))
            {
                ReloadLevel();


            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                LoadLevelByIndex(0);

            }
        }
        
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ReloadLevel()
    {
        
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        animator.SetTrigger("start");
        Time.timeScale = 1;
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    public void LoadLevelByIndex(int levelIndex)
    {
        StartCoroutine(LoadLevel(levelIndex));
    }

   
    public void QuitGame()
    {

        Application.Quit();
    }

}
