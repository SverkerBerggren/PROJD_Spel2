using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovement : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;
    [System.NonSerialized] public Vector3 mousePosition;
    private CardDisplay cardDisplay;
    

    void Start()
    {
        mainCamera = Camera.main;
        cardDisplay = GetComponent<CardDisplay>();
    }

    private void OnMouseDown()
    {
        offset = transform.position - mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 12));
        if (GameState.Instance.targetingEffect != null && cardDisplay.card.typeOfCard == CardType.Attack) GameState.Instance.targetingEffect.SetActive(true);
    }

    private void OnMouseDrag()
    {
        if (gameObject.tag.Equals("LandmarkSlot")) return;
        if (gameObject.GetComponent<CardDisplay>().opponentCard == true) return;
        
        


        mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 12));

        transform.position = mousePosition + offset;

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
