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
    public int lobbyIdToSearch = 100000;

    public Button startGameButton;
    public bool pollServer = false;

    public void OnClick()
    {
        RequestHostLobby requestHostLobby = new RequestHostLobby();

        requestHostLobby.whichPlayer = ClientConnection.Instance.playerId;

        if (inputField.text.Equals(""))
            requestHostLobby.lobbyName = "Lobby";
        else
            requestHostLobby.lobbyName = inputField.text;

        ClientConnection.Instance.isHost = true;
        ClientConnection.Instance.playerId = 0;

        ClientConnection.Instance.AddRequest(requestHostLobby, ResponseHost);

        InternetLoop internetLoop = FindObjectOfType<InternetLoop>();
        internetLoop.hasJoinedLobby = true;

        StartCoroutine(PollCoroutine());

    }






    public IEnumerator PollCoroutine()
    {   
        while(keepGoing)
        {
            RequestAvailableLobbies request = new RequestAvailableLobbies();
            ClientConnection.Instance.AddRequest(request, ResponseAvailable);
            yield return new WaitForSeconds(0.1f);
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
                if(lobby.anotherPlayerJoind)
                {
                    startGameButton.interactable = true;
                    ClientConnection.Instance.gameId = lobby.gameId;
                    keepGoing = false;
                }
            }
        }
    }
}
