using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CardMovement : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;
    [System.NonSerialized] public Vector3 mousePosition;
    private CardDisplay cardDisplay;

    [System.NonSerialized] public bool clickedOnCard;


    private ActionOfPlayer actionOfPlayer;
    private GameState gameState;



    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cardDisplay = GetComponent<CardDisplay>();
        actionOfPlayer = ActionOfPlayer.Instance;
        gameState = GameState.Instance;
    }

    private void OnMouseDown()
    {
        print("Click");
    }

    public void OnDown()
    {
        if (!actionOfPlayer.CheckIfCanPlayCard(cardDisplay, false)) return;

        //offset = transform.position - mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
        cardDisplay.mouseDown = true;
     
        if (gameState.targetingEffect != null && cardDisplay.card.typeOfCard == CardType.Attack)
            gameState.targetingEffect.SetActive(true);

        cardDisplay.clickedOnCard = true;
        cardDisplay.ResetSize();       
    }

    public void OnDrag()
    {
        if (!actionOfPlayer.CheckIfCanPlayCard(cardDisplay, false)) return;
        if (gameObject.tag.Equals("LandmarkSlot")) return;
        if (cardDisplay.opponentCard == true) return;
        if (actionOfPlayer.selectCardOption) return;

        cardDisplay.ResetSize();
        mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 50));
        cardDisplay.displayTransform.position = mousePosition;
        //transform.position = mousePosition;
    }

    private void Update()
    {       
        if (clickedOnCard && actionOfPlayer.selectCardOption)
        {       
            mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20));
            transform.position = mousePosition + offset;
        }
    }
}
