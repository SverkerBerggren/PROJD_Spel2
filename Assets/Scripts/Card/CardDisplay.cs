using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [System.NonSerialized] public SpriteRenderer artworkSpriteRenderer;

    public Card card;  

    [Header("CardAtributes")]
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

    public Sprite backfaceSprite;

    public GameObject border;
    [System.NonSerialized] public bool opponentCard;

    private void Start()
    {
        artworkSpriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    private void UpdateTextOnCard()
    {
        if (card == null) return;
        
        if (opponentCard)
        {

        }
        
        if (!opponentCard)
        {
            UpdateMaterialOnCard();

            cardName.text = card.cardName;
            manaText.text = card.manaCost.ToString();
            description.text = card.description;

            if (ActionOfPlayer.Instance.currentMana >= card.manaCost)
                cardPlayableEffect.SetActive(true);
            else
                cardPlayableEffect.SetActive(false);
        }
            

        
       
        //manaText.text = card.manaCost.ToString();
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


    private void FixedUpdate()
    {
        UpdateTextOnCard();
    }

    private void OnMouseEnter()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1); 
    }
    private void OnMouseExit()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
    }
}
