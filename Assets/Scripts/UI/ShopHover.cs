using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHover : MonoBehaviour
{
    [SerializeField] private GameObject shopMenu;

    public void ShowShopMenu()
    {
        if (GameState.Instance.hasPriority && GameState.Instance.isItMyTurn)
        {
            shopMenu.SetActive(true);
        }
    }
}
