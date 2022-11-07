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

}
