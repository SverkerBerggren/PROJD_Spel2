using UnityEngine;

public class EnableSettings : MonoBehaviour
{
    private GameObject actionOfPlayer;
    private bool settingEnabled = false;

    [SerializeField] private GameObject checkImage;

    private void Start()
    {
        actionOfPlayer = ActionOfPlayer.Instance.gameObject;
    }

    public void OnClickOneSwitch()
    {
        if(!settingEnabled)
        {
            checkImage.SetActive(true);
            actionOfPlayer.GetComponent<NewOneSwitch>().enabled = true;
            actionOfPlayer.GetComponent<NewOneSwitch>().initialClick = true;
            settingEnabled = true;
        }
        else
        {
            checkImage.SetActive(false);
            actionOfPlayer.GetComponent<NewOneSwitch>().enabled = false;
            settingEnabled = false;
        }
    }


    public void OnClickLowerDetail()
    {
        if(!settingEnabled)
        {
            checkImage.SetActive(true);
            actionOfPlayer.GetComponent<LowerDetail>().SetUpLowerDetailBkg();
            settingEnabled = true;
        }
        else
        {
            checkImage.SetActive(false);
            actionOfPlayer.GetComponent<LowerDetail>().DefaultDetailBkg();
            settingEnabled = false;
        }
    }

    public void OnClickLowerDetailShader()
    {
        if (!settingEnabled)
        {
            checkImage.SetActive(true);
            actionOfPlayer.GetComponent<LowerDetail>().LowerDetailShader();
            settingEnabled = true;
        }
        else
        {
            checkImage.SetActive(false);
            actionOfPlayer.GetComponent<LowerDetail>().DefaultDetailShader();
            settingEnabled = false;
        }
    }

    public void OnClickDisableAnimAndEffects()
    {
        if (!settingEnabled)
        {
            checkImage.SetActive(true);
            actionOfPlayer.GetComponent<DisableAllEffects>().DisableEffects();
            settingEnabled = true;
        }
        else
        {
            checkImage.SetActive(false);
            actionOfPlayer.GetComponent<DisableAllEffects>().enableEffects();
            settingEnabled = false;
        }
    }

}
