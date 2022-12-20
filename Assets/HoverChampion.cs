using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverChampion : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    [SerializeField] private ChampionToShow championToShow;
    [SerializeField] private int index;
    [SerializeField] private bool enemyChampion;
    [SerializeField] private int timeBeforeShow;
    [SerializeField] private int currentTime;
    [SerializeField] private bool startShow;
    [SerializeField] private bool hasShown;


    public void OnPointerEnter(PointerEventData eventData)
    {   
        startShow = true;
        hasShown = false;
        currentTime = timeBeforeShow;
    }
    private void FixedUpdate()
    {
        if(startShow)
        {
            currentTime -= 1;

            if(currentTime<= 0)
            {
                if (!hasShown)
                {
                    if (enemyChampion)
                    {
                        if (GameState.Instance.opponentChampions.Count - 1 < index)
                        {
                            return;
                        }
                        else
                        {
                            championToShow.ShowChampion(index, enemyChampion);
                        }
                    }
                    else
                    {
                        if (GameState.Instance.playerChampions.Count - 1 < index)
                        {
                            return;
                        }
                        else
                        {
                            championToShow.ShowChampion(index, enemyChampion);
                        }
                    }
                }
                hasShown = true;
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        championToShow.HideChampion();
        startShow = false;
        hasShown = false;
        currentTime = timeBeforeShow;
    }


}
