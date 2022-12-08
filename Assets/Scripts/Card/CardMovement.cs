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

    }

    public void OnDown()
    {
        cardDisplay.ResetSize();
        if (!actionOfPlayer.CheckIfCanPlayCard(cardDisplay, false)) return;
        //offset = transform.position - mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
        cardDisplay.mouseDown = true;
     

        if (gameState.targetingEffect != null && cardDisplay.card.typeOfCard == CardType.Attack)
            gameState.targetingEffect.SetActive(true);
        if (actionOfPlayer.selectCardOption)
            clickedOnCard = true;
    }

    public void OnDrag()
    {
        if (!actionOfPlayer.CheckIfCanPlayCard(cardDisplay, false)) return;
        if (gameObject.tag.Equals("LandmarkSlot")) return;
        if (cardDisplay.opponentCard == true) return;
        if (actionOfPlayer.selectCardOption) return;

       

        mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 12));

        transform.position = mousePosition + offset;
    }

    private void Update()
    {
        
        if (clickedOnCard)
        {
            
            mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 12));

            transform.position = mousePosition + offset;
        }
    }

    private void OnMouseDrag()
    {




        /*        RaycastHit hit;

                Physics.Raycast(mousePosition, Vector3.forward * 1f, out hit, 10f);
                Debug.DrawRay(mousePosition, Vector3.forward * 1f, Color.red, 100f);

                if (hit.collider == null)
                {
                    //cardDisplay.border.SetActive(false);
                    return;
                }

                if (hit.collider.gameObject.name.Equals("CardTriggerCollider"))
                {
                  //  cardDisplay.border.SetActive(true);
                }
                else
                {
                   // cardDisplay.border.SetActive(false);
                }*/
    }
}
