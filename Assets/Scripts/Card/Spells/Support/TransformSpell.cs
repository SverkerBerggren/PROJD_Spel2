using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.XR;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/TransformSpell")]
public class TransformSpell : Spells
{
    public override void PlaySpell()
    {
        // Takes the players hand and checks if there are any champion cards, then it choses a random card that it makes playable for all champions
        List<CardDisplay> cardsInHand = ActionOfPlayer.Instance.handPlayer.cardsInHand;       
        List<Card> cardList = new List<Card>();

        for (int i = 0; i < cardsInHand.Count; i++)
        {
            Card card = cardsInHand[i].card;
            if (card.championCard)
            {
                cardList.Add(card);
            }
        }

        if (cardList.Count == 0)
        {
            Debug.Log("There where no champion cards to transform");
        }

        int randomIndex = Random.Range(0, cardList.Count);
        cardList[randomIndex].championCardType = ChampionCardType.All;
        
        Debug.Log("Champion card thats playable for all: " + cardList[randomIndex]);
    }
}
