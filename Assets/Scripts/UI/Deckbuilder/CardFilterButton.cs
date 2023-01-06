using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFilterButton : MonoBehaviour
{
	[SerializeField] private CardType cardType;

	public void OnClick()
	{
		Deckbuilder.Instance.cardTypeFilter = cardType;
	}
}
