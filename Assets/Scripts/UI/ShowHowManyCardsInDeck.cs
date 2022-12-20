using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowHowManyCardsInDeck : MonoBehaviour
{
    [SerializeField] private GameObject howManyCardsPanel;
    [SerializeField] private TMP_Text amountOfCardsText;

    private Deck deck;

    private void Start()
    {
        deck = Deck.Instance;
    }

    private void OnMouseEnter()
    {
        if (deck != null)
            amountOfCardsText.text = "You have " + deck.deckPlayer.Count + " Cards in your deck";
        else
            amountOfCardsText.text = "You have " + Graveyard.Instance.graveyardPlayer.Count + " Cards in your graveyard";
        howManyCardsPanel.SetActive(true);
    }

    private void OnMouseExit()
    {
        howManyCardsPanel.SetActive(false);
    }
}
