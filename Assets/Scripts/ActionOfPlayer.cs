using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;
using TMPro;

public class ActionOfPlayer : MonoBehaviour
{
    public Hand handPlayer;
    public Hand handOpponent;

    [SerializeField] private TMP_Text manaText;

    public GameObject choice;

    private int cardCost;
    public int playerMana = 0;
    public int currentMana = 0;
    public readonly int maxMana = 10;
    [System.NonSerialized] public int tauntPlaced = 0;
    public bool selectCardOption = false;

    private GameState gameState;

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

        gameState = GameState.Instance;
    }



    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.M))
        {
            playerMana++;
        }
        if (Input.GetKey(KeyCode.D))
        {
            GameState.Instance.DrawCard(1, null);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ListEnum lE = new ListEnum();
            lE.myChampions = true;
            choice.SetActive(true);
            Choise.Instance.ShowChoiceMenu(lE, 1, WhichMethod.switchChampion);
        }
        manaText.text = "Mana: " + currentMana.ToString();
    }

    public bool CheckIfCanPlayCard(CardDisplay cardDisplay)
    {
        cardCost = cardDisplay.manaCost;
        if (gameState.factory > 0)        
            if (gameState.playerLandmarks.Count >= 3)
                cardCost -= (2 * gameState.factory);

        if (currentMana >= cardCost)
        {
            currentMana -= cardCost;
            return true;
        }
        else
        {
            Debug.Log("You don't have enough Mana");
            return false;
        }
    }

    public void IncreaseMana()
    {
        if(playerMana < maxMana)
            playerMana++;

        currentMana = playerMana;
    }

    public void ChangeCardOrder(bool isPlayer , CardDisplay cardDisplay)
    {
        /*
        Hand hand;
        if (isPlayer)
            hand = handPlayer;
        else
            hand = handOpponent;

        int index = hand.cardSlotsInHand.IndexOf(cardDisplay.gameObject);

        for (; index < hand.cardSlotsInHand.Count; index++)
        {
            CardDisplay cardI = hand.cardSlotsInHand[index].GetComponent<CardDisplay>();
            for (int j = index + 1; j < hand.cardSlotsInHand.Count; j++)
            {
                int g = hand.cardSlotsInHand.Count;
                if (hand.cardSlotsInHand[index].activeSelf)
                {
                    CardDisplay cardJ = hand.cardSlotsInHand[j].GetComponent<CardDisplay>();
                    cardI.manaCost = cardJ.manaCost;
                    cardI.card = cardJ.card;
                    index--;
                }
            }
        }
        */
    }
}
