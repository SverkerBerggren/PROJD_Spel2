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
        {
            if (shopMenu.activeSelf)
            {
                shopMenu.SetActive(false);
                if (NewOneSwitch.Instance != null)
                    NewOneSwitch.Instance.ResetBools();
            }
            else
            {
                shopMenu.SetActive(true);
                if (NewOneSwitch.Instance != null)
                    NewOneSwitch.Instance.shop = true;
            }
        }
            
    }

    public void OneSwitchSelected()
    {
        if (oneSwitchSelected.activeSelf)      
            oneSwitchSelected.SetActive(false);
        else
            oneSwitchSelected.SetActive(true);
    }
}
