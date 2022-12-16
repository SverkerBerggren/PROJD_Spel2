using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DyslexicPauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_FontAsset regularFont;
    public TMP_FontAsset dyslexicFont;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI infoText;

    public float regularFontSize = 36;
    public float regularFontSizeTitle = 42;

    public float dyslexicFontSize = 26;
    public float dyslexicFontSizeTitle = 32;

    public void OnClick()
    {
        if(titleText.font.Equals(regularFont))
        {
            titleText.font = dyslexicFont;
            infoText.font = dyslexicFont;
            infoText.fontSize = dyslexicFontSize;
            titleText.fontSize = dyslexicFontSizeTitle;
        }
        else
        {
            titleText.font = regularFont;
            infoText.font = regularFont;
            infoText.fontSize = regularFontSize;
            titleText.fontSize = regularFontSizeTitle;
        }
    }
}
