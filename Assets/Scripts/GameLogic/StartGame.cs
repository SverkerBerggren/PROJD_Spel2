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

        List<string> ownChampions = new List<string>();

        foreach (string stringen in Setup.Instance.myChampions)
        {
            ownChampions.Add(stringen);
        }
        gameSetup.opponentChampions = ownChampions;
        if(gameSetup.opponentChampions == null)
        {
            gameSetup.opponentChampions = new List<string>();
        }
        Dictionary<string,int> deckListToSend = new Dictionary<string, int>();
        List<CardAndAmount> cardsToSend = new List<CardAndAmount>();
        foreach (Card card in Setup.Instance.playerDeckList)
        {
            if(!deckListToSend.ContainsKey(card.CardName))
            {
                deckListToSend.Add(card.CardName, 1);
            }
            else
            {
                deckListToSend[card.CardName] += 1;
            }
        }
        foreach(string cardName in deckListToSend.Keys)
        {
            CardAndAmount cardAndAmountToAdd = new CardAndAmount();
            cardAndAmountToAdd.cardName = cardName;
            cardAndAmountToAdd.amount = deckListToSend[cardName];
            cardsToSend.Add(cardAndAmountToAdd);
        }

        gameSetup.deckList = cardsToSend;

        if(gameSetup.deckList == null)
        {
            gameSetup.deckList = new List<CardAndAmount>();
        }

        if ( Random.Range(0, 2) == 0)
        {
            gameSetup.firstTurn = true;
            Setup.Instance.shouldStartGame = true;
        }
        else
        {
            Setup.Instance.shouldStartGame = false;
            gameSetup.firstTurn = false;
        }

        print("Not In method");
        ClientConnection.Instance.AddRequest(gameSetup, EmptyMethod);
    }

    public void EmptyMethod(ServerResponse response) {} // Is used to not create errors
}
