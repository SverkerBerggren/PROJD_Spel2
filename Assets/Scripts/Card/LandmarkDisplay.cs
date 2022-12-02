using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LandmarkDisplay : Displays
{  
    public int health;
    //public Landmarks landmark;
	public GameObject landmarkPrefab;
    private GameState gameState;
    private Graveyard graveyard;
    public int index;
    public bool opponentLandmarks = false;
    [NonSerialized] public bool landmarkEnabled = true;

    [SerializeField] private LandmarkDisplay previewLandmarkDisplay;
    private CardDisplayAtributes previewCardDisplayAtributes;

    private void Awake()
    {
        cardDisplayAtributes = transform.GetChild(0).GetComponent<CardDisplayAtributes>();
        previewCardDisplayAtributes = previewLandmarkDisplay.transform.GetChild(0).GetComponent<CardDisplayAtributes>();
        cardDisplayAtributes.UpdateTextOnCard(this);
    }

    private void Start()
    {
        gameState = GameState.Instance;
        graveyard = Graveyard.Instance;
    }

    private void FixedUpdate()
    {
        if (card != null)
        {
            landmarkPrefab.SetActive(true);
        }
    }

    private void OnEnable()
    {
        if (card != null)
            gameState.Refresh();
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
        Landmarks landmark = (Landmarks)card;
        landmark.WhenLandmarksDie();
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
        previewLandmarkDisplay.gameObject.SetActive(true);
        previewLandmarkDisplay.card = card;
        previewLandmarkDisplay.manaCost = manaCost;
        previewLandmarkDisplay.health = health;

        previewCardDisplayAtributes.UpdateTextOnCard(previewLandmarkDisplay);
    }

    private void OnMouseExit()
    {
        if (card == null) return;
        previewLandmarkDisplay.gameObject.SetActive(false);
        previewLandmarkDisplay.card = null;
        previewCardDisplayAtributes.UpdateTextOnCard(previewLandmarkDisplay);
    }

    public void UpdateTextOnCard()
    {
        cardDisplayAtributes.UpdateTextOnCard(this);
    }

    public void DisableLandmark()
    {
        landmarkEnabled = false;
    }

    public void EnableLandmark()
    {
        landmarkEnabled = true;
    }
}


