using System;
using UnityEngine;

public class DeckbuilderCardButton : MonoBehaviour
{
	private int tapCount = 0;
	[NonSerialized] public Card Card;
	[NonSerialized] public Champion Champion;

	public void OnClick()
	{
		tapCount++;
		Invoke(nameof(OnDoAction), 0.2f);
	}

	private void OnDoAction()
	{
		if (tapCount == 1)
			Add(Card != null);
		else 
			Remove(Card != null);
		tapCount = 0;
	}

	public void Add(bool isCard)
	{
		if(isCard)
			Setup.Instance.AddCard(Card);
		else
			Setup.Instance.AddChampion(Champion);
	}

	private void Remove(bool isCard)
	{
		if (isCard)
			Setup.Instance.RemoveCard(Card);
		else
			Setup.Instance.RemoveChampion(Champion);
	}
}
