using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LandmarkDisplay : MonoBehaviour
{
    public Landmarks card;
    public SpriteRenderer artworkSpriteRenderer;
    public int health;
    public bool occultGathering = false;
    [NonSerialized] public int tenExtraDamage;
    private GameState gameState;



    private void Start()
    {
        gameState = GameState.Instance;
    }

    private void UpdateTextOnCard()
    {
        if (card == null) return;

        artworkSpriteRenderer.sprite = card.artwork;
    }

    public void DestroyLandmark()
    {
        card = null;
    }

    private void LandmarkDead()
    {
        card.LandmarkEffectTakeBack();
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
        if (card == null)
            artworkSpriteRenderer.sprite = null;

        if (gameState.amountOfTurns == 10)
        {
            if (card.cardName.Equals("Mysterious Forest"))
            {
                DestroyLandmark();
                gameState.DrawCard(5);
            }
        }
        UpdateTextOnCard();
    }
}


