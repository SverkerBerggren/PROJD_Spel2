using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LandmarkDisplay : MonoBehaviour
{
    public Landmarks card;
    
    public int health;
    public TMP_Text healthText;
    public TMP_Text descriptionText;
    public TMP_Text nameText;
    public TMP_Text manaText;

    public GameObject landmarkPrefab;

    public bool occultGathering = false;
    [NonSerialized] public int tenExtraDamage;
    private GameState gameState;
    public int index;



    private void Start()
    {
        gameState = GameState.Instance;
    }

    private void UpdateTextOnCard()
    {
        if (card == null)
        {
            landmarkPrefab.SetActive(false);
            return;
        }
        landmarkPrefab.SetActive(true);
        healthText.text = health.ToString();
        descriptionText.text = card.description;
        manaText.text = card.manaCost.ToString();
        nameText.text = card.cardName;
    }

    public void DestroyLandmark()
    {
        card = null;
    }

    private void LandmarkDead()
    {
        card.LandmarkEffectTakeBack();
        card.WhenLandmarksDie();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            LandmarkDead();
            Graveyard.Instance.AddCardToGraveyard(card);
            card = null;
        }
    }

    private void FixedUpdate()
    {
        if (gameState.amountOfTurns == 10)
        {
            if (card.cardName.Equals("Mysterious Forest"))
            {
                DestroyLandmark();
                gameState.DrawCard(5, null);
            }
        }
        UpdateTextOnCard();
    }
}


