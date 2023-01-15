using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/DuelistSupport")]
public class DuelistSupport : Spells
{
    public DuelistSupport()
    {
        ChampionCard = true;
        ChampionCardType = ChampionCardType.Duelist;
    }
	protected override void PlaySpell()
    {      
        List<AvailableChampion> enemyChamps = GameState.Instance.OpponentChampions;
        int index = 0;

        if (GameState.Instance.OpponentChampions.Count > 1)
        {
            for (int i = enemyChamps.Count - 1; i > 0; i--)
            {
                if (enemyChamps[i].Champion.Health > enemyChamps[i - 1].Champion.Health)
                    index = i - 1;
                else
                    index = i;
            }
        }
        else
            index = 0;

        //I Do
        ListEnum lEOpponenent = new ListEnum();
        lEOpponenent.opponentChampions = true;
        TargetInfo tIForOpponent = new TargetInfo(lEOpponenent, index);

        GameState.Instance.SwapChampionWithTargetInfo(tIForOpponent, false);

        if (GameState.Instance.IsOnline)
        {
            RequestSwitchActiveChamps request = new RequestSwitchActiveChamps(tIForOpponent);
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, GameState.Instance.RequestEmpty);
        }
        DrawAttackCard();
	}

    private void DrawAttackCard()
    {
		ActionOfPlayer actionOfPlayer = ActionOfPlayer.Instance;
		foreach (Card card in Deck.Instance.DeckPlayer)
		{
			if (card.TypeOfCard == CardType.Attack)
			{
				actionOfPlayer.DrawCardPlayer(1, card, true);
				Deck.Instance.RemoveCardFromDeck(card);
				if (GameState.Instance.IsOnline)
				{
					RequestDrawCard request = new RequestDrawCard(1);
					ClientConnection.Instance.AddRequest(request, GameState.Instance.RequestEmpty);
				}
				break;
			}
		}
	}
}
