using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    
    
    public void OnClick()
    {
        RequestGameSetup gameSetup = new RequestGameSetup();
        gameSetup.whichPlayer = 1;
        gameSetup.reciprocate = true;

        gameSetup.lobbyId = FindObjectOfType<CreateLobby>().lobbyIdToSearch;


        // gameSetup.Type = 15;
        List<string> ownChampions = new List<string>();

        foreach (string stringen in Setup.Instance.myChampions)
        {
            ownChampions.Add(stringen);
        }
        gameSetup.opponentChampions = ownChampions;

        print("Not In method");
        ClientConnection.Instance.AddRequest(gameSetup, EmptyMethod);
    }

    public void EmptyMethod(ServerResponse response)
    {
        
    }
}
