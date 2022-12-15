using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayCardManager : MonoBehaviour
{
    private static PlayCardManager instance;

    private ActionOfPlayer actionOfPlayer;
    private CardDisplay cardDisplay;
    private Card card;
    private Graveyard graveyard;
    private GameState gameState;

    public static PlayCardManager Instance { get { return instance; } set { instance = value; }  }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        graveyard = Graveyard.Instance;
        actionOfPlayer = ActionOfPlayer.Instance;
        gameState = GameState.Instance;
    }

    public bool CanCardBePlayed(CardDisplay cardDisplay)
    {
        this.cardDisplay = cardDisplay;
        card = cardDisplay.card;
        if (gameState.isOnline)
        {
            if (!gameState.isItMyTurn || !gameState.hasPriority)
                return false;
        }

        if (cardDisplay.opponentCard == true)
            return false;

        // Checking if the card used is a champion card
        if (card.championCardType != ChampionCardType.All && card.championCard)
        {
            if (gameState.playerChampion.champion.championCardType != card.championCardType)
                return false;
        }

        return true;
    }

    public void PlayCard(TypeOfCardTargeting typeOfcardTargeting, GameObject target)
    {          
        switch (typeOfcardTargeting)
        {
            case TypeOfCardTargeting.Targeted:
                PlayedATargetableCard(target);
                break;
            case TypeOfCardTargeting.UnTargeted:
                PlayedAnUntargetableCard();
                break;
            default:

                break;
        }
    }

    public TypeOfCardTargeting CheckIfHitAnEnemy(GameObject target)
    {
        if (target == null || target.CompareTag("DeckAndGraveyard")) 
            return TypeOfCardTargeting.None;

        if (card.targetable && (target.CompareTag("Champion") || target.CompareTag("LandmarkSlot")))
        {       
            if (TauntCard()) 
                return TypeOfCardTargeting.Taunt;

            if (actionOfPlayer.CheckIfCanPlayCard(cardDisplay, true))
                return TypeOfCardTargeting.Targeted;

        }
        else if (!card.targetable && target.CompareTag("NonTargetCollider"))
        {
            if (actionOfPlayer.CheckIfCanPlayCard(cardDisplay, true)) 
                return TypeOfCardTargeting.UnTargeted;
        }
        return TypeOfCardTargeting.None;
    }

    public bool TauntCard()
    {
        // Should indicate the TauntLandmark so its more obvious
        if (card.typeOfCard != CardType.Attack) return false;

        foreach (LandmarkDisplay landmarkDisplay in gameState.opponentLandmarks)
        {
            if (landmarkDisplay.card == null) continue;

            if (landmarkDisplay.card is TauntLandmark && actionOfPlayer.CheckIfCanPlayCard(cardDisplay, true))
            {
                print("LandmarkTAUNT");
                card.Target = null;
                card.LandmarkTarget = landmarkDisplay;
                gameState.ShowPlayedCard(card, false, -1);
                card.PlayCard();
                graveyard.AddCardToGraveyard(card);
                gameState.AddCardToPlayedCardsThisTurn(cardDisplay);
                return true;
            }

        }
       // CardGoBackToStartingPosition();
        return false;
    }

    public void PlayedAnUntargetableCard()
    {
        if (card.typeOfCard == CardType.Landmark)
        {
            int amountOfLandmarksAlreadyInUse = 0;
            foreach (LandmarkDisplay landmarkDisplay in GameState.Instance.playerLandmarks)
            {
                if (landmarkDisplay.card == null)
                {
                    PlaceLandmark(landmarkDisplay);
                    gameState.ShowPlayedCard(card, false, -1);
                    card.PlayCard();
                    Landmarks landmark = (Landmarks)landmarkDisplay.card;
                    break;
                }
                else
                    amountOfLandmarksAlreadyInUse++;
            }
            if (amountOfLandmarksAlreadyInUse == 4)
            {
                //CardGoBackToStartingPosition();
                return;
            }
        }

        else if (card.typeOfCard == CardType.Spell || card.typeOfCard == CardType.Attack)
        {
            Graveyard.Instance.AddCardToGraveyard(card);
            gameState.ShowPlayedCard(card, false, -1);
            card.PlayCard();
            gameState.AddCardToPlayedCardsThisTurn(cardDisplay);
        }
    }

    public void PlayedATargetableCard(GameObject gameObjectTargeted)
    {
        if (gameObjectTargeted.CompareTag("Champion"))
        {
            card.Target = gameObjectTargeted.GetComponent<AvailableChampion>().champion;

            Graveyard.Instance.AddCardToGraveyard(card);
            gameState.ShowPlayedCard(card, false, -1);
            card.PlayCard();
            gameState.AddCardToPlayedCardsThisTurn(cardDisplay);
        }

        else if (gameObjectTargeted.CompareTag("LandmarkSlot") && gameObjectTargeted.GetComponent<LandmarkDisplay>().card != null)
        {
            card.LandmarkTarget = gameObjectTargeted.GetComponent<LandmarkDisplay>();
            Graveyard.Instance.AddCardToGraveyard(card);
            gameState.ShowPlayedCard(card, false, -1);
            card.PlayCard();
            gameState.AddCardToPlayedCardsThisTurn(cardDisplay);
        }
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
public enum TypeOfCardTargeting
{
    Taunt,
    Targeted,
    UnTargeted,
    None
}
