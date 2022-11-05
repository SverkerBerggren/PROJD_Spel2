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
    private GameState gameState;

    private GameObject gameObjectHit;
    

    void Start()
    {
        graveyard = Graveyard.Instance;
        actionOfPlayer = ActionOfPlayer.Instance;
        gameState = GameState.Instance;
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

        if (gameState.isOnline)
        {
            if (!gameState.isItMyTurn)
            {
                CardGoBackToStartingPosition();
                return;
            }
        }

        gameState.targetingEffect.SetActive(false);

        if (cardDisplay.opponentCard == true) return;

        //Quaternion angle = Quaternion.AxisAngle()

        GameObject gO = GameObject.Find("Platform");

        RaycastHit[] hitEnemy;
        hitEnemy = Physics.RaycastAll(mousePosition, Vector3.forward * 100 + Vector3.down * 55, 200f);
        Debug.DrawRay(mousePosition, Vector3.forward * 100 + Vector3.down * 55, Color.red, 100f);

        for (int i = 0; i < hitEnemy.Length;i++)
        {
            gameObjectHit = hitEnemy[i].collider.gameObject;

            if (gameObjectHit == null || gameObjectHit.tag.Equals("DeckAndGraveyard") && actionOfPlayer.CheckIfCanPlayCard(card))
            {
                CardGoBackToStartingPosition();
                return;
            }

            gameState.ShowPlayedCard(card);

            if (card.targetable)          
                PlayedATargetableCard();
           
            if (!card.targetable && hitEnemy[i].collider.tag.Equals("NonTargetCollider"))
            {
                PlayedAnUntargetableCard();
            }   
        }       
    }

    private void PlayedAnUntargetableCard()
    {
        if (card.typeOfCard == CardType.Landmark)
        {
            int amountOfLandmarksAlreadyInUse = 0;
            foreach (LandmarkDisplay landmarkDisplay in GameState.Instance.playerLandmarks)
            {
                if (landmarkDisplay.card == null)
                {
                    PlaceLandmark(landmarkDisplay);
                    break;
                }

                else
                    amountOfLandmarksAlreadyInUse++;
            }
            if (amountOfLandmarksAlreadyInUse == 4)
                CardGoBackToStartingPosition();
        }

        else if (card.typeOfCard == CardType.Spell)
        {
            Graveyard.Instance.AddCardToGraveyard(card);
            card.PlayCard();
            cardDisplay.card = null;
        }
    }

    private void PlayedATargetableCard()
    {
        // Should indicate the TauntLandmark so its more obvious
        if (actionOfPlayer.tauntPlaced > 0)
        {
            if (gameObjectHit.tag.Equals("TauntCard"))
            {
                card.LandmarkTarget = gameObjectHit.GetComponent<LandmarkDisplay>();
                card.PlayCard();
                graveyard.AddCardToGraveyard(card);
                cardDisplay.card = null;
            }
            CardGoBackToStartingPosition();
            return;
        }

        if (gameObjectHit.tag.Equals("Champion"))
        {
            card.Target = gameObjectHit.GetComponent<AvailableChampion>().champion;

            Graveyard.Instance.AddCardToGraveyard(card);
            card.PlayCard();
            cardDisplay.card = null;
        }

        else if (gameObjectHit.tag.Equals("LandmarkSlot"))
        {
            card.LandmarkTarget = gameObjectHit.GetComponent<LandmarkDisplay>();
            Graveyard.Instance.AddCardToGraveyard(card);
            card.PlayCard();
            cardDisplay.card = null;
        }
    }

    private void CardGoBackToStartingPosition()
    {
        gameObjectRectTransform.anchoredPosition = startposition;
        print(gameObjectRectTransform.anchoredPosition);
    }

    private void PlaceLandmark(LandmarkDisplay landmarkSlot)
    {
        cardDisplay.card = null;
        Landmarks landmark = (Landmarks)card;
        GameState.Instance.LandmarkPlaced(landmarkSlot.index, landmark, false);


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


}
