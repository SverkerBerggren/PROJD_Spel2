using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayAttributes : MonoBehaviour
{
    public TMP_Text cardName;
    public TMP_Text description;
    public TMP_Text manaText;
    public TMP_Text hpText;

    [Header("CardMaterial")]
    public MeshRenderer artworkMeshRenderer;

    public Material attackCardMaterial;
    public Material spellCardMaterial;
    public Material landmarkCardMaterial;

    public GameObject cardPlayableEffect;

    [SerializeField] private GameObject nameBackground;
	[SerializeField] private GameObject championBorder;
	[SerializeField] private Image currentSprite;
    public GameObject hpGameObject;
	[NonSerialized] public GameObject championCardHolder;


	[System.NonSerialized] public int damageShow = 0;
    [System.NonSerialized] public int amountToHealShow = 0;
    [System.NonSerialized] public int amountToShieldShow = 0;
    [System.NonSerialized] public int amountOfCardsToDrawShow = 0;
    [System.NonSerialized] public int amountOfCardsToDiscardShow = 0;

    [System.NonSerialized] public bool previewCard = false;

    private Calculations calculations;

    private void UpdateDependingOnCard(Displays display)
    {
        if (display is LandmarkDisplay)
        {
            LandmarkDisplay displayLandmark = (LandmarkDisplay)display;
            hpText.text = displayLandmark.health.ToString();
        }
        else if (display is CardDisplay)
        {
            CardDisplay cardDisplay = (CardDisplay)display;
            if (!cardDisplay.opponentCard)
            {
                Card card = cardDisplay.card;
                UpdateMaterialOnCard(card);
                if (cardPlayableEffect != null)
                {
                    ShowCardPlayableEffect(cardDisplay);
                }
            }
            else
            {
                cardDisplay.SetBackfaceOnOpponentCards(ActionOfPlayer.Instance.backfaceCard);
                return;
            }
        }

        if (display.card.ChampionCard && display.card.ChampionCardType != ChampionCardType.All)
        {
            championBorder.gameObject.SetActive(true);
            currentSprite.gameObject.SetActive(true);
            currentSprite.sprite = CardRegister.Instance.championTypeRegister[display.card.ChampionCardType].ChampBackground;
        }
        else
        {
            currentSprite.gameObject.SetActive(false);
            championBorder.gameObject.SetActive(false);
        }

        description.text = display.card.Description;
        manaText.text = display.manaCost.ToString();
        cardName.text = display.card.CardName;
    }

    public void UpdateTextOnCard(Displays display)
    {
        if (display.card == null) return;
        
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

    private void ShowCardPlayableEffect(CardDisplay cardDisplay)
    {

        bool isTheRightChampionCard = true;
        if (cardDisplay.card.ChampionCard)
        {
            CardTargeting cardTargeting = GetComponentInParent<CardTargeting>();
            if (cardDisplay.card.ChampionCardType != GameState.Instance.playerChampion.champion.ChampionCardType && cardDisplay.card.ChampionCardType != ChampionCardType.All)
            {
                isTheRightChampionCard = false;
            }
        }
        if (ActionOfPlayer.Instance.currentMana >= cardDisplay.manaCost && GameState.Instance.isItMyTurn && isTheRightChampionCard)
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


        UpdateVariables(display.card);
    }


}
