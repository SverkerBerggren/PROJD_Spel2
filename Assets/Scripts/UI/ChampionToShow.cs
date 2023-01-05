using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionToShow : MonoBehaviour
{
    [SerializeField] private ChampionAttributes championAttributes;

    public void ShowChampion(int index, bool enemyChampion)
    {   
        if(enemyChampion) 
        {

            championAttributes.UpdateChampionCard(GameState.Instance.opponentChampions[index].champion);
            championAttributes.gameObject.SetActive(true);
        }
        else
        {
            championAttributes.UpdateChampionCard(GameState.Instance.playerChampions[index].champion);
            championAttributes.gameObject.SetActive(true);
        }
    }

    public void HideChampion()
    {
        championAttributes.gameObject.SetActive(false);
    }
}
