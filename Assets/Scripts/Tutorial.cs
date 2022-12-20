using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialWindow;
    public TextMeshProUGUI header;
    public TextMeshProUGUI body;


    public static Tutorial instance;

    private void Awake() {
        if(instance == null)
            instance = this;
            else
            Destroy(gameObject);
    }

    public void ShowTutorial(string  header, string body) {
        this.header.text = header;
        this.body.text = body;

        tutorialWindow.SetActive(true);
    }

    public void HideTutorial()
    {
        tutorialWindow.SetActive(false);
    }
}
