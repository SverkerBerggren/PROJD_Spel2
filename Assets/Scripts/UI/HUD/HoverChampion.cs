using UnityEngine;
using UnityEngine.EventSystems;

public class HoverChampion : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    private GameState gameState;
    [SerializeField] private ChampionAttributes championAttributes;
    [SerializeField] private int index;
    [SerializeField] private bool enemyChampion;
    [SerializeField] private int timeBeforeShow;
    [SerializeField] private int currentTime;
    [SerializeField] private bool startShow;
    [SerializeField] private bool hasShown;


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

            if(currentTime <= 0)
            {
                if (!hasShown)
                {
                    if (enemyChampion)
                    {
                        if (gameState.OpponentChampions.Count - 1 < index)
                            return;
                        else
                            ShowChampion(index, enemyChampion);
                    }
                    else
                    {
                        if (gameState.PlayerChampions.Count - 1 < index)
                            return;
                        else
                            ShowChampion(index, enemyChampion);
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
            championAttributes.UpdateChampionCard(GameState.Instance.OpponentChampions[index].Champion);
            championAttributes.gameObject.SetActive(true);
        }
        else
        {
            championAttributes.UpdateChampionCard(GameState.Instance.PlayerChampions[index].Champion);
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
