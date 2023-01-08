using UnityEngine;

public class ShowShop : MonoBehaviour
{
    [SerializeField] private GameObject shopMenu;

    public void Hide()
    {
        shopMenu.SetActive(false);
    }
    public void Show()
    {
        if (GameState.Instance.IsItMyTurn && GameState.Instance.HasPriority)
            shopMenu.SetActive(true);
    }
}
