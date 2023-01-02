using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckbuilderCardButton : MonoBehaviour
{
	[NonSerialized] public Card card;
	[NonSerialized] public Champion champion;
	private int tapCount = 0;

	public void OnClick()
	{
		tapCount++;
		Invoke(nameof(OnDoAction), 0.2f);
	}

	private void OnDoAction()
	{
		if (tapCount == 1)
		{
			Add(card != null);
		}
		else 
		{
			Remove(card != null);
		}
		tapCount = 0;
	}

	public void Add(bool isCard)
	{
		if(isCard)
			Setup.Instance.AddCard(card);
		else
			Setup.Instance.AddChampion(champion);
	}

	private void Remove(bool isCard)
	{
		if (isCard)
			Setup.Instance.RemoveCard(card);
		else
			Setup.Instance.RemoveChampion(champion);
	}
}
