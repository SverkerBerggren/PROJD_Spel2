using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    //access to all effect prefab
    //Set up effect when it calls
    // Start is called before the first frame update
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private GameObject healingPrefab;
  
    
    private Dictionary<Tuple<string,bool>, GameObject> shields; //sort champions name and it's shiled prefab ALT sort champion ist för name
    private GameObject shieldToGo;
    //for controlling propety in shader graph, for simulate a fade out effec

    private static EffectController instance;

    public static EffectController Instance { get { return instance; } set { instance = value; } }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        shields = new Dictionary<Tuple<string, bool>, GameObject>();
    }

    private void FixedUpdate()
    {
        ShieldPosition();   
    }

    private void ShieldPosition()
    {
        foreach(Tuple<string,bool> availableChampion in shields.Keys)
        {
            if(availableChampion.Item2 == false)
            {
                foreach(AvailableChampion champOnField in GameState.Instance.playerChampions)
                {
                    if(champOnField.champion.championName.Equals(availableChampion.Item1))  
                    {
                        shields[availableChampion].transform.position = champOnField.transform.position;
                    }
                }
            }
            else
            {
                foreach(AvailableChampion champOnField in GameState.Instance.opponentChampions)
                {
                    if(champOnField.champion.championName.Equals(availableChampion.Item1))  
                    {
                        shields[availableChampion].transform.position = champOnField.transform.position;
                    }
                }
            }
        }      
    }

    //two parameters, which champion should have shiled and how much  
    //the shield shall only has tre state, on, half-on, and disappear
    public void ActiveShield(Tuple<string,bool> tupleShields, int shieldAmount, GameObject gameObject)
    {
        //shiled effect 100 procent
        //Set upp shield effect here at champions position 
        //can get shileds value throuht AvailableChampions.shield
        //if the champion doesn't has any shield before, instantiate a new
        //otherwise change shiled value from invisible to visible
        //ALT: set shiled as child to champion


        if (!shields.ContainsKey(tupleShields))
        {
            Vector3 shieldPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 3, gameObject.transform.position.z);
            GameObject toStore = Instantiate(shieldPrefab, shieldPos, Quaternion.identity); //the GO should have Shieldeffect script
            shields.Add(tupleShields, toStore);            
        }
            
        //champions.shield = shiledAmount;
    }
    public void DestroyShield(Tuple<string,bool> champion)
    {   //shiled effect 0 procent
        //this champion's shiled should be destroys 
        
        shieldToGo = shields[champion];
        shieldToGo.GetComponent<Shieldeffect>().Disslove(); 
        shields.Remove(champion);
        //apply the fade out effect
    }

    public void GainHealingEffect(GameObject go)
    {
        Instantiate(healingPrefab, go.transform.position, Quaternion.identity);
    }


}
