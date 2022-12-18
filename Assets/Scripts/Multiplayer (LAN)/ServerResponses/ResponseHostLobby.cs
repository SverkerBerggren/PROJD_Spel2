using System.Collections;
using System.Collections.Generic;


public class ResponseHostLobby : ServerResponse
{
    public string lobbyName = "";
    public int lobbyId = 0; 
    public ResponseHostLobby()
    {
        Type = 16;
    }
}
