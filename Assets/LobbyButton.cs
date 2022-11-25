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
        RequestJoinLobby requestJoinLobby = new RequestJoinLobby();

        SetJoinImage(true);


        ClientConnection.Instance.AddRequest(requestJoinLobby, ResponseJoinLobby);
    }

    public void ResponseJoinLobby(ServerResponse response)
    {   
        ResponseJoinLobby responseJoinLobby = (ResponseJoinLobby)response;
        ClientConnection.Instance.gameId = responseJoinLobby.gameId;
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
