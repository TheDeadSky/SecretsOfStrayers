using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuActions : MonoBehaviour
{

    [Header("Multiplayer UI Objects")]
    public GameObject hostButton;
    public GameObject joinButton;
    public Text ipAddressText;

    public Menus menuPanels;
	
	void Start ()
    {
        menuPanels.Initialize();
        Return();
	}

    /// <summary>
    /// Open menu to set up single player game.
    /// </summary>
    public void SinglePlayer()
    {
        
    }

    /// <summary>
    /// Open multiplayer menu.
    /// </summary>
    public void MultiPlayer()
    {
        menuPanels.MultiplayerPanel.SetActive(true);
        menuPanels.MainMenuPanel.SetActive(false);
    }

    /// <summary>
    /// Returns to main menu.
    /// </summary>
    public void Return()
    {
        menuPanels.DisableAll();
        menuPanels.MainMenuPanel.SetActive(true);
    }

    /// <summary>
    /// Quit from game.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    public void UpdateMPButtonsListiners()
    {
        //hostButton.GetComponent<Button>().onClick.AddListener(networkManager.StartupHost);
        //joinButton.GetComponent<Button>().onClick.AddListener(networkManager.JoinGame);
    }
}

[System.Serializable]
public class Menus
{
    public GameObject MainMenuPanel;
    public GameObject MultiplayerPanel;
    public GameObject SingleplayerPanel;
    public GameObject SettingsPanel;

    List<GameObject> allPanels = new List<GameObject>();

    /// <summary>
    /// Initialization of panels.
    /// </summary>
    public void Initialize()
    {
        if(MainMenuPanel != null)
        {
            allPanels.Add(MainMenuPanel);
        }
        if(MultiplayerPanel != null)
        {
            allPanels.Add(MultiplayerPanel);
        }
        if(SingleplayerPanel != null)
        {
            allPanels.Add(SingleplayerPanel);
        }
        if(SettingsPanel != null)
        {
            allPanels.Add(SettingsPanel);
        }
    }

    /// <summary>
    /// Disabling all panels.
    /// </summary>
    public void DisableAll()
    {
        if (allPanels.Count <= 0)
            return;

        foreach(var mp in allPanels)
        {
            if(mp.activeInHierarchy == true)
                mp.SetActive(false);
        }
    }

    
}