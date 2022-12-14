using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CardTargeting : MonoBehaviour
{
    private Vector3 startposition;
    private Vector3 mousePosition;
    private RectTransform gameObjectRectTransform;

    private CardDisplay cardDisplay;
    private CardMovement cardMovement;
    private PlayCardManager playCardManager;

    private GameObject gameObjectHit;

    private TypeOfCardTargeting typeOfCardTarget;



    void Start()
    {
        playCardManager = PlayCardManager.Instance;
        cardDisplay = GetComponent<CardDisplay>();       

        if (!gameObject.CompareTag("LandmarkSlot"))
        {
                gameObjectRectTransform = GetComponent<RectTransform>();           
                startposition = gameObjectRectTransform.anchoredPosition;
        }
    }

    public void MouseUp()
    {
        cardMovement = GetComponent<CardMovement>();
        mousePosition = cardMovement.mousePosition;
        cardDisplay.mouseDown = false;
        cardDisplay.clickedOnCard = false;

        

        if (!playCardManager.CanCardBePlayed(cardDisplay))
        {
            cardDisplay.ResetSize();
            return;
        }

        RaycastHit[] hitEnemy;
        hitEnemy = Physics.RaycastAll(mousePosition, Vector3.forward * 100 + Vector3.down * 55, 200f);
        Debug.DrawRay(mousePosition, Vector3.forward * 100 + Vector3.down * 55, Color.red, 100f);

        if (CheckIfRaycastHitEnemy(hitEnemy) == TypeOfCardTargeting.None)
            cardDisplay.ResetSize();
        else
            playCardManager.PlayCard(typeOfCardTarget, gameObjectHit);

        //Snapping the card back
        cardDisplay.displayTransform.localPosition = Vector3.zero;
        cardDisplay.displayTransform.position += new Vector3(0, 7.5f, -1);
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

/*    private void CardGoBackToStartingPosition()
    {
        gameObjectRectTransform.anchoredPosition = startposition;
        
    }*/
}
