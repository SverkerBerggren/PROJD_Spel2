using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading;

public class CreateLobby : MonoBehaviour
{
    public TMP_InputField inputField;
    bool keepGoing = true;
    private int lobbyIdToSearch = 0;

    public Button startGameButton;


    public void OnClick()
    {
        RequestHostLobby requestHostLobby = new RequestHostLobby();

        requestHostLobby.whichPlayer = ClientConnection.Instance.playerId;
        requestHostLobby.lobbyName = inputField.text;


        ClientConnection.Instance.AddRequest(requestHostLobby, ResponseHost);

        Thread serverPolling = new Thread(PollServerLobby);


        serverPolling.Start();

    }

    private void PollServerLobby()
    {
       

        while(keepGoing)
        {
            RequestAvailableLobbies request = new RequestAvailableLobbies();

            ClientConnection.Instance.AddRequest(request, ResponseAvailable);
        }
    }

    public void ResponseHost(ServerResponse response)
    {
        ResponseHostLobby responseHostLobby = (ResponseHostLobby)response;

        lobbyIdToSearch = responseHostLobby.lobbyId; 
    }
    public void ResponseAvailable(ServerResponse response)
    {
        ResponseAvailableLobbies responseHostLobby = (ResponseAvailableLobbies)response;

        foreach(Server.HostedLobby lobby in responseHostLobby.Lobbies)
        {
            if(lobby.lobbyId == lobbyIdToSearch)
            {
                startGameButton.interactable = true;
                keepGoing = false;
            }
        }
    }
}
