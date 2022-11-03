using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTargeting : MonoBehaviour
{
    private Vector3 startposition;
    private RectTransform gameObjectRectTransform;
    private ActionOfPlayer actionOfPlayer;

    private CardDisplay cardDisplay;
    private CardMovement cardMovement;
    private Vector3 mousePosition;
    private Card card;
    private Graveyard graveyard;


    private GameObject gameObjectHit;
    

    void Start()
    {
        graveyard = Graveyard.Instance;
        actionOfPlayer = ActionOfPlayer.Instance;
        cardDisplay = GetComponent<CardDisplay>();       

        if (!gameObject.tag.Equals("LandmarkSlot"))
        {
                gameObjectRectTransform = GetComponent<RectTransform>();           
                startposition = gameObjectRectTransform.anchoredPosition;
        }
    }

    private void OnMouseUp()
    {
        cardMovement = GetComponent<CardMovement>();
        mousePosition = cardMovement.mousePosition;
        card = cardDisplay.card;

        if (GameState.Instance.isOnline)
        {
            if (!GameState.Instance.isItMyTurn)
            {
                CardGoBackToStartingPosition();
                return;
            }
        }
            

        if (cardDisplay.opponentCard == true) return;

        //Quaternion angle = Quaternion.AxisAngle()

        GameObject gO = GameObject.Find("Platform");

        RaycastHit hitEnemy;
        Physics.Raycast(mousePosition, Vector3.forward * 100 + Vector3.down * 55, out hitEnemy, 200f);
        Debug.DrawRay(mousePosition, Vector3.forward * 100 + Vector3.down * 55, Color.red, 100f);


        if (hitEnemy.collider == null)
        {
            print("RUNS");
            CardGoBackToStartingPosition();
            return;
        }
        
        print(hitEnemy.collider.gameObject.name);

        gameObjectHit = hitEnemy.transform.gameObject;


        if (actionOfPlayer.CheckIfCanPlayCard(card))
        {

            GameState.Instance.ShowPlayedCard(card);
            WhatToDoWhenTargeted();
            print(gameObjectHit.name);
        }
        else
            CardGoBackToStartingPosition();
    }
    private void CardGoBackToStartingPosition()
    {
        gameObjectRectTransform.anchoredPosition = startposition;
        print(gameObjectRectTransform.anchoredPosition);
    }

    private void WhatToDoWhenTargeted()
    {
        if (actionOfPlayer.tauntPlaced > 0)
        {
            if (gameObjectHit.tag.Equals("TauntCard"))
            {
                card.LandmarkTarget = gameObjectHit.GetComponent<LandmarkDisplay>();
                card.PlayCard();
                graveyard.AddCardToGraveyard(card);
                cardDisplay.card = null;                
            }
            return;
        }

        switch (gameObjectHit.tag)
        {
            case "Champion":
                card.Target = gameObjectHit.GetComponent<AvailableChampion>().champion;
                 
                Graveyard.Instance.AddCardToGraveyard(card);
                card.PlayCard();
                cardDisplay.card = null;
                
                break;
            case "LandmarkSlot":
                WhatToDoWhenLandmarkSlotTargeted();
                break;
        }
        
    }

    private void WhatToDoWhenLandmarkSlotTargeted()
    {
        LandmarkDisplay landmarkSlot = gameObjectHit.GetComponent<LandmarkDisplay>();
        Landmarks landmark = null;

        switch (card.tag)
        {
            case "DestroyLandmark":
                card.PlayCard();
                graveyard.AddCardToGraveyard(card);
                cardDisplay.card = null;
                return;
            case "AttackSpell":
                card.LandmarkTarget = landmarkSlot;
                Graveyard.Instance.AddCardToGraveyard(card);
                card.PlayCard();
                cardDisplay.card = null;
                break;
            case "Spell":
                card.LandmarkTarget = landmarkSlot;

                Graveyard.Instance.AddCardToGraveyard(card);
                card.PlayCard();
                cardDisplay.card = null;
                return;
        }


        if (card.typeOfCard == CardType.Landmark)
        {         
            GameState.Instance.LandmarkPlaced(landmarkSlot.index, landmark,false);


            if (GameState.Instance.isOnline)
            {
                RequestPlayLandmark request = new RequestPlayLandmark();
                request.whichPlayer = ClientConnection.Instance.playerId;

                CardAndPlacement cardAndPlacement = new CardAndPlacement();
                cardAndPlacement.cardName = landmark.cardName;

                TargetInfo targetInfo = new TargetInfo();
                targetInfo.index = landmarkSlot.index;
                ListEnum listEnum = new ListEnum();
                listEnum.myLandmarks = true;
                targetInfo.whichList = listEnum;

                cardAndPlacement.placement = targetInfo;

                request.landmarkToPlace = cardAndPlacement;

                ClientConnection.Instance.AddRequest(request, GameState.Instance.RequestEmpty);
            }
        }
        cardDisplay.card = null;

    }


}
