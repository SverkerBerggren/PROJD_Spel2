using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyButton : MonoBehaviour
{
    public int lobbyId = 0;

    public string lobbyName = "";

    public TextMeshProUGUI textMesh; 

    public GameObject joinImage;

    public void OnClick()
    {   
        if(ClientConnection.Instance.isHost)
        {
            return;     
        }
        RequestJoinLobby requestJoinLobby = new RequestJoinLobby();
        requestJoinLobby.lobbyId = lobbyId; 


        SetJoinImage(true);


        ClientConnection.Instance.AddRequest(requestJoinLobby, ResponseJoinLobby);
    }

    public void ResponseJoinLobby(ServerResponse response)
    {   
        ResponseJoinLobby responseJoinLobby = (ResponseJoinLobby)response;
        ClientConnection.Instance.gameId = responseJoinLobby.gameId;

        InternetLoop internetLoop = FindObjectOfType<InternetLoop>();
        internetLoop.hasJoinedLobby = true;
    }

    public void SetText(string text)
    {
        textMesh.text = text;
    }


    public void SetJoinImage(bool state)
    {
        joinImage.SetActive(state);
    }
}
