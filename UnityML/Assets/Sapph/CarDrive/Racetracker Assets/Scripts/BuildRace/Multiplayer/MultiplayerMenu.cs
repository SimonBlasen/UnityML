using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerMenu : MonoBehaviour
{
    public InputField inputPlayername;
    public InputField inputIP;
    public InputField inputPort;


	// Use this for initialization
	void Start ()
    {
        inputIP.text = "m.m-core.eu";
        inputPort.text = "24400";
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ButtonBackClick()
    {
        SceneManager.LoadScene("Garages");
    }

    public void ButtonConnectClick()
    {
        if (inputPlayername.text.Length > 0 && inputIP.text.Length > 0 && inputPort.text.Length > 0 && inputPlayername.text.Length <= 16)
        {
            IpAndPort.playername = inputPlayername.text;
            IpAndPort.ip = inputIP.text;
            try
            {
                IpAndPort.port = Convert.ToInt32(inputPort.text);

                SceneManager.LoadScene("Multiplayer");
            }
            catch(Exception ex)
            {

            }
        }
    }
}
