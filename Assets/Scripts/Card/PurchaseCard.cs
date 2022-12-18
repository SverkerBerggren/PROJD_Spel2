using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseCard : MonoBehaviour
{
    [SerializeField] private int costOfUnspentMana = 0;
    private ActionOfPlayer actionOfPlayer;
    public void CardToPurchase(Card cardPurchased)
    {
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
