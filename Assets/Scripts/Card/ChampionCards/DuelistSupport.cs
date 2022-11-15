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

        AvailableChampion champToSwapTo;
        int index = 0;

        for (int i = enemyChamps.Count; i > 0; i++)
        {
            if (enemyChamps[i].champion.health > enemyChamps[i-1].champion.health)
            {
                champToSwapTo = enemyChamps[i - 1];
                index = i - 1;
            }
            else
            {
                champToSwapTo = enemyChamps[i];
                index = i;
            }
        }

        ListEnum lE = new ListEnum();
        lE.opponentChampions = true;

        TargetInfo tI = new TargetInfo(lE, index);

        GameState.Instance.SwapChampionWithTargetInfo(tI, false);
    }
}
