using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardBanner : MonoBehaviour
{
	private MeshRenderer artworkMeshRenderer;
	private int count = 0;
	[SerializeField] private TMP_Text cardName;
	[SerializeField] private TMP_Text manaCost;
	[SerializeField] private TMP_Text countText;
	[SerializeField] private GameObject championBorder;
	[SerializeField] private Image currentSprite;


	public void SetCard(Card card)
	{
		count = 1;
		countText.text = "x " + count;
		cardName.text = card.CardName;
		manaCost.text = card.MaxManaCost.ToString();
		//

		if (card.ChampionCard)
		{
			championBorder.gameObject.SetActive(true);
			currentSprite.sprite = CardRegister.Instance.championTypeRegister[card.ChampionCardType].ChampBackground;
			currentSprite.gameObject.SetActive(true);
		}
		else
		{
			championBorder.gameObject.SetActive(false);
			currentSprite.gameObject.SetActive(false);
		}
	}

	public void SetValue(int value)
	{
		count = value;

		if (count <= 0)
			gameObject.SetActive(false);
		else
		{
			countText.text = "x " + count;
			gameObject.SetActive(true);
		}
	}
}
