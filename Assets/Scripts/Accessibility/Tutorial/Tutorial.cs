using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject tutorialWindow;
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI body;


    public static Tutorial Instance;

    private void Awake() {
        if(Instance == null)
            Instance = this;
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
