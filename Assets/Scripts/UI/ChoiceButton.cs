using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChoiceButton : MonoBehaviour
{
    public TargetInfo targetInfo;
    [Header("Cards")]
    public GameObject cardPrefab;
    public GameObject championPrefab;

    [SerializeField] private GameObject clickedEffect;

    [Header("Accessability")]
    [SerializeField] private GameObject hoverEffect;
    public void OnClick()
    {
        if (clickedEffect.activeSelf)
        {
            Choice.Instance.RemoveTargetInfo(targetInfo);
            clickedEffect.SetActive(false);
        }
        else
        {
            print("Effect");
            Choice.Instance.AddTargetInfo(targetInfo);
            clickedEffect.SetActive(true);
        }
    }

    public void OneSwitchHoverChoice()
    {
        if (hoverEffect.activeSelf)
        {
            hoverEffect.SetActive(false);
        }
        else
        {
            hoverEffect.SetActive(true);
        }
    }
}
