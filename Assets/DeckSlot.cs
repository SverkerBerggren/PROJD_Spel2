using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class DeckSlot : MonoBehaviour
{
    private string deck;
    private Button deckButton;
	private TMP_Text deckButtonText;
	[SerializeField] private Button deleteButton;
	private Setup setup;
	private Deckbuilder deckbuilder;

	private void Start()
	{
		setup = Setup.Instance;
		deckbuilder = Deckbuilder.Instance;
		deckButton = gameObject.GetComponent<Button>();
		deckButtonText = GetComponentInChildren<TMP_Text>();
	}

	public void SetDeck(string deckName)
    {
		Start();
        deck = deckName;
		deckButtonText.text = deckName;
		deleteButton.interactable = true;
		deckButton.interactable = true;
	}

    public void DeleteDeck()
    {
		if (File.Exists(Setup.savePath + deck + ".txt"))
		{
			File.Delete(Setup.savePath + deck + ".txt");
		}
		deckbuilder.saveDeck.interactable = true;
		deck = "";
		deckButtonText.text = "Empty Deckslot";
		deckButton.interactable = false;
		deleteButton.interactable = false;
	}

    public void OnClick()
    {
        if (!deck.Equals(""))
        {
			setup.LoadDeckToFile(deck);
			deckbuilder.UpdateDeckList();
		}
    }
}
