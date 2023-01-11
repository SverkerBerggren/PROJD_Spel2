using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectIndicater : MonoBehaviour
{
    [SerializeField] private GameObject[] landmarkSelectBoxsOpponent;
    [SerializeField] private GameObject[] landmarkSelectBoxsPlayer;
    [SerializeField] private GameObject championSelectBoxOpponent;
    [SerializeField] private GameObject championSelectBoxPlayer;

    private GameState gameState;
    // Start is called before the first frame update

    private static SelectIndicater instance;
    public static SelectIndicater Instance { get { return instance; } }

    private void Awake()
    {
        if (Instance == null)      
            instance = this;      
        else     
            Destroy(gameObject);       
    }

    private void Start()
    {
        gameState = GameState.Instance;
    }

    //when player is trying to play a attack card, active the landmark indicater box if opponent has landmarks
    //***Speical notice with protectiv walls, only that landmark should have indicater.

    public void UppdateIndicater(Card card)
    {
        if (!card.Targetable) return;

        if (card is HealAndShieldChampion)
        {

        }


        // Check if opponent has a taunt landmark
        for (int i = 0; i < gameState.OpponentLandmarks.Count; i++)
        {
            LandmarkDisplay landmarkDisp = gameState.OpponentLandmarks[i];
            if (landmarkDisp.Card is TauntLandmark)
            {
                landmarkSelectBoxsOpponent[i].SetActive(true);
                return;
            }
        }

        championSelectBoxOpponent.SetActive(true);
        championSelectBoxPlayer.SetActive(true);
        for (int i = 0; i< gameState.OpponentLandmarks.Count; i++)
        {
            //if the landmark is targetable and the slot of landmark prefab is active. then active indicater 
            if (gameState.OpponentLandmarks[i].Card != null)
                landmarkSelectBoxsOpponent[i].SetActive(true);
            if (gameState.PlayerLandmarks[i].Card != null)
                landmarkSelectBoxsPlayer[i].SetActive(true);
        }
      
    }
    public void DisableIndicater()
    {
        championSelectBoxOpponent.SetActive(false);
        championSelectBoxPlayer.SetActive(false);
        foreach (GameObject go in landmarkSelectBoxsOpponent)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in landmarkSelectBoxsPlayer)
        {
            go.SetActive(false);
        }
    }


}
