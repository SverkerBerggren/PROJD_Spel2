using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuObjectToEnable;
    [SerializeField] private ReturnButton returnButton;
    [SerializeField] private GameObject settingsButton;

    public bool IsPauseMenuActive;

    // Update is called once per frame
    void Update()
    {
        if (!IsPauseMenuActive)
            settingsButton.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!IsPauseMenuActive)
                ShowPauseMenu();
            else
                returnButton.ReturnAStep();
        }
    }

    public void ShowPauseMenu()
    {
        settingsButton.SetActive(false);
        IsPauseMenuActive = true;
        pauseMenuObjectToEnable.SetActive(true);

        NewOneSwitch.Instance.resetBools();
        NewOneSwitch.Instance.options = true;
    }

    public void SetIsPauseMenuActive(bool value)
    {
        IsPauseMenuActive = value;
        pauseMenuObjectToEnable.SetActive(value);
    }
}
