using UnityEngine;

public class PlayCardManager : MonoBehaviour
{
    private ActionOfPlayer actionOfPlayer;
    private CardDisplay cardDisplay;
    private Card card;
    private Graveyard graveyard;
    private GameState gameState;

    private static PlayCardManager instance;
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

    private TypeOfCardTargeting CheckTarget(GameObject target)
    {
		if (TauntCard())
            return TypeOfCardTargeting.Taunt;

		else if (target.TryGetComponent(out AvailableChampion availableChampion))
		{
			if (actionOfPlayer.CheckIfCanPlayCard(cardDisplay, true))
                return TypeOfCardTargeting.Targeted;
        }
		else if (target.TryGetComponent(out LandmarkDisplay landmarkDisplay))
		{
			if (landmarkDisplay.Card != null && actionOfPlayer.CheckIfCanPlayCard(cardDisplay, true))
				return TypeOfCardTargeting.Targeted;
		}
		return TypeOfCardTargeting.None;
	}

    private void PlaceLandmark(LandmarkDisplay landmarkSlot)
    {
        GameState.Instance.AddCardToPlayedCardsThisTurn(cardDisplay.Card);
        Landmarks landmark = (Landmarks)card;
        GameState.Instance.LandmarkPlaced(landmarkSlot.Index, landmark, false);

		if (GameState.Instance.IsOnline)
        {
            RequestPlayLandmark request = new RequestPlayLandmark();
            request.whichPlayer = ClientConnection.Instance.playerId;

            CardAndPlacement cardAndPlacement = new CardAndPlacement();
            cardAndPlacement.CardName = landmark.CardName;

            TargetInfo targetInfo = new TargetInfo();
            targetInfo.index = landmarkSlot.Index;
            ListEnum listEnum = new ListEnum();
            listEnum.myLandmarks = true;
            targetInfo.whichList = listEnum;

            cardAndPlacement.Placement = targetInfo;

            request.landmarkToPlace = cardAndPlacement;

            ClientConnection.Instance.AddRequest(request, GameState.Instance.RequestEmpty);
        }
    }

	public TypeOfCardTargeting CheckIfHitAnEnemy(GameObject target)
	{
		if (target == null || target.CompareTag("DeckAndGraveyard"))
			return TypeOfCardTargeting.None;

		if (card.Targetable)
			return CheckTarget(target);

		else if (!card.Targetable && target.CompareTag("NonTargetCollider"))
		{
			if (actionOfPlayer.CheckIfCanPlayCard(cardDisplay, true))
				return TypeOfCardTargeting.UnTargeted;
		}
		return TypeOfCardTargeting.None;
	}

	public void PlayedAnUntargetableCard()
    {
        if (card.TypeOfCard == CardType.Landmark)
        {
            int amountOfLandmarksAlreadyInUse = 0;
            foreach (LandmarkDisplay landmarkDisplay in GameState.Instance.PlayerLandmarks)
            {
                if (landmarkDisplay.Card == null)
                {
                    PlaceLandmark(landmarkDisplay);
                    card.PlayCard();
					actionOfPlayer.ChangeCardOrder(true, cardDisplay);
					Landmarks landmark = (Landmarks)landmarkDisplay.Card;
                    break;
                }
                else
                    amountOfLandmarksAlreadyInUse++;
            }
            if (amountOfLandmarksAlreadyInUse == 4) return;
        }

        else if (card.TypeOfCard == CardType.Spell || card.TypeOfCard == CardType.Attack)
        {
            Graveyard.Instance.AddCardToGraveyard(card);
            card.PlayCard();
			actionOfPlayer.ChangeCardOrder(true, cardDisplay);
		}
    }

    public void PlayedATargetableCard(GameObject gameObjectTargeted)
    {
        if (gameObjectTargeted.TryGetComponent(out AvailableChampion availableChampion))
        {
            card.Target = availableChampion.Champion;
        }
        else if (gameObjectTargeted.TryGetComponent(out LandmarkDisplay landmarkDisplay))
        {
            if (landmarkDisplay.Card == null) return;

            card.LandmarkTarget = landmarkDisplay;
        }
        Graveyard.Instance.AddCardToGraveyard(card);
        card.PlayCard();
        actionOfPlayer.ChangeCardOrder(true, cardDisplay);                   
    }

	public bool TauntCard()
	{
		// Should indicate the TauntLandmark so its more obvious
		if (card.TypeOfCard != CardType.Attack) return false;

		foreach (LandmarkDisplay landmarkDisplay in gameState.OpponentLandmarks)
		{
			if (landmarkDisplay.Card == null) continue;

			if (landmarkDisplay.Card is TauntLandmark && actionOfPlayer.CheckIfCanPlayCard(cardDisplay, true))
			{
				card.Target = null;
				card.LandmarkTarget = landmarkDisplay;
				card.PlayCard();
				graveyard.AddCardToGraveyard(card);
				actionOfPlayer.ChangeCardOrder(true, cardDisplay);
				return true;
			}

		}
		return false;
	}

	public bool CanCardBePlayed(CardDisplay cardDisplay)
	{
		this.cardDisplay = cardDisplay;
		card = cardDisplay.Card;
		if (gameState.IsOnline)
		{
			if (!gameState.IsItMyTurn || !gameState.HasPriority)
				return false;
		}

		if (cardDisplay.OpponentCard)
			return false;

		// Checking if the card used is a champion card
		if (card.ChampionCardType != ChampionCardType.All && card.ChampionCard)
		{
			if (gameState.PlayerChampion.Champion.ChampionCardType != card.ChampionCardType)
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
