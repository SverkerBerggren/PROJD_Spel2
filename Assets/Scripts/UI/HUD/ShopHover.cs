using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHover : MonoBehaviour
{
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject oneSwitchSelected;

    public void ShowShopMenu()
    {
        if (GameState.Instance.HasPriority && GameState.Instance.IsItMyTurn)
            shopMenu.SetActive(true);
    }

    public void OneSwitchSelected()
    {
        if (oneSwitchSelected.activeSelf)      
            oneSwitchSelected.SetActive(false);
        else
            oneSwitchSelected.SetActive(true);
    }
}
