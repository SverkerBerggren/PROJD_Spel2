using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/DuelistLandmark")]
public class DuelistLandmark : Landmarks
{
    public DuelistLandmark(DuelistLandmark card) : base(card.MinionHealth, card.CardName, card.Description, card.MaxManaCost, card.Damage, card.AmountToHeal, card.AmountToShield)
    {
        ChampionCard = true;
        ChampionCardType = ChampionCardType.Duelist;
    }

    public override void PlaceLandmark()
    {
        GameState gameState = GameState.Instance;
        if (gameState.isOnline)
        {
            RequestStopSwapping stopSwapRequest = new RequestStopSwapping(false);
            stopSwapRequest.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(stopSwapRequest, gameState.RequestEmpty);
        }
    }

    public override void WhenLandmarksDie()
    {
        GameState gameState = GameState.Instance;
        if (gameState.isOnline)
        {
            RequestStopSwapping stopSwapRequest = new RequestStopSwapping(true);
            stopSwapRequest.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(stopSwapRequest, gameState.RequestEmpty);
        }
    }
}
