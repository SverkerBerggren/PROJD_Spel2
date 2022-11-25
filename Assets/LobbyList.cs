using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyList : MonoBehaviour
{

    public GameObject lobbyButton;

    public Dictionary<int, GameObject> lobbyButtons = new Dictionary<int, GameObject>();



    private void Start()
    {
        StartCoroutine(ShowLobbiesEnumerator());
    }

    private void FixedUpdate()
    {


    }

    public IEnumerator ShowLobbiesEnumerator()
    {   while(true)
        {
            RequestAvailableLobbies request = new RequestAvailableLobbies();

            ClientConnection.Instance.AddRequest(request, ShowLobbies);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ShowLobbies(ServerResponse response)
    {
        ResponseAvailableLobbies recievedResponse = (ResponseAvailableLobbies)response;

        foreach(Server.HostedLobby lobby in recievedResponse.Lobbies)
        {
            if (!lobbyButtons.ContainsKey(lobby.lobbyId))
            {

                GameObject createdbutton = Instantiate(lobbyButton, gameObject.transform);
                createdbutton.GetComponent<LobbyButton>().lobbyId = lobby.lobbyId;
                createdbutton.GetComponent<LobbyButton>().SetText(lobby.lobbyName);

                lobbyButtons.Add(lobby.lobbyId,createdbutton);
            }
            else 
            {
                lobbyButtons[lobby.lobbyId].GetComponent<LobbyButton>().SetJoinImage(lobby.anotherPlayerJoind);
            }

        }

        foreach (Server.HostedLobby lobby in recievedResponse.removedLobbies)
        {
            if (lobbyButtons.ContainsKey(lobby.lobbyId))
            {
                lobbyButtons.Remove(lobby.lobbyId);
            }
        }
    }
}
