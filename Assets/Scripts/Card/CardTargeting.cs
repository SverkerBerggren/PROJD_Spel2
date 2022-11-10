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

        if (!gameObject.CompareTag("LandmarkSlot"))
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
        cardDisplay.mouseDown = false;


        if (gameState.isOnline)
        {
            if (!gameState.isItMyTurn || !gameState.hasPriority)
            {
                CardGoBackToStartingPosition();
                return;
            }
        }

        if (cardDisplay.opponentCard == true) return;

        RaycastHit[] hitEnemy;
        hitEnemy = Physics.RaycastAll(mousePosition, Vector3.forward * 100 + Vector3.down * 55, 200f);
        Debug.DrawRay(mousePosition, Vector3.forward * 100 + Vector3.down * 55, Color.red, 100f);

        for (int i = 0; i < hitEnemy.Length;i++)
        {
            gameObjectHit = hitEnemy[i].collider.gameObject;

            if (gameObjectHit == null || gameObjectHit.CompareTag("DeckAndGraveyard")) break;
            
            if (card.targetable && (gameObjectHit.CompareTag("Champion") || gameObjectHit.CompareTag("LandmarkSlot")))
            {
                if (TauntCard()) break;
                
                if (actionOfPlayer.CheckIfCanPlayCard(cardDisplay))
                     PlayedATargetableCard();
            }           
            else if (!card.targetable && gameObjectHit.CompareTag("NonTargetCollider"))
            {
                if (actionOfPlayer.CheckIfCanPlayCard(cardDisplay))
                    PlayedAnUntargetableCard();
            }   
        }
        CardGoBackToStartingPosition();
    }

    private bool TauntCard()
    {
        // Should indicate the TauntLandmark so its more obvious
        foreach (LandmarkDisplay landmarkDisplay in gameState.opponentLandmarks)
        {
            if (landmarkDisplay.card == null) continue;

            if (landmarkDisplay.card.tag.Equals("TauntLandmark") && actionOfPlayer.CheckIfCanPlayCard(cardDisplay))
            {
                print("LandmarkTAUNT");
                card.Target = null;
                card.LandmarkTarget = landmarkDisplay;
                card.PlayCard();
                gameState.ShowPlayedCard(card);
                graveyard.AddCardToGraveyard(card);
                gameState.AddCardToPlayedCardsThisTurn(cardDisplay);
                return true;
            }
            
        }
        CardGoBackToStartingPosition();
        return false;
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
                    card.PlayCard();
                    gameState.ShowPlayedCardLandmark(landmarkDisplay.card);                    
                    break;
                }
                else
                    amountOfLandmarksAlreadyInUse++;
            }
            if (amountOfLandmarksAlreadyInUse == 4)
            {
                CardGoBackToStartingPosition();
                return;
            }
        }

        else if (card.typeOfCard == CardType.Spell || card.typeOfCard == CardType.Attack)
        {
            Graveyard.Instance.AddCardToGraveyard(card);
            card.PlayCard();
            gameState.ShowPlayedCard(card);
            gameState.AddCardToPlayedCardsThisTurn(cardDisplay);
        }
    }

    private void PlayedATargetableCard()
    {



        if (gameObjectHit.CompareTag("Champion"))
        {
            card.Target = gameObjectHit.GetComponent<AvailableChampion>().champion;

            Graveyard.Instance.AddCardToGraveyard(card);
            card.PlayCard();
            gameState.ShowPlayedCard(card);
            gameState.AddCardToPlayedCardsThisTurn(cardDisplay);
        }

        else if (gameObjectHit.CompareTag("LandmarkSlot") && gameObjectHit.GetComponent<LandmarkDisplay>().card != null)
        {
            card.LandmarkTarget = gameObjectHit.GetComponent<LandmarkDisplay>();
            Graveyard.Instance.AddCardToGraveyard(card);
            card.PlayCard();
            gameState.ShowPlayedCard(card);
            gameState.AddCardToPlayedCardsThisTurn(cardDisplay);
        }
    }

    private void CardGoBackToStartingPosition()
    {
        gameObjectRectTransform.anchoredPosition = startposition;
        cardDisplay.ResetSize();
    }

    private void PlaceLandmark(LandmarkDisplay landmarkSlot)
    {
        GameState.Instance.AddCardToPlayedCardsThisTurn(cardDisplay);
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
