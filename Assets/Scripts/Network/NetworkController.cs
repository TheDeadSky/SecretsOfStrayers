using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkController : NetworkManager
{

    Text ipAddressText;

    public void StartupHost()
    {
        NetworkServer.Reset();
        SetPort();
        singleton.StartHost();
    }

    public void JoinGame()
    {
        SetIPAddress(ipAddressText.text);
        SetPort();
        singleton.StartClient();
    }

    public void Disconnect()
    {
        singleton.StopServer();
        singleton.client.Disconnect();
    }

    void SetIPAddress(string ipAddress)
    {
        singleton.networkAddress = ipAddress;
    }

    void SetPort()
    {
        singleton.networkPort = 7777;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += NextSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= NextSceneLoaded;
    }

    void NextSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MainMenuScene")
        {
            GameObject tempGO = GameObject.Find("MainMenu");
            tempGO.GetComponent<MainMenuActions>().networkManager = this;
            tempGO.GetComponent<MainMenuActions>().UpdateMPButtonsListiners();
            ipAddressText = tempGO.GetComponent<MainMenuActions>().ipAddressText;
        }
        else
        {
            GameObject.Find("GameMenu").GetComponent<GameMenuActions>().networkManager = this;
        }
    }
}
