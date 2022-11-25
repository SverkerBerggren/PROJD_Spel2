using System.Collections;
using System.Collections.Generic;


public class RequestHostLobby : ClientRequest
{
    public string lobbyName = "";
    public RequestHostLobby()
    {
        Type = 18;
    }
}
