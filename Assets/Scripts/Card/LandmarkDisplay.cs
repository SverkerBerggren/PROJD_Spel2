using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LandmarkDisplay : Displays
{  
    public int health;
    public Landmarks landmark;
	public GameObject landmarkPrefab;

    public bool occultGathering = false;
    [NonSerialized] public int tenExtraDamage;
    private GameState gameState;
    private Graveyard graveyard;
    public int index;
    public bool opponentLandmarks = false;

    [SerializeField] private LandmarkDisplay previewLandmarkDisplay;
    private CardDisplayAtributes previewCardDisplayAtributes;

    private void Awake()
    {
        cardDisplayAtributes = transform.GetChild(0).GetComponent<CardDisplayAtributes>();
        previewCardDisplayAtributes = previewLandmarkDisplay.transform.GetChild(0).GetComponent<CardDisplayAtributes>();
    }

    private void Start()
    {
        gameState = GameState.Instance;
        graveyard = Graveyard.Instance;
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
        if (landmark == null) return;
        previewLandmarkDisplay.gameObject.SetActive(true);
        previewLandmarkDisplay.card = card;
        previewLandmarkDisplay.landmark = landmark;
        previewLandmarkDisplay.manaCost = manaCost;
        previewLandmarkDisplay.health = health;

        previewCardDisplayAtributes.UpdateTextOnCard(previewLandmarkDisplay);
    }

    private void OnMouseExit()
    {
        if (landmark == null) return;
        previewLandmarkDisplay.gameObject.SetActive(false);
        previewLandmarkDisplay.card = null;
        previewLandmarkDisplay.landmark = null;
        previewCardDisplayAtributes.UpdateTextOnCard(previewLandmarkDisplay);
    }

    public void UpdateTextOnCard()
    {
        cardDisplayAtributes.UpdateTextOnCard(this);
    }
}


