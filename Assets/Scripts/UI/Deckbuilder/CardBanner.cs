using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardBanner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private int count = 0;
	private Card card;

	private GameObject preview;
	private CardDisplayAttributes attributes;
	[SerializeField] private MeshRenderer artworkMeshRenderer;
	[SerializeField] private Material attackCardMaterial;
	[SerializeField] private Material spellCardMaterial;
	[SerializeField] private Material landmarkCardMaterial;
	[SerializeField] private TMP_Text cardName;
	[SerializeField] private TMP_Text manaCost;
	[SerializeField] private TMP_Text countText;
	[SerializeField] private GameObject championBorder;
	[SerializeField] private Image currentSprite;


	public void SetCard(Card card)
	{
		this.card = card;
		preview = GameObject.FindGameObjectWithTag("PreviewCard").transform.GetChild(0).gameObject;
		attributes = preview.GetComponent<CardDisplayAttributes>();
		count = 1;
		countText.text = "x " + count;
		cardName.text = card.CardName;
		manaCost.text = card.MaxManaCost.ToString();

		switch (card.TypeOfCard)
		{
			case CardType.Attack:
				artworkMeshRenderer.material = attackCardMaterial;
				break;

			case CardType.Spell:
				artworkMeshRenderer.material = spellCardMaterial;
				break;

			case CardType.Landmark:
				artworkMeshRenderer.material = landmarkCardMaterial;
				break;
		}

		if (card.ChampionCard) // Adds a champion border when its a champion card
		{
			championBorder.gameObject.SetActive(true);
			CardRegister cardRegister = CardRegister.Instance;
			currentSprite.sprite = cardRegister.championTypeRegister[card.ChampionCardType].ChampBackground;
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

	public void OnClick()
	{
		Setup setup = Setup.Instance;
		if (!card.ChampionCard)
			setup.RemoveCard(card);
		else
		{
			CardRegister cardRegister = CardRegister.Instance;
			setup.RemoveChampion(cardRegister.championTypeRegister[card.ChampionCardType]);
		}
		preview.SetActive(false);
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
		preview.SetActive(true);
		attributes.previewCard = true;
		attributes.UpdateTextOnCardWithCard(card);
	}

    public void OnPointerExit(PointerEventData eventData)
    {
		preview.SetActive(false);
	}
}
