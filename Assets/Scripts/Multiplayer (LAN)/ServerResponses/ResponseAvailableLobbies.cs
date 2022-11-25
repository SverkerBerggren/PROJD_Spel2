using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseAvailableLobbies : ServerResponse
{   

    public List<Server.HostedLobby> Lobbies = new List<Server.HostedLobby>();

    public List<Server.HostedLobby> removedLobbies = new List<Server.HostedLobby>();
    public ResponseAvailableLobbies()
    {
        Type = 15; 
    }
}
