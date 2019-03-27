using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour {

	
	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    /// <summary>
    /// Open menu to set up single player game.
    /// </summary>
    public void SinglePlayer()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    /// <summary>
    /// Open multiplayer menu.
    /// </summary>
    public void MultiPlayer()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    /// <summary>
    /// Quit from game.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
