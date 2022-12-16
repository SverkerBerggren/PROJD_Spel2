using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public bool isPauseMenuActive;
    [SerializeField] private GameObject pauseMenuObjectToEnable;
    [SerializeField] private ReturnButton returnButton;
    [SerializeField] private GameObject settingsButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPauseMenuActive)
            settingsButton.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Escape) && !isPauseMenuActive)
        {
            ShowPauseMenu();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPauseMenuActive)
        {
            returnButton.ReturnAStep();
        }

        
    }

    public void ShowPauseMenu()
    {
        settingsButton.SetActive(false);
        isPauseMenuActive = true;
        pauseMenuObjectToEnable.SetActive(true);
    }


    public void setIsPauseMenuActive(bool value)
    {
        isPauseMenuActive = value;
        pauseMenuObjectToEnable.SetActive(value);
    }


}
