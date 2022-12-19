using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBoxTexture : MonoBehaviour
{
    public RawImage parentImage;
    public RawImage imageToChange;

    public Texture texturePanel;
    public Texture textureImage;


    public string textToChangeTitle;
    public string textToChangeInfo;

    public TMP_FontAsset originalFont;
    public TMP_FontAsset dyslexicFont;


    public TextMeshProUGUI titleText;
    public TextMeshProUGUI infoText;
    private void Start()
    {
        parentImage = gameObject.GetComponentInParent<RawImage>();
    }
    public void OnClick()
    {
       // parentImage.texture = texturePanel;

        
        if(textureImage != null)
        {
            imageToChange.texture = textureImage;
            imageToChange.gameObject.SetActive(true);
      
        }
        else
        {
            imageToChange.texture = null;
            imageToChange.gameObject.SetActive(false);
        }
        print("klickas det?");
        titleText.text = textToChangeTitle;
        infoText.text = textToChangeInfo;
    }
}
