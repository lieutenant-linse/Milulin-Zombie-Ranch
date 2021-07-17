using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text scoreText;
    public Text highscoreText;

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void Quit() {

    	Debug.Log("Quit");
    	Application.Quit();
    }

    public void Setup(int score, int highscore)
    {
        scoreText.text = "SCORE: " + score.ToString() + " POINTS";
        highscoreText.text = "HIGHSCORE: " + highscore.ToString() + " POINTS";
    }
    
}