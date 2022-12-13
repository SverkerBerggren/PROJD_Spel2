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
    public GameObject hpGameObject;


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
                UpdateMaterialOnCard(cardDisplay.card);
                if (cardPlayableEffect != null)
                {
                    ShowCardPlayableEffect(cardDisplay);
                }
            }
            else
            {
                cardDisplay.SetBackfaceOnOpponentCards(ActionOfPlayer.Instance.backfaceCard);
            }
        }

        description.text = display.card.description;
        manaText.text = display.manaCost.ToString();
        cardName.text = display.card.cardName;
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

        UpdateMaterialOnCard(card);

        cardName.text = card.cardName;
        manaText.text = card.maxManaCost.ToString();
        description.text = card.description;

        UpdateVariables(card);  
        description.text = CardParser.Instance.CheckKeyword(this);
    }

    private void ShowCardPlayableEffect(CardDisplay cardDisplay)
    {

        bool isTheRightChampionCard = true;
        if (cardDisplay.card.championCard)
        {
            CardTargeting cardTargeting = GetComponentInParent<CardTargeting>();
            if (cardDisplay.card.championCardType != GameState.Instance.playerChampion.champion.championCardType && cardDisplay.card.championCardType != ChampionCardType.All)
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
     
        switch (card.typeOfCard)
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
                hpText.text = landmarkCard.minionHealth.ToString();
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

        if (card.damage != 0)
            damageShow = calculations.CalculateDamage(card.damage, previewCard);
        if (card.amountToHeal != 0)
            amountToHealShow = calculations.CalculateHealing(card.amountToHeal, previewCard);
        if (card.amountToShield != 0)
            amountToShieldShow = calculations.CalculateShield(card.amountToShield, previewCard);
        if (card.amountOfCardsToDraw != 0)
            amountOfCardsToDrawShow = card.amountOfCardsToDraw;
        if (card.amountOfCardsToDiscard != 0)
            amountOfCardsToDiscardShow = card.amountOfCardsToDiscard;
    }

    public void UpdateVariables(Displays display)
    {
        calculations = Calculations.Instance;

        if (display is CardDisplay)
        {
            Calculations.Instance.CalculateHandManaCost((CardDisplay)display);
        }

        UpdateVariables(display.card);
    }


}
