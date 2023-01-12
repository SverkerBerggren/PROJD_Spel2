using System.Collections;
using System.Collections.Generic;
public class ResponseAvailableLobbies : ServerResponse
{   

    public List<Server.HostedLobby> Lobbies = new List<Server.HostedLobby>();

    public List<int> removedLobbies = new List<int>();
    public ResponseAvailableLobbies()
    {
        Type = 15; 
    }
}
