using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Change Scenes

public class MainMenu : MonoBehaviour {

    private void Start()
    {
        SceneManager.UnloadSceneAsync("Main");
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Quit () {

    	Debug.Log("Quit");
    	Application.Quit();
    }
}
