using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTimer : MonoBehaviour
{
   // GameController gameController;
     public float duration;
     float TimeLeft;
     bool TimerOn = false;

    AudioSource audioSource;

    public GameObject TimerTxt;
    GameManager gameManager;

    void Start()
    {
        //gameController = FindObjectOfType<GameController>();
        TimeLeft = duration;
        TimerOn = false;
        updateTimer(duration);
        gameManager = FindObjectOfType<GameManager>();
       // audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) && !TimerOn)
        {
            TimerOn = true;
        }

        if (TimerOn)
        {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else
            {
                gameManager.StartLose();
                Debug.Log("Time is UP!");
                TimeLeft = 0;
                TimerOn = false;
                
            }
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        if (minutes <= 0 && seconds <= duration * 0.0 )
        {
            /*
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
          //  gameController.Lose();
           // gameObject.SetActive(false);
            */
           // Debug.Log("YOU LOST");
        }else if (minutes <= 0 && seconds <= duration * 0.3)
        {
            TimerTxt.GetComponent<TMP_Text>().color = Color.red;
            /*
            if (!(audioSource.isPlaying))
            {
               // audioSource.Play();
            }
            */
            
        }
        else if (minutes <= 0 && seconds <= duration * 0.5) {
           
        }
        else if (minutes <= 0 && seconds <= duration * 0.7)
        {
          
        }
        TimerTxt.GetComponent<TMP_Text>().text = "" + string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
