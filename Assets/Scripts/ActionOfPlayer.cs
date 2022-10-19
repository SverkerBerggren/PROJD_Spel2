using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class ActionOfPlayer : MonoBehaviour
{
    public Hand handPlayer;
    public Hand handOpponent;

    private Card cardToPlay;
    private int cardCost;
    public int playerMana;

    private static ActionOfPlayer instance;

    public static ActionOfPlayer Instance { get { return instance; } set { instance = value; } }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.M))
        {
            playerMana++;
        }
        if (Input.GetKey(KeyCode.D))
        {
            GameState.Instance.DrawCard(1);
        }
    }

    public bool CheckIfCanPlayCard(Card card, CardDisplay cardDisplay)
    {
        cardCost = card.manaCost;
        if (playerMana >= cardCost)
        {
            playerMana -= cardCost;
            cardToPlay = card;
            card.PlayCard();
            cardDisplay.card = null;
            return true;

        }
        else
        {
            Debug.Log("You don't have enough Mana");
            return false;
        }
    }

}
