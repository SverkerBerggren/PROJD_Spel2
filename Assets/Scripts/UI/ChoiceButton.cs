using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChoiceButton : MonoBehaviour
{
    public TargetInfo targetInfo;
    [SerializeField] private GameObject clickedEffect;
    public void OnClick()
    {
        if (clickedEffect.activeSelf)
        {
            Choice.Instance.RemoveTargetInfo(targetInfo);
            clickedEffect.SetActive(false);
        }
        else
        {
            Choice.Instance.AddTargetInfo(targetInfo);
            clickedEffect.SetActive(true);
        }
    }
}
