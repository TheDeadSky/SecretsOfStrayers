using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Temp_NWControl : MonoBehaviour
{
	
	void Start ()
    {

	}
	
	
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.L))
            GetComponent<NetworkManagerHUD>().enabled = false;
        if(Input.GetKeyDown(KeyCode.Escape))
            GetComponent<NetworkManagerHUD>().enabled = true;
    }
}
