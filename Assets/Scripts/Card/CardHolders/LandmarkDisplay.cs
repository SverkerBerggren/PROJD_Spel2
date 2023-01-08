using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LandmarkDisplay : Displays
{  
    private GameState gameState;
    private Graveyard graveyard;
    private CardDisplayAttributes previewCardDisplayAtributes;
    private GameObject landmarkPrefab;

    [SerializeField] private LandmarkDisplay previewLandmarkDisplay;

    [NonSerialized] public bool LandmarkEnabled = true;
    [NonSerialized] public CardDisplayAttributes CardDisplayAtributes;

    public int Health;
    public int Index;
    public bool OpponentLandmarks = false;


    private void Awake()
    {
        CardDisplayAtributes = transform.GetChild(0).GetComponent<CardDisplayAttributes>();
        previewCardDisplayAtributes = previewLandmarkDisplay.transform.GetChild(0).GetComponent<CardDisplayAttributes>();
        landmarkPrefab = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        gameState = GameState.Instance;
        graveyard = Graveyard.Instance;

        if (previewLandmarkDisplay.gameObject.name.Equals(gameObject.name)) return;

        landmarkPrefab.SetActive(false);
    }

    private void LandmarkDead()
    {
        if (OpponentLandmarks)
        {
            graveyard.AddCardToGraveyardOpponent(Card);
        }
        else
        {
            graveyard.AddCardToGraveyard(Card);
        }
        Landmarks landmark = (Landmarks)Card;
        landmark.WhenLandmarksDie();
        landmarkPrefab.SetActive(false);
        Card = null;
    }

    public void DestroyLandmark()
    {
        LandmarkDead();

        if (gameState.IsOnline)
		{
			TargetInfo targetInfo = new TargetInfo();
			ListEnum listEnum = new ListEnum();

            if (OpponentLandmarks)
                listEnum.opponentLandmarks = true;
            else
                listEnum.myLandmarks = true;

			targetInfo.whichList = listEnum;

			targetInfo.index = Index;

			List<TargetInfo> targetInfoList = new List<TargetInfo>();
			targetInfoList.Add(targetInfo);

			RequestDestroyLandmark request = new RequestDestroyLandmark(targetInfoList);
            request.whichPlayer = ClientConnection.Instance.playerId;
			ClientConnection.Instance.AddRequest(request, gameState.RequestEmpty);
		}
	}

    public void TakeDamage(int amount)
    {
        Health -= amount;

        if (Health <= 0)
        {
            LandmarkDead();                     
        }
    }

    public void OnEnter()
    {
        if (Card == null) return;
        previewLandmarkDisplay.gameObject.SetActive(true);
        previewLandmarkDisplay.Card = Card;
        previewLandmarkDisplay.ManaCost = ManaCost;
        previewLandmarkDisplay.Health = Health;

        previewCardDisplayAtributes.UpdateTextOnCard(previewLandmarkDisplay);
    }

    public void OnExit()
    {
        if (Card == null) return;
        previewLandmarkDisplay.gameObject.SetActive(false);
        previewLandmarkDisplay.Card = null;
        previewCardDisplayAtributes.UpdateTextOnCard(previewLandmarkDisplay);
    }

    public void UpdateTextOnCard()
    {
        landmarkPrefab.SetActive(true);
        CardDisplayAtributes.UpdateTextOnCard(this);
    }

    public void DisableLandmark()
    {
        LandmarkEnabled = false;
    }

    public void EnableLandmark()
    {
        LandmarkEnabled = true;
    }
}


