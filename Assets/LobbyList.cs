using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyList : MonoBehaviour
{

    public GameObject lobbyButton;

    public List<GameObject> lobbyButtons = new List<GameObject>();



    private void FixedUpdate()
    {
        RequestAvailableLobbies request = new RequestAvailableLobbies();

        ClientConnection.Instance.AddRequest(request, ShowLobbies);


    }

    public void ShowLobbies(ServerResponse response)
    {
        ResponseAvailableLobbies recievedResponse = (ResponseAvailableLobbies)response;

        foreach(GameObject obj  in lobbyButtons)
        {
            Destroy(obj);
        }
        lobbyButtons.Clear();

        foreach(Server.HostedLobby lobby in recievedResponse.Lobbies)
        {
            GameObject createdbutton = Instantiate(lobbyButton, gameObject.transform);
            createdbutton.GetComponent<LobbyButton>().lobbyId = lobby.lobbyId;
        }
        
    }
}
