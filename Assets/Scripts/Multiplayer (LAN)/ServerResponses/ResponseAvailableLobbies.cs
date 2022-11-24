using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseAvailableLobbies : ServerResponse
{   

    public List<Server.HostedLobby> Lobbies;
    public ResponseAvailableLobbies()
    {
        Type = 15; 
    }
}
