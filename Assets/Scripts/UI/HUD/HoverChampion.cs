using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverChampion : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    [SerializeField] private ChampionAttributes championAttributes;
    [SerializeField] private int index;
    [SerializeField] private bool enemyChampion;
    [SerializeField] private int timeBeforeShow;
    [SerializeField] private int currentTime;
    [SerializeField] private bool startShow;
    [SerializeField] private bool hasShown;
    private GameState gameState;


    private void Start()
    {
        gameState = GameState.Instance;
    }

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
                        if (gameState.opponentChampions.Count - 1 < index)
                        {
                            return;
                        }
                        else
                        {
                            ShowChampion(index, enemyChampion);
                        }
                    }
                    else
                    {
                        if (gameState.playerChampions.Count - 1 < index)
                        {
                            return;
                        }
                        else
                        {
                            ShowChampion(index, enemyChampion);
                        }
                    }
                }
                hasShown = true;
            }
        }
    }

    private void ShowChampion(int index, bool enemyChampion)
    {
        if (enemyChampion)
        {

            championAttributes.UpdateChampionCard(GameState.Instance.opponentChampions[index].Champion);
            championAttributes.gameObject.SetActive(true);
        }
        else
        {
            championAttributes.UpdateChampionCard(GameState.Instance.playerChampions[index].Champion);
            championAttributes.gameObject.SetActive(true);
        }
    }

    private void HideChampion()
    {
        championAttributes.gameObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideChampion();
        startShow = false;
        hasShown = false;
        currentTime = timeBeforeShow;
    }


}
