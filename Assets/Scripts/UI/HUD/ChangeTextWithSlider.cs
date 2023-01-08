using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeTextWithSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public TMP_Text TextToChange;

    public void ChangeText()
    {       
        TextToChange.text = slider.value + "/" + slider.maxValue; 
    }
}
