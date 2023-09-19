using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTutorialMenu : MonoBehaviour
{
    
    public GameObject tutorialPanel;


    public void OpenTutorial()
    {
        if (tutorialPanel.activeSelf)
        {
            tutorialPanel.SetActive(false);
            if (NewOneSwitch.Instance != null)
                NewOneSwitch.Instance.ResetBools();
        }
        else
        {
            tutorialPanel.SetActive(true);
            if (NewOneSwitch.Instance != null)
                NewOneSwitch.Instance.tutorialMenu = true;
        }
    }
}
