using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhichManaCrystalsToShow : MonoBehaviour
{
    public List<GameObject> manaCrystals = new List<GameObject>();
    private ActionOfPlayer actionOfPlayer;
    // Start is called before the first frame update
    void Start()
    {
        actionOfPlayer = ActionOfPlayer.Instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < manaCrystals.Count; i++) 
        {
            GameObject manaCrystal = manaCrystals[i];
            if (actionOfPlayer.currentMana >= (i + 1))
                manaCrystal.SetActive(true);
            else
                manaCrystal.SetActive(false);
        }
    }
}
