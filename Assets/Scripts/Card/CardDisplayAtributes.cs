using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDisplayAtributes : MonoBehaviour
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
    [SerializeField] private GameObject hpGameObject;


    public void UpdateTextOnCard(LandmarkDisplay landmarkDisplay)
    {
        if (landmarkDisplay.card == null)
        {
            landmarkDisplay.landmarkPrefab.SetActive(false);
            return;
        }
        landmarkDisplay.landmarkPrefab.SetActive(true);
        hpText.text = landmarkDisplay.health.ToString();
        description.text = landmarkDisplay.card.description;
        manaText.text = landmarkDisplay.manaCost.ToString();
        cardName.text = landmarkDisplay.card.cardName;      
    }

    public void UpdateTextOnCard(CardDisplay cardDisplay)
    {
        if (cardDisplay.card == null) return;


        if (!cardDisplay.opponentCard)
        {
            UpdateMaterialOnCard(cardDisplay.card);

            cardDisplay.UpdateVariables();
            cardName.text = cardDisplay.card.cardName;
            manaText.text = cardDisplay.manaCost.ToString();
            description.text = cardDisplay.card.description;
            CardParser.Instance.CheckKeyword(cardDisplay);


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
    public void UpdateTextOnCardWithCard(Card card)
    {
        if (card == null) return;

        UpdateMaterialOnCard(card);

        cardName.text = card.cardName;
        manaText.text = card.maxManaCost.ToString();
        description.text = card.description;      
    }

    private void ShowCardPlayableEffect(CardDisplay cardDisplay)
    {

        bool isTheRightChampionCard = true;
        if (cardDisplay.card.championCard)
        {
            CardTargeting cardTargeting = GetComponentInParent<CardTargeting>();
            if (cardDisplay.card.championCardType != GameState.Instance.playerChampion.champion.championCardType)
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
        switch (card.typeOfCard)
        {
            case CardType.Attack:
                nameBackground.SetActive(false);
                hpGameObject.SetActive(false);
                artworkMeshRenderer.material = attackCardMaterial;
                break;
            case CardType.Spell:
                nameBackground.SetActive(false);
                hpGameObject.SetActive(false);
                artworkMeshRenderer.material = spellCardMaterial;
                break;
            case CardType.Landmark:
                nameBackground.SetActive(true);
                hpGameObject.SetActive(true);
                Landmarks landmarkCard = (Landmarks)card;
                hpText.text = landmarkCard.minionHealth.ToString();
                artworkMeshRenderer.material = landmarkCardMaterial;
                break;
        }
    }
}
