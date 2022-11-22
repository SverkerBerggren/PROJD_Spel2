using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public SpriteRenderer artworkSpriteRenderer;

    public Card card;  

    [Header("CardAtributes")]
    public TMP_Text cardName;
    public TMP_Text description;
    public TMP_Text manaText;
    public int manaCost;

    [Header("CardMaterial")]
    public MeshRenderer artworkMeshRenderer;

    public Material attackCardMaterial;
    public Material spellCardMaterial;
    public Material landmarkCardMaterial;

    public GameObject cardPlayableEffect;

    public GameObject nameBackground;
    public GameObject hpGameObject;
    public TMP_Text hpText;

    public GameObject border;
    [System.NonSerialized] public bool opponentCard;
    public bool mouseDown = false;
    public bool alreadyBig = false;
    public Vector3 originalSize;

    private CardTargeting cardTargeting;
    private Calculations calculations;

    [System.NonSerialized] public int damageShow = 0;
    [System.NonSerialized] public int amountToHealShow = 0;
    [System.NonSerialized] public int amountToShieldShow = 0;
    [System.NonSerialized] public int amountOfCardsToDrawShow = 0;
    [System.NonSerialized] public int amountOfCardsToDiscardShow = 0;


    

    private void Start()
    {
        originalSize = transform.localScale;
        cardTargeting = GetComponent<CardTargeting>();
        calculations = Calculations.Instance;
    }

    public void UpdateTextOnCard()
    {
        if (card == null) return;
        
        
        if (!opponentCard)
        {
            UpdateMaterialOnCard();

            cardName.text = card.cardName;
            manaText.text = manaCost.ToString();
            description.text = card.description;

            
            UpdateVariables();
            CardParser.Instance.CheckKeyword(this);


            if (cardPlayableEffect != null)
            {
                bool isTheRightChampionCard = true;
                if (card.championCard)
                {
                    if (card.championCardType != cardTargeting.WhichChampionIsActive())
                    {
                        isTheRightChampionCard = false;
                    }
                }

                if (!isTheRightChampionCard) return;
                
                if (ActionOfPlayer.Instance.currentMana >= manaCost && GameState.Instance.isItMyTurn)
                    cardPlayableEffect.SetActive(true);
                else
                    cardPlayableEffect.SetActive(false);
            }
        }
            

        
       
        //manaText.text = card.manaCost.ToString();
    }

    private void UpdateVariables()
    {
        Calculations.Instance.CalculateHandManaCost(this);

        if (card.damage != 0)
            damageShow = calculations.CalculateDamage(card.damage);
        if (card.amountToHeal != 0)
            amountToHealShow = calculations.CalculateHealing(card.amountToHeal);
        if (card.amountToShield != 0)
            amountToShieldShow = calculations.CalculateShield(card.amountToShield);
        if (card.amountOfCardsToDraw != 0)
            amountOfCardsToDrawShow = card.amountOfCardsToDraw;
        if (card.amountOfCardsToDiscard != 0)
            amountOfCardsToDiscardShow = card.amountOfCardsToDiscard;
    }

    private void UpdateMaterialOnCard()
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

    public void ResetSize()
    {
        transform.localScale = originalSize;
    }

    private void OnMouseEnter()
    {
        if (opponentCard) return;
       
        if (!alreadyBig)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 7, transform.position.z - 1);
            transform.localScale = new Vector3(transform.localScale.x + 0.5f, transform.localScale.x + 0.5f, transform.localScale.x + 0.5f);
            alreadyBig = true;
        }
    }
    private void OnMouseExit()
    {
        if (opponentCard) return;
        if (!mouseDown)
        {
            alreadyBig = false;
            transform.position = new Vector3(transform.position.x, transform.position.y - 7, transform.position.z + 1);
            ResetSize();
        }

    }
}
