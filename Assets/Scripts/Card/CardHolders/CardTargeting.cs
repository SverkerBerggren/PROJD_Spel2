using UnityEngine;

public class CardTargeting : MonoBehaviour
{
    private Vector3 mousePosition;
    private CardDisplay cardDisplay;
    private CardMovement cardMovement;
    private PlayCardManager playCardManager;
    private GameObject gameObjectHit;
    private TypeOfCardTargeting typeOfCardTarget;
    private Camera mainCamera;

    void Start()
    {
        playCardManager = PlayCardManager.Instance;
        cardDisplay = GetComponent<CardDisplay>();       
        mainCamera = Camera.main;
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
    public void MouseUp()
    {
        cardMovement = GetComponent<CardMovement>();
        mousePosition = cardMovement.MousePosition;
        cardDisplay.mouseDown = false;
        cardDisplay.clickedOnCard = false;

        if (!playCardManager.CanCardBePlayed(cardDisplay))
        {
            cardDisplay.ResetSize();
            return;
        }

        RaycastHit[] hitEnemy;
        Vector3 dir = mousePosition - mainCamera.transform.position;
        hitEnemy = Physics.RaycastAll(mousePosition, dir, 200f);       
        Debug.DrawRay(mousePosition, dir, Color.blue, 100f);

        if (CheckIfRaycastHitEnemy(hitEnemy) == TypeOfCardTargeting.None)
            cardDisplay.ResetSize();
        else
            playCardManager.PlayCard(typeOfCardTarget, gameObjectHit);

        //Snapping the card back
        cardDisplay.displayTransform.localPosition = Vector3.zero;
        cardDisplay.displayTransform.position += new Vector3(0, 7.5f, -1);
    }

}
