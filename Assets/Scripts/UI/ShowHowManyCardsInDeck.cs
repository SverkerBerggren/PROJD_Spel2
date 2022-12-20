using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ShowHowManyCardsInDeck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject howManyCardsPanel;
    [SerializeField] private TMP_Text amountOfCardsText;
    [SerializeField] private bool opponentDeck;

    private Deck deck;

    private void Start()
    {
        deck = Deck.Instance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    { 

        if(opponentDeck)
            amountOfCardsText.text = "You have " + deck.deckOpponent.Count + " Cards in your deck";
        else
            amountOfCardsText.text = "You have " + deck.deckPlayer.Count + " Cards in your deck";

        


        howManyCardsPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        howManyCardsPanel.SetActive(false);
    }
}
