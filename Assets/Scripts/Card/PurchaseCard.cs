using UnityEngine;

public class PurchaseCard : MonoBehaviour
{
    [SerializeField] private int costOfUnspentMana = 0;
    [SerializeField] private GameObject oneSwitchHover;
    public void CardToPurchase(Card cardPurchased)
    {
        GameState gameState = GameState.Instance;
		ActionOfPlayer actionOfPlayer = ActionOfPlayer.Instance;

        if (!(gameState.hasPriority) || !(gameState.isItMyTurn) || (Choice.Instance.IsChoiceActive)) return;

        if (actionOfPlayer.UnspentMana >= costOfUnspentMana)
        {
            cardPurchased.PurchasedFormShop = true;
            cardPurchased.PlayCard();
            actionOfPlayer.UnspentMana -= costOfUnspentMana;
        }
    }

    public void OneSwitchHover()
    {
        if (oneSwitchHover.activeSelf)
            oneSwitchHover.SetActive(false);
        else
            oneSwitchHover.SetActive(true);
    }
}
