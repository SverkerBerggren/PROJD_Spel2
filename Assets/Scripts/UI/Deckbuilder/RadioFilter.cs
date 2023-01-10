using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioFilter : MonoBehaviour
{
	public CardFilter CardFilter;

	public void ActivateFilter()
	{
		Deckbuilder.Instance.FilterCards(CardFilter);
	}
}
