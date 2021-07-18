using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{

    public static ScoreController instance;
    public GameOver GameOver;
    public Text scoreText;
    public Text highscoreText;

    int score = 0;
    int highscore = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0); //persistent highscore in PlayerPrefs

        //transmit the score to the UI element
        scoreText.text = score.ToString() + " POINTS"; 
        highscoreText.text = "Highscore: " + highscore.ToString();
    }

    //called by Dethdelay() function in EnemyController 1/2
    public void AddPoint()
    {
        score += 1;
        scoreText.text = score.ToString() + " POINTS"; //transmit the score to the UI element
        if (highscore < score)
            PlayerPrefs.SetInt("highscore", score); //Overwrite the Highscore in PlayerPrefs
        GameOverScore();
    }

    public void GameOverScore()
    {
        GameOver.Setup(score, highscore);
    }
}
