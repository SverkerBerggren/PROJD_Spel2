using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LandmarkDisplay : Displays
{  
    public int health;
    //public Landmarks landmark;
	//public GameObject landmarkPrefab;
    private GameState gameState;
    private Graveyard graveyard;
    public int index;
    public bool opponentLandmarks = false;
    private GameObject landmarkPrefab;
    [NonSerialized] public bool landmarkEnabled = true;
    [NonSerialized] public CardDisplayAttributes cardDisplayAtributes;

    [SerializeField] private LandmarkDisplay previewLandmarkDisplay;
    private CardDisplayAttributes previewCardDisplayAtributes;

    private void Awake()
    {
        cardDisplayAtributes = transform.GetChild(0).GetComponent<CardDisplayAttributes>();
        previewCardDisplayAtributes = previewLandmarkDisplay.transform.GetChild(0).GetComponent<CardDisplayAttributes>();
        //cardDisplayAtributes.UpdateTextOnCard(this);
        landmarkPrefab = transform.GetChild(0).gameObject;

        
    }

    private void Start()
    {
        gameState = GameState.Instance;
        graveyard = Graveyard.Instance;



        if (previewLandmarkDisplay.gameObject.name.Equals(gameObject.name)) return;


        landmarkPrefab.SetActive(false);
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
        landmarkPrefab.SetActive(false);
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

    public void OnEnter()
    {
        if (card == null) return;
        previewLandmarkDisplay.gameObject.SetActive(true);
        previewLandmarkDisplay.card = card;
        previewLandmarkDisplay.manaCost = manaCost;
        previewLandmarkDisplay.health = health;

        previewCardDisplayAtributes.UpdateTextOnCard(previewLandmarkDisplay);
    }

    public void OnExit()
    {
        if (card == null) return;
        previewLandmarkDisplay.gameObject.SetActive(false);
        previewLandmarkDisplay.card = null;
        previewCardDisplayAtributes.UpdateTextOnCard(previewLandmarkDisplay);
    }

    public void UpdateTextOnCard()
    {
        landmarkPrefab.SetActive(true);
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


