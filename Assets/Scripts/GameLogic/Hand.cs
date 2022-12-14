using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Hand : MonoBehaviour
{
	public List<CardDisplay> cardSlotsInHand = new List<CardDisplay>();
	public List<CardDisplay> cardsInHand = new List<CardDisplay>();

	[SerializeField] private Vector3 handStartPoition;

	private bool dissolveDone = false;

	private void Start()
	{
		// Anledningen �r load order
		Invoke(nameof(InvokeRefresh), 0.1f);
	}

	private void InvokeRefresh()
	{
		FixCardOrderInHand();
		GameState.Instance.Refresh();
	}

	public void FixCardOrderInHand()
	{
		cardsInHand.Clear();
		if (!cardSlotsInHand[0].OpponentCard)
			transform.position = handStartPoition;
		for (int i = 0; i < cardSlotsInHand.Count; i++)
		{
			CardDisplay cardDisplay = cardSlotsInHand[i];

			if (cardDisplay.Card == null)
			{
				cardDisplay.HideUnusedCard();
				continue;
			}

			if (!cardsInHand.Contains(cardDisplay))
				cardsInHand.Add(cardDisplay);
			if (!cardDisplay.OpponentCard)
				cardDisplay.UpdateTextOnCard();

			if (!cardDisplay.OpponentCard)
			{
				transform.position += new Vector3(-4f, 0, 0);
				cardDisplay.transform.localPosition = new Vector3(-1.75f + (i * 1.75f), -0.5f, -1.55f - (i * 0.05f));
			}
		}
	}


	public Card DiscardRandomCardInHand()
	{
		if (cardsInHand.Count <= 0)
			return null;
		int cardIndex = UnityEngine.Random.Range(0, cardsInHand.Count);
		List<int> cardIndexes = new List<int>() { cardIndex };
		DiscardCardListWithIndexes(cardIndexes, true);
		return cardsInHand[cardIndex].Card;
	}

	public List<Card> DiscardMultipleRandomCards(int amountOfCards, bool isPlayer)
	{
		if (cardsInHand.Count <= 0)
			return null;

		List<int> cardIndexes = new List<int>();
		List<Card> cards = new List<Card>();

		if (cardsInHand.Count < amountOfCards)
			amountOfCards = cardsInHand.Count;

		for (int i = 0; i < amountOfCards; i++)
		{

			int index = UnityEngine.Random.Range(0, cardsInHand.Count);
			if (!cardIndexes.Contains(index))
			{
				cards.Add(cardsInHand[index].Card);
				cardIndexes.Add(index);
			}
			else
				i--;
		}

		DiscardCardListWithIndexes(cardIndexes, isPlayer);
		return cards;
	}

	public List<string> DiscardCardListWithIndexes(List<int> cardIndexes, bool isPlayer)
	{
		List<string> cards = new List<string>();
		List<CardDisplay> cardDisp = new List<CardDisplay>();

		for (int i = 0; i < cardIndexes.Count; i++)
		{
			cardDisp.Add(cardsInHand[cardIndexes[i]]);
			cards.Add(cardDisp[i].Card.CardName);
		}
		dissolveDone = false;
		Dissolve(cardDisp);

		StartCoroutine(RemoveCards(cardIndexes, isPlayer));
		return cards;
	}


	private IEnumerator RemoveCards(List<int> cardIndexes, bool isPlayer)
	{
		yield return new WaitUntil(() => dissolveDone == true);
		List<int> cardIndexesCopy = new List<int>(cardIndexes);
		cardIndexesCopy = FixIndexesWhenRemovingCards(cardIndexesCopy);
		for (int i = 0; i < cardIndexesCopy.Count; i++)
		{
			ActionOfPlayer.Instance.ChangeCardOrder(isPlayer, cardsInHand[cardIndexesCopy[i]]);
		}
	}

	public void FixMulligan(List<int> cardIndexes)
	{
		cardIndexes = FixIndexesWhenRemovingCards(cardIndexes);
		Deck deck = Deck.Instance;
		for (int i = 0; i < cardIndexes.Count; i++)
		{
			Card card = cardsInHand[cardIndexes[i]].Card;
			deck.AddCardToDeckPlayer(card);
			ActionOfPlayer.Instance.ChangeCardOrder(true, cardsInHand[cardIndexes[i]]);
		}
		deck.ShuffleDeck();
		ActionOfPlayer.Instance.DrawCardPlayer(cardIndexes.Count, null, true);
	}

	private List<int> FixIndexesWhenRemovingCards(List<int> indexes)
	{
		for (int i = 0; i < indexes.Count; i++)
		{
			for (int j = i + 1; j < indexes.Count; j++)
			{
				if (indexes[j] > indexes[i])
				{
					indexes[j]--;
				}
			}
		}
		return indexes;
	}

	private async void Dissolve(List<CardDisplay> cardDisplays)
	{
		Task[] tasks = new Task[cardDisplays.Count];

		for (int i = 0; i < cardDisplays.Count; i++)
		{
			await Task.Delay(50);
			tasks[i] = cardDisplays[i].CardDissolve.DissolveCard();
		}

		await Task.WhenAll(tasks);
		dissolveDone = true;
	}
}

