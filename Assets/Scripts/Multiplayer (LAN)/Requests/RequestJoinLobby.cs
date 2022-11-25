using System.Collections;
using System.Collections.Generic;


public class RequestJoinLobby : ClientRequest
{
    
    public int lobbyId = 0; 
    public RequestJoinLobby()
    {
        Type = 19;
    }

}
