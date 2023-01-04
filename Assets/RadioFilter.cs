using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioFilter : MonoBehaviour
{
	public CardFilter cardFilter;

	public void ActivateFilter()
	{
		Deckbuilder.Instance.FilterCards(cardFilter);
	}
}
