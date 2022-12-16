using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeTextWithSlider : MonoBehaviour
{
    public TMP_Text textToChange;
    [SerializeField] private Slider slider;

    public void ChangeText()
    {       
        textToChange.text = slider.value + "/" + slider.maxValue; 
    }
}
