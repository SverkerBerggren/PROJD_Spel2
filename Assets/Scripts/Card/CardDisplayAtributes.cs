using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDisplayAtributes : MonoBehaviour
{
    public TMP_Text cardName;
    public TMP_Text description;
    public TMP_Text manaText;

    [Header("CardMaterial")]
    public MeshRenderer artworkMeshRenderer;

    public Material attackCardMaterial;
    public Material spellCardMaterial;
    public Material landmarkCardMaterial;

    public GameObject cardPlayableEffect;

    public GameObject nameBackground;
    public GameObject hpGameObject;
    public TMP_Text hpText;

    public void UpdateTextOnCard(CardDisplay cardDisplay)
    {
        if (cardDisplay.card == null) return;

        if (!cardDisplay.opponentCard)
        {
            UpdateMaterialOnCard(cardDisplay.card);

            cardName.text = cardDisplay.card.cardName;
            manaText.text = cardDisplay.manaCost.ToString();
            description.text = cardDisplay.card.description;


            cardDisplay.UpdateVariables();
            CardParser.Instance.CheckKeyword(cardDisplay);


            if (cardPlayableEffect != null)
            {
                bool isTheRightChampionCard = true;
                if (cardDisplay.card.championCard)
                {
                    if (cardDisplay.card.championCardType != cardDisplay.cardTargeting.WhichChampionIsActive())
                    {
                        isTheRightChampionCard = false;
                    }
                }

                if (!isTheRightChampionCard) return;

                if (ActionOfPlayer.Instance.currentMana >= cardDisplay.manaCost && GameState.Instance.isItMyTurn)
                    cardPlayableEffect.SetActive(true);
                else
                    cardPlayableEffect.SetActive(false);
            }
        }
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
