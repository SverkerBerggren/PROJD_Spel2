using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhichManaCrystalsToShow : MonoBehaviour
{
    public List<GameObject> playerManaCrystals = new List<GameObject>();
    public List<GameObject> opponentManaCrystals = new List<GameObject>();
    private ActionOfPlayer actionOfPlayer;

    private static WhichManaCrystalsToShow instance;

    public static WhichManaCrystalsToShow Instance { get { return instance; } set { instance = value; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        actionOfPlayer = ActionOfPlayer.Instance;
    }

    public void UpdateManaCrystals()
    {
        for (int i = 0; i < playerManaCrystals.Count; i++)
        {
            if (actionOfPlayer.CurrentMana >= (i + 1))
                playerManaCrystals[i].SetActive(true);
            else
                playerManaCrystals[i].SetActive(false);
        }
        for (int i = 0; i < opponentManaCrystals.Count; i++)
        {
            if (actionOfPlayer.EnemyMana >= (i + 1))
                opponentManaCrystals[i].SetActive(true);
            else
                opponentManaCrystals[i].SetActive(false);
        }
    }
}
