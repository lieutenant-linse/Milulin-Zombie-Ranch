using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    //UI Elements
    public GameObject endscreen;
    public GameObject timesUpText;
    Image timerBar;
    public GameObject player;
    
    //Time Elements
    public float maxTime = 180f;
    float timeLeft;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        //UI Elements
        timesUpText.SetActive(false);
        timerBar = GetComponent<Image>();


        timeLeft = maxTime; //timeleft starts with maxTime
        Time.timeScale = 1; //as fast as "real" time 
        endscreen = GameObject.FindGameObjectWithTag("GameOverScreen");
        endscreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0)
        {
            //timer count down
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
        }
        else //Timer is over - UI Elemts get activated
        {
            timesUpText.SetActive(true);
            StartCoroutine(TimeOverDelay());
            
        }
    }

    private IEnumerator TimeOverDelay()
    {
        player.GetComponent<PlayerMovement>().playerDead = true;
        yield return new WaitForSeconds(1f);
        endscreen.SetActive(true);
        Time.timeScale = 0;
    }
}
