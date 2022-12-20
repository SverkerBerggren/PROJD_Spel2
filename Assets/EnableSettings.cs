using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSettings : MonoBehaviour
{
    [SerializeField] private GameObject checkImage;
    private bool settingEnabled = false;
    private GameObject actionOfPlayer;

    private void Start()
    {
        actionOfPlayer = ActionOfPlayer.Instance.gameObject;
    }

    public void OnClickOneSwitch()
    {
        if(!settingEnabled)
        {
            checkImage.SetActive(true);
            actionOfPlayer.GetComponent<OneSwitch>().enabled = true;
            settingEnabled = true;
        }
        else
        {
            checkImage.SetActive(false);
            actionOfPlayer.GetComponent<OneSwitch>().enabled = false;
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

}
