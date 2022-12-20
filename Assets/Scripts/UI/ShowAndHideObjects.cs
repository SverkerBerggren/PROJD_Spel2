using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAndHideObjects : MonoBehaviour
{
    [SerializeField] private GameObject shopMenu;

    public void Hide()
    {
        shopMenu.SetActive(false);
    }
    public void Show()
    {
        if (GameState.Instance.isItMyTurn && GameState.Instance.hasPriority)
        {
            shopMenu.SetActive(true);
        }
    }
}
