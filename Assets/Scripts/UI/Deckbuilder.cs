using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Deckbuilder : MonoBehaviour
{
    private Setup setup;
    private CardRegister register;
    private List<GameObject> buttons = new List<GameObject>();
    [SerializeField] private GameObject buttonHolder;
    public GameObject cardButton;
    private TMP_Text decklist;
    [SerializeField] private int maxCopies = 3;
    [SerializeField] private int deckCount = 40;

    private static Deckbuilder instance;
    public static Deckbuilder Instance { get { return instance; } set { instance = value; } }
    // Start is called before the first frame update

    private void Awake()
	{
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(Instance);
        }
        setup = Setup.Instance;
        decklist = GetComponentInChildren<TMP_Text>();
        UpdateDeckList();
	}

    void Start()
    {
        register = CardRegister.Instance;
        foreach (Card card in register.cardRegister.Values)
        {
            if (!card.championCard)    
            MakeButtonsCards(card);
        }
        //MakeButtonsChampions();
    }

	private void MakeButtonsCards(Card card)
    {
        GameObject gO = Instantiate(cardButton, buttonHolder.transform);
        CardDisplayAtributes cardDisplayAtributes = gO.GetComponentInChildren<CardDisplayAtributes>();
        cardDisplayAtributes.UpdateTextOnCardWithCard(card);
        gO.GetComponent<DeckbuilderCardButton>().card = card;
        buttons.Add(gO);
    }

    /*
    private void MakeButtonsChampions(Sprite championSprite, ListEnum listEnum, int index)
    {
        GameObject gO = Instantiate(cardButton, buttonHolder.transform);
        gO.GetComponent<Image>().enabled = true;
        gO.GetComponent<Image>().sprite = championSprite;
        gO.transform.localScale = new Vector3(1.3f, 1, 0.4f);

        gO.GetComponentInParent<GridLayoutGroup>().spacing = new Vector2(100, -100);

        gO.transform.Find("Landmark_Prefab").gameObject.SetActive(false);

        gO.GetComponent<ChoiceButton>().targetInfo = new TargetInfo(listEnum, index);
        buttons.Add(gO);
        //if (amountOfTargets == 0)
            gO.GetComponent<Button>().interactable = false;
    }
    */

    public void UpdateDeckList()
    {
        decklist.text = "Decklist\n\n";
        decklist.text += "Champions " + setup.myChampions.Count + "/3\n";
        foreach (string champion in setup.myChampions)
        {
            decklist.text += champion + "\n";
        }
        decklist.text += "\n";
        decklist.text += "Cards " + setup.playerDeckList.Count + "/" + deckCount + "\n";
        foreach (Card card in setup.playerDeckList)
        {
            decklist.text += card.cardName + "\n";
        }
    }
}
