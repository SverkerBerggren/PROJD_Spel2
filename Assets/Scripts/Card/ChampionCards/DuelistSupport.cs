using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/DuelistSupport")]
public class DuelistSupport : Spells
{
    public DuelistSupport()
    {
        championCard = true;
        championCardType = ChampionCardType.Duelist;
    }
    public override void PlaySpell()
    {      
        List<AvailableChampion> enemyChamps = GameState.Instance.opponentChampions;

        int index = 0;

        for (int i = enemyChamps.Count - 1; i > 0; i--)
        {
            
            if (enemyChamps[i].champion.health > enemyChamps[i-1].champion.health)
            {
                index = i - 1;
            }
            else
            {
                index = i;
            }
        }
        //I Do
        ListEnum lEOpponenent = new ListEnum();
        lEOpponenent.opponentChampions = true;
        TargetInfo tIForOpponent = new TargetInfo(lEOpponenent, index);

        GameState.Instance.SwapChampionWithTargetInfo(tIForOpponent, false);

        if (GameState.Instance.isOnline)
        {
            RequestSwitchActiveChamps request = new RequestSwitchActiveChamps(tIForOpponent);
            request.whichPlayer = ClientConnection.Instance.playerId;

            ClientConnection.Instance.AddRequest(request, GameState.Instance.RequestEmpty);
        }


        ActionOfPlayer actionOfPlayer = ActionOfPlayer.Instance;
        foreach (Card card in Deck.Instance.deckPlayer)
        {
            if (card is AttackSpell)
            {
                actionOfPlayer.DrawCardPlayer(1, card, true);
                Deck.Instance.RemoveCardFromDeck(card);
                if (GameState.Instance.isOnline)
                {
                    RequestDrawCard request = new RequestDrawCard(1);
                    ClientConnection.Instance.AddRequest(request, GameState.Instance.RequestEmpty);
                }
                break;
            }
        }
    }
}
