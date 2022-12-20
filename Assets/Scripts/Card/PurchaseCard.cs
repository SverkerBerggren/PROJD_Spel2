using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseCard : MonoBehaviour
{
    [SerializeField] private int costOfUnspentMana = 0;
    private ActionOfPlayer actionOfPlayer;
    public void CardToPurchase(Card cardPurchased)
    {
        if (!(GameState.Instance.hasPriority) || !(GameState.Instance.isItMyTurn) || (Choice.Instance.isChoiceActive)) return;

        actionOfPlayer = ActionOfPlayer.Instance;
        
        if (actionOfPlayer.unspentMana >= costOfUnspentMana)
        {
            print("Should Purchase Card");
            cardPurchased.purchasedFormShop = true;
            cardPurchased.PlayCard();
            actionOfPlayer.unspentMana -= costOfUnspentMana;
        }
    }
}
