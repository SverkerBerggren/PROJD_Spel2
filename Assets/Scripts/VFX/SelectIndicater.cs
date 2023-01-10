using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectIndicater : MonoBehaviour
{
    public GameObject[] landmarkSlots;
    public GameObject[] landmarkSelectBoxs;
    public GameObject championSelectBox;
    // Start is called before the first frame update

    private static SelectIndicater instance;
    public static SelectIndicater Instance { get { return instance; } }

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
  
    }
    //when player is trying to play a attack card, active the landmark indicater box if opponent has landmarks
    //***Speical notice with protectiv walls, only that landmark should have indicater.

    public void UppdateIndicater(CardType tp)
    {
        if (tp != CardType.Attack) return;
   
        championSelectBox.SetActive(true);
        for(int i = 0; i< landmarkSlots.Length; i++)
        {
            //if the landmark is targetable and the slot of landmark prefab is active. then active indicater 
            if (landmarkSlots[i].activeInHierarchy && landmarkSlots[i].GetComponent<Transform>().parent.GetComponent<LandmarkDisplay>().Card.Targetable)
            {              
                landmarkSelectBoxs[i].SetActive(true);
            }
    
        }
      
    }
    public void DisableIndicater()
    {
        championSelectBox.SetActive(false);
        foreach(GameObject go in landmarkSelectBoxs)
        {
            go.SetActive(false);
        }
    }


}
