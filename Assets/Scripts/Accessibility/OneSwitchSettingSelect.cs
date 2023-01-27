using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSwitchSettingSelect : MonoBehaviour
{
    [SerializeField] private GameObject oneSwitchHover;
    public void OneSwitchHover()
    {
        if (oneSwitchHover.activeSelf)
            oneSwitchHover.SetActive(false);
        else
            oneSwitchHover.SetActive(true);
    }

    public void HoverOn()
    {
        oneSwitchHover.SetActive(true);
    }

    public void HoverOff()
    {
        oneSwitchHover.SetActive(false);
    }

}
