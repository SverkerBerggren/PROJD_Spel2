using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBoxTexture : MonoBehaviour
{
	[SerializeField] private RawImage imageToChange;
	[SerializeField] private Texture texturePanel;
	[SerializeField] private Texture textureImage;
	[SerializeField] private string textToChangeTitle;
	[SerializeField] private string textToChangeInfo;
    [SerializeField] private TMP_FontAsset originalFont;
    [SerializeField] private TMP_FontAsset dyslexicFont;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI infoText;


<<<<<<< HEAD
    [SerializeField] private AudioClip audioClipToSet;
    [SerializeField] private TextTOSpeechButton textToSpeechScript;
=======
    [SerializeField] private AudioClip clipToSet;
    [SerializeField] private TextTOSpeechButton speechButton;

>>>>>>> Main-5.0

    public void OnClick()
    {   
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
        titleText.text = textToChangeTitle;
        infoText.text = textToChangeInfo;

<<<<<<< HEAD
        textToSpeechScript.clipToPlay = audioClipToSet;
=======
        speechButton.clipToPlay = clipToSet;
>>>>>>> Main-5.0
    }
}
