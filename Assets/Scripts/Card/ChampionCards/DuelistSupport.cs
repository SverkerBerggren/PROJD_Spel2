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

        ListEnum lE = new ListEnum();
        lE.myChampions = true;

        TargetInfo tI = new TargetInfo(lE, index);

        GameState.Instance.SwapChampionWithTargetInfo(tI, false);

        ActionOfPlayer actionOfPlayer = ActionOfPlayer.Instance;

        foreach (Card card in actionOfPlayer.handPlayer.deck.deckPlayer)
        {
            if (card is AttackSpell)
            {
                actionOfPlayer.DrawCardPlayer(1, card, true);
                actionOfPlayer.handPlayer.deck.RemoveCardFromDeck(card);
                if (GameState.Instance.isOnline)
                {
                    //Kanske new request???
                }
                break;
            }
        }
    }
}
