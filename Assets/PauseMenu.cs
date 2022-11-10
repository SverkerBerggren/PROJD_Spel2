using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public bool isPauseMenuActive;
    [SerializeField] private GameObject pauseMenuObjectToEnable;
    [SerializeField] private ReturnButton returnButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isPauseMenuActive)
        {
            isPauseMenuActive = true;
            pauseMenuObjectToEnable.SetActive(true); 
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPauseMenuActive)
        {
            returnButton.ReturnAStep();
        }

        
    }

    public void setIsPauseMenuActive(bool value)
    {
        isPauseMenuActive = value;
        pauseMenuObjectToEnable.SetActive(value);
    }


}
