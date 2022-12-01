using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CardTargeting : MonoBehaviour
{
    private Vector3 startposition;
    private RectTransform gameObjectRectTransform;

    private ActionOfPlayer actionOfPlayer;
    private CardDisplay cardDisplay;
    private CardMovement cardMovement;
    private Vector3 mousePosition;
    private Card card;
    private Graveyard graveyard;
    private GameState gameState;
    private PlayCardManager playCardManager;

    private GameObject gameObjectHit;

    private TypeOfCardTargeting typeOfCardTarget;



    void Start()
    {
        graveyard = Graveyard.Instance;
        actionOfPlayer = ActionOfPlayer.Instance;
        gameState = GameState.Instance;
        playCardManager = PlayCardManager.Instance;
        cardDisplay = GetComponent<CardDisplay>();       

        if (!gameObject.CompareTag("LandmarkSlot"))
        {
                gameObjectRectTransform = GetComponent<RectTransform>();           
                startposition = gameObjectRectTransform.anchoredPosition;
        }
    }

    private void OnMouseUp()
    {
        cardMovement = GetComponent<CardMovement>();
        mousePosition = cardMovement.mousePosition;
        card = cardDisplay.card;
        cardDisplay.mouseDown = false;

        if (!PlayCardManager.Instance.CanCardBePlayed(cardDisplay))
        {
            CardGoBackToStartingPosition();
            return;
        }

        RaycastHit[] hitEnemy;
        hitEnemy = Physics.RaycastAll(mousePosition, Vector3.forward * 100 + Vector3.down * 55, 200f);
        Debug.DrawRay(mousePosition, Vector3.forward * 100 + Vector3.down * 55, Color.red, 100f);

        if (CheckIfRaycastHitEnemy(hitEnemy) == TypeOfCardTargeting.None)
            CardGoBackToStartingPosition();
        else
            playCardManager.PlayCard(typeOfCardTarget, gameObjectHit);
    }



    private TypeOfCardTargeting CheckIfRaycastHitEnemy(RaycastHit[] hitEnemy)
    {
        typeOfCardTarget = TypeOfCardTargeting.None;
        for (int i = 0; i < hitEnemy.Length; i++)
        {
            gameObjectHit = hitEnemy[i].collider.gameObject;
            typeOfCardTarget = playCardManager.CheckIfHitAnEnemy(gameObjectHit);
            if (typeOfCardTarget != TypeOfCardTargeting.None)
                break;
        }
        return typeOfCardTarget;
    }

    private void CardGoBackToStartingPosition()
    {
        gameObjectRectTransform.anchoredPosition = startposition;
        cardDisplay.ResetSize();
    }
}
