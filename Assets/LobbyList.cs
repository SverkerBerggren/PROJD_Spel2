using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyList : MonoBehaviour
{

    public GameObject lobbyButton;


    private void FixedUpdate()
    {
        RequestAvailableLobbies request = new RequestAvailableLobbies();

        ClientConnection.Instance.AddRequest(request, ShowLobbies);


    }

    public void ShowLobbies(ServerResponse response)
    {
        ResponseAvailableLobbies recievedResponse = (ResponseAvailableLobbies)response;


        foreach(Server.HostedLobby lobby in recievedResponse.Lobbies)
        {
            GameObject createdbutton = Instantiate(lobbyButton, gameObject.transform);
        }
        
    }
}
