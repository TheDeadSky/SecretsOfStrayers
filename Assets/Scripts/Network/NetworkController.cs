using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : NetworkManager
{

	public void StartupHost()
    {
        SetPort();

        singleton.StartHost();
        Debug.Log(singleton.serverBindToIP);
    }

    public void JoinGame()
    {
        SetIPAddress();
        SetPort();
        singleton.StartClient();
    }

    void SetIPAddress()
    {
        singleton.networkAddress = "";
    }

    void SetPort()
    {
        singleton.networkPort = 7777;
    }
}
