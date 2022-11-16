using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LandmarkDisplay : MonoBehaviour
{
    public Landmarks card;
    
    public int health;
    public TMP_Text healthText;
    public TMP_Text descriptionText;
    public TMP_Text nameText;
    public TMP_Text manaText;
	public int manaCost;

	public GameObject landmarkPrefab;

    public bool occultGathering = false;
    [NonSerialized] public int tenExtraDamage;
    private GameState gameState;
    private Graveyard graveyard;
    public int index;
    public bool opponentLandmarks = false;

    private GameObject previewLandmark;
    private LandmarkDisplay previewLandmarkDisplay;

    private void Start()
    {
        gameState = GameState.Instance;
        graveyard = Graveyard.Instance;

        previewLandmark = transform.parent.GetChild(4).gameObject;
        previewLandmarkDisplay = previewLandmark.GetComponent<LandmarkDisplay>();
    }

    private void UpdateTextOnCard()
    {
        if (card == null)
        {
            landmarkPrefab.SetActive(false);
            return;
        }
        landmarkPrefab.SetActive(true);
        healthText.text = health.ToString();
        descriptionText.text = card.description;
        manaText.text = manaCost.ToString();
        nameText.text = card.cardName;
    }

    public void DestroyLandmark()
    {
        LandmarkDead();

        if (gameState.isOnline)
		{
			TargetInfo targetInfo = new TargetInfo();
			ListEnum listEnum = new ListEnum();

            if (opponentLandmarks)
                listEnum.opponentLandmarks = true;
            else
                listEnum.myLandmarks = true;

			targetInfo.whichList = listEnum;

			targetInfo.index = index;

			List<TargetInfo> targetInfoList = new List<TargetInfo>();
			targetInfoList.Add(targetInfo);

			RequestDestroyLandmark request = new RequestDestroyLandmark(targetInfoList);
            request.whichPlayer = ClientConnection.Instance.playerId;
			ClientConnection.Instance.AddRequest(request, gameState.RequestEmpty);
		}
	}

    private void LandmarkDead()
    {
        if (opponentLandmarks)
        {
            graveyard.AddCardToGraveyardOpponent(card);
            gameState.opponentChampion.champion.WhenLandmarksDie();

        }
        else
        {
            gameState.playerChampion.champion.WhenLandmarksDie();
            graveyard.AddCardToGraveyard(card);
        }
        if (!opponentLandmarks)
            gameState.playerChampion.champion.WhenLandmarksDie();
        card.LandmarkEffectTakeBack();
        card.WhenLandmarksDie();
        card = null;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            LandmarkDead();                     
        }
    }

    private void OnMouseEnter()
    {
        if (card == null) return;
        previewLandmark.SetActive(true);
        previewLandmarkDisplay.card = card;
        previewLandmarkDisplay.manaCost = manaCost;
        previewLandmarkDisplay.health = health;
    }

    private void OnMouseExit()
    {
        if (card == null) return;
        previewLandmark.SetActive(false);
        previewLandmarkDisplay.card = null;
    }

    private void FixedUpdate()
    {
        if (gameState.amountOfTurns == 10)
        {
            if (card != null)
            {
                if (card.cardName.Equals("Mysterious Forest"))
                {
                    DestroyLandmark();
                    gameState.DrawCard(5, null);
                }
            }
            
        }
        UpdateTextOnCard();
    }
}


