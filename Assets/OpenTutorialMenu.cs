using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTutorialMenu : MonoBehaviour
{
    
    public GameObject tutorialPanel;

    public void OpenTutorial()
    {
        if (tutorialPanel != null)
        {
            bool isActive = tutorialPanel.activeSelf;

            tutorialPanel.SetActive(!isActive);
        }
    }
}
