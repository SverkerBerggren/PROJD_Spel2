using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyButton : MonoBehaviour
{
    public int lobbyId = 0;

    public string lobbyName = ""; 

    public void OnClick()
    {   
        RequestJoinLobby requestJoinLobby = new RequestJoinLobby();
        


        ClientConnection.Instance.AddRequest(requestJoinLobby, ResponseJoinLobby);
    }

    public void ResponseJoinLobby(ServerResponse response)
    {   
        ResponseJoinLobby responseJoinLobby = (ResponseJoinLobby)response;
        ClientConnection.Instance.gameId = responseJoinLobby.gameId;
    }

}
