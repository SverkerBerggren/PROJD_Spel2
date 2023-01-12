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

    public IEnumerator ShowLobbiesEnumerator()
    {   while(true)
        {
            RequestAvailableLobbies request = new RequestAvailableLobbies();

            ClientConnection.Instance.AddRequest(request, ShowLobbies);

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ShowLobbies(ServerResponse response)
    {
        ResponseAvailableLobbies recievedResponse = (ResponseAvailableLobbies)response;

        if(response.SQLIsConnected)
        {
            ClientConnection.Instance.SQLIsConnected = true;
        }

        List<int> allLobbies = new List<int>();

        foreach(Server.HostedLobby lobby in recievedResponse.Lobbies)
        {
            allLobbies.Add(lobby.lobbyId);
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
        List<int> currentLobbyButtons = new List<int>(lobbyButtons.Keys);


        foreach(int intRemove in allLobbies)
        {
            currentLobbyButtons.Remove(intRemove);
        }

        foreach(int intRemove in currentLobbyButtons)
        {
            lobbyButtons.Remove(intRemove);
        }
        

    }
}
