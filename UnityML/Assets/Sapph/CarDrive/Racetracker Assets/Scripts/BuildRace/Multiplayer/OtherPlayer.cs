using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer
{
    private string playerName = "None";

    public OtherPlayer()
    {

    }

    public string PlayerName
    {
        get
        {
            return playerName;
        }
        set
        {
            playerName = value;
        }
    }
}
