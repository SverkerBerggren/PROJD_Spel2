using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeckSlot : MonoBehaviour
{
    private string deck;
    private Button deckButton;
	[SerializeField] private Button deleteButton;

	private void Start()
	{
		deckButton = gameObject.GetComponent<Button>();
	}

	public void SetDeck(string deckName)
    {
        deck = deckName;
		deleteButton.interactable = true;
		deckButton.interactable = true;
	}

    public void DeleteDeck()
    {
        deck = "";
		deckButton.interactable = false;
		deleteButton.interactable = false;
	}

    public void OnClick()
    {
        if (!deck.Equals(""))
        {
			Setup.Instance.LoadDeck(deck);
			Deckbuilder.Instance.UpdateDeckList();
		}
    }
}
