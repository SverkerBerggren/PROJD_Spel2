using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckbuilderCardButton : MonoBehaviour
{
	[NonSerialized] public Card card;
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
			Add();
		}
		else 
		{
			Remove();
		}
		tapCount = 0;
	}

	public void Add()
	{
		Setup.Instance.AddCard(card);
	}

	private void Remove()
	{
		Setup.Instance.RemoveCard(card);
	}
}
