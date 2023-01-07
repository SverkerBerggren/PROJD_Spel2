using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DyslexicPauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TMP_FontAsset regularFont;
    [SerializeField] private TMP_FontAsset dyslexicFont;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI infoText;

    [SerializeField] private float regularFontSize = 36;
    [SerializeField] private float regularFontSizeTitle = 42;

    [SerializeField] private float dyslexicFontSize = 26;
    [SerializeField] private float dyslexicFontSizeTitle = 32;

    [SerializeField] private List<TextMeshProUGUI> listOfTextToChange;

    [SerializeField] private float fontSizeMultiplier = 1.25f;
    [SerializeField] private float fontSizeDivider = 0.8f;


    public void OnClick()
    {
        if(titleText.font.Equals(regularFont))
        {
            titleText.font = dyslexicFont;
            infoText.font = dyslexicFont;
            infoText.fontSize = dyslexicFontSize;
            titleText.fontSize = dyslexicFontSizeTitle;

            foreach(TextMeshProUGUI item in listOfTextToChange)
            {
                item.font = dyslexicFont;
                item.fontSize = (float)(item.fontSize * fontSizeMultiplier); 
            }
        }
        else
        {
            titleText.font = regularFont;
            infoText.font = regularFont;
            infoText.fontSize = regularFontSize;
            titleText.fontSize = regularFontSizeTitle;
            foreach (TextMeshProUGUI item in listOfTextToChange)
            {
                item.font = regularFont;
                item.fontSize = (float)(item.fontSize * fontSizeDivider);

            }
        }
    }
}
