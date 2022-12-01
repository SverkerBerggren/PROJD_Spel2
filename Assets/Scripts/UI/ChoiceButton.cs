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
        Choice.Instance.AddTargetInfo(targetInfo);
        clickedEffect.SetActive(true);
    }
}
