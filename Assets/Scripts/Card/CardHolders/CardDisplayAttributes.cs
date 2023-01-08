using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayAttributes : MonoBehaviour
{
    private Calculations calculations;

	[SerializeField] private TMP_Text cardName;
	[SerializeField] private TMP_Text manaText;

	[SerializeField] private MeshRenderer artworkMeshRenderer;

    [Header("CardMaterial")]
	[SerializeField] private Material attackCardMaterial;
	[SerializeField] private Material spellCardMaterial;
	[SerializeField] private Material landmarkCardMaterial;

    [SerializeField] private GameObject nameBackground;
	[SerializeField] private GameObject championBorder;
	[SerializeField] private Image currentSprite;
	[SerializeField] private GameObject hpGameObject;

	[NonSerialized] public GameObject championCardHolder;
	[NonSerialized] public int damageShow = 0;
    [NonSerialized] public int amountToHealShow = 0;
    [NonSerialized] public int amountToShieldShow = 0;
    [NonSerialized] public int amountOfCardsToDrawShow = 0;
    [NonSerialized] public int amountOfCardsToDiscardShow = 0;
    [NonSerialized] public bool previewCard = false;

	public TMP_Text hpText;
	public TMP_Text description;
    public GameObject cardPlayableEffect;

    private void UpdateDependingOnCard(Displays display)
    {
        if (display is LandmarkDisplay)
        {
            LandmarkDisplay displayLandmark = (LandmarkDisplay)display;
            hpText.text = displayLandmark.Health.ToString();
        }
        else if (display is CardDisplay)
        {
            CardDisplay cardDisplay = (CardDisplay)display;
            if (!cardDisplay.OpponentCard)
            {
                Card card = cardDisplay.Card;
                UpdateMaterialOnCard(card);
                if (cardPlayableEffect != null)
                    ShowCardPlayableEffect(cardDisplay);
            }
            else
            {
                cardDisplay.SetBackfaceOnOpponentCards(ActionOfPlayer.Instance.BackfaceCard);
                return;
            }
        }

        if (display.Card.ChampionCard && display.Card.ChampionCardType != ChampionCardType.All)
        {
            championBorder.gameObject.SetActive(true);
            currentSprite.gameObject.SetActive(true);
            currentSprite.sprite = CardRegister.Instance.championTypeRegister[display.Card.ChampionCardType].ChampBackground;
        }
        else
        {
            currentSprite.gameObject.SetActive(false);
            championBorder.gameObject.SetActive(false);
        }

        description.text = display.Card.Description;
        manaText.text = display.ManaCost.ToString();
        cardName.text = display.Card.CardName;
    }


    private void ShowCardPlayableEffect(CardDisplay cardDisplay)
    {
        bool isTheRightChampionCard = true;
        if (cardDisplay.Card.ChampionCard)
        {
            CardTargeting cardTargeting = GetComponentInParent<CardTargeting>();
            if (cardDisplay.Card.ChampionCardType != GameState.Instance.PlayerChampion.Champion.ChampionCardType && cardDisplay.Card.ChampionCardType != ChampionCardType.All)
            {
                isTheRightChampionCard = false;
            }
        }
        if (ActionOfPlayer.Instance.CurrentMana >= cardDisplay.ManaCost && GameState.Instance.IsItMyTurn && isTheRightChampionCard)
            cardPlayableEffect.SetActive(true);
        else
            cardPlayableEffect.SetActive(false);
    }


    private void UpdateMaterialOnCard(Card card)
    {
        Material materialToChange = null;
     
        switch (card.TypeOfCard)
        {
            case CardType.Attack:
                nameBackground.SetActive(false);
                hpGameObject.SetActive(false);
                materialToChange = attackCardMaterial;
                break;
            case CardType.Spell:
                nameBackground.SetActive(false);
                hpGameObject.SetActive(false);
                materialToChange = spellCardMaterial;
                break;
            case CardType.Landmark:
                nameBackground.SetActive(true);
                hpGameObject.SetActive(true);
                Landmarks landmarkCard = (Landmarks)card;
                hpText.text = landmarkCard.MinionHealth.ToString();
                materialToChange = landmarkCardMaterial;
                break;
        }

        if (artworkMeshRenderer == null)
        {
            GetComponent<RawImage>().material = materialToChange;
        }
        else
        {
            artworkMeshRenderer.material = materialToChange;
        }
    }

    private void UpdateVariables(Card card)
    {
        calculations = Calculations.Instance;

        if (card.Damage != 0)
            damageShow = calculations.CalculateDamage(card.Damage, previewCard);
        if (card.AmountToHeal != 0)
            amountToHealShow = calculations.CalculateHealing(card.AmountToHeal, previewCard);
        if (card.AmountToShield != 0)
            amountToShieldShow = calculations.CalculateShield(card.AmountToShield, previewCard);
        if (card.AmountOfCardsToDraw != 0)
            amountOfCardsToDrawShow = card.AmountOfCardsToDraw;
        if (card.AmountOfCardsToDiscard != 0)
            amountOfCardsToDiscardShow = card.AmountOfCardsToDiscard;
    }

    public void UpdateVariables(Displays display)
    {
        calculations = Calculations.Instance;

        if (!previewCard)
        {
            if (display is CardDisplay)
            {
                Calculations.Instance.CalculateHandManaCost((CardDisplay)display);
            }
        }

        UpdateVariables(display.Card);
    }

    public void UpdateTextOnCard(Displays display)
    {
        if (display.Card == null) return;
        
        UpdateVariables(display);
        UpdateDependingOnCard(display);
        description.text = CardParser.Instance.CheckKeyword(this);
    }

    public void UpdateTextOnCardWithCard(Card card)
    {
        if (card == null) return;

        if (card.ChampionCard && card.ChampionCardType != ChampionCardType.All)
        {
            championBorder.gameObject.SetActive(true);
            currentSprite.gameObject.SetActive(true);
            currentSprite.sprite = CardRegister.Instance.championTypeRegister[card.ChampionCardType].ChampBackground;
        }
        else
        {
            currentSprite.gameObject.SetActive(false);
            championBorder.gameObject.SetActive(false);
        }

        UpdateMaterialOnCard(card);

        cardName.text = card.CardName;
        manaText.text = card.MaxManaCost.ToString();
        description.text = card.Description;

        UpdateVariables(card);  
        description.text = CardParser.Instance.CheckKeyword(this);
    }
}
