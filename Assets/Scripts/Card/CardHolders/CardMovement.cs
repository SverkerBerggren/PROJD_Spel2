using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CardMovement : MonoBehaviour
{
    private ActionOfPlayer actionOfPlayer;
    private GameState gameState;
    private Vector3 offset;
    private Camera mainCamera;
    private CardDisplay cardDisplay;

    [System.NonSerialized] public Vector3 MousePosition;
    [System.NonSerialized] public bool ClickedOnCard;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cardDisplay = GetComponent<CardDisplay>();
        actionOfPlayer = ActionOfPlayer.Instance;
        gameState = GameState.Instance;
    }
    private void Update()
    {       
        if (ClickedOnCard && actionOfPlayer.SelectCardOption)
        {       
            MousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20));
            transform.position = MousePosition + offset;
        }
    }

    public void OnDown()
    {
        if (!actionOfPlayer.CheckIfCanPlayCard(cardDisplay, false)) return;

        cardDisplay.mouseDown = true;
     
        if (gameState.TargetingEffect != null && cardDisplay.Card.TypeOfCard == CardType.Attack)
            gameState.TargetingEffect.SetActive(true);
        MousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 50));
        cardDisplay.clickedOnCard = true;
        cardDisplay.ResetSize();       
    }

    public void OnDrag()
    {
        if (!actionOfPlayer.CheckIfCanPlayCard(cardDisplay, false)) return;
        if (cardDisplay.Card.ChampionCard && cardDisplay.Card.ChampionCardType != ChampionCardType.All)
        {
            if(gameState.PlayerChampion.Champion.ChampionCardType != cardDisplay.Card.ChampionCardType) return;
        } 
		if (gameObject.tag.Equals("LandmarkSlot")) return;
        if (cardDisplay.OpponentCard == true) return;
        if (actionOfPlayer.SelectCardOption) return;

        cardDisplay.ResetSize();
        MousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 50));
        cardDisplay.displayTransform.position = MousePosition;
    }

}
