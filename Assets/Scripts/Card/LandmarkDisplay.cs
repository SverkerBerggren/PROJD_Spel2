using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LandmarkDisplay : Displays
{  
    public int health;
    private Landmarks landmark;
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
        landmark = (Landmarks)card;
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
        }
        else
        {
            graveyard.AddCardToGraveyard(card);
        }

        landmark.LandmarkEffectTakeBack();
        landmark.WhenLandmarksDie();
        card = null;
        landmark = null;
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

    public void UpdateTextOnCard()
    {
        cardDisplayAtributes = transform.GetChild(0).GetComponent<CardDisplayAtributes>();
        cardDisplayAtributes.UpdateTextOnCard(this);
    }
}


