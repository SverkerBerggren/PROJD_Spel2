using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Choise : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
    public ListEnum listEnum;

    public List<TargetInfo> chosenTargets = new List<TargetInfo>();
    public int amountOfTargetsToSelect = 0;
    public GameObject choiceButtonPrefab;

    public TMP_Text descriptionText;
    public GameObject buttonHolder;

    private GameState gameState;

    public WhichMethod whichMethod;

    private static Choise instance;

    public static Choise Instance { get { return instance; } set { instance = value; } }

    public void ChoiseOfALifetime()
    {
        if (listEnum.myChampions)
        {

        }
        else if (listEnum.opponentChampions)
        {

        }
        else if (listEnum.myLandmarks)
        {

        }
        else if (listEnum.opponentLandmarks)
        {

        }
        else if (listEnum.myGraveyard)
        {

        }
        else if (listEnum.opponentGraveyard)
        {

        }
    }

    public void ShowChoiceMenu(ListEnum listEnum)
    {
        if (listEnum.myChampions)
        {
            for (int i = 0; i < gameState.playerChampions.Count; i++)
            {
                AvailableChampion champ = gameState.playerChampions[i];
                if (champ == gameState.playerChampion) continue;

                GameObject gO = Instantiate(choiceButtonPrefab, buttonHolder.transform);

                gO.GetComponent<Image>().sprite = champ.champion.artwork;

                gO.GetComponent<ChoiceButton>().targetInfo = new TargetInfo(listEnum, i);

            }
        }
    }

    public void AddTargetInfo(TargetInfo targetInfo)
    {
        chosenTargets.Add(targetInfo);

        if (chosenTargets.Count == amountOfTargetsToSelect)
        {
            switch(whichMethod)
            {
                case WhichMethod.switchChampion:
                    SwitchChamp(false);
                    break;

                case WhichMethod.switchChampionDied:
                    SwitchChamp(true);
                    break;
            }
        }
    }

    private void SwitchChamp(bool died)
    {
        gameState.SwapChampionWithTargetInfo(chosenTargets[0], died);

        if (gameState.isOnline)
        {
            RequestSwitchActiveChamps request = new RequestSwitchActiveChamps(chosenTargets[0]);
            request.whichPlayer = ClientConnection.Instance.playerId;

            ClientConnection.Instance.AddRequest(request, gameState.RequestEmpty);
        }
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        gameState = GameState.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum WhichMethod
{
    switchChampion, switchChampionDied
}
