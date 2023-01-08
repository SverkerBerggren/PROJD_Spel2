using UnityEngine;

public class CardFilterButton : MonoBehaviour
{
	[SerializeField] private CardType cardType;

	public void OnClick()
	{
		Deckbuilder.Instance.CardTypeFilter = cardType;
	}
}
