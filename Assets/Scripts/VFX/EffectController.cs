using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectController : MonoBehaviour
{
    //access to all effect prefab
    //Set up effect when it calls
    // Start is called before the first frame update
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private GameObject healingPrefab;
    [SerializeField] private GameObject cultistAttackPrefab;
    [SerializeField] private Transform targetPos;
    [SerializeField] private ParticleSystem onHit;
    
    private Dictionary<Tuple<string,bool>, GameObject> shields; //sort champions name and it's shiled prefab ALT sort champion ist för name
    private GameObject shieldToGo;
    //for controlling propety in shader graph, for simulate a fade out effec
    
    

    private static EffectController instance;

    public static EffectController Instance { get { return instance; } set { instance = value; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        shields = new Dictionary<Tuple<string, bool>, GameObject>();
        //should have to know where to spwn Cultist attack effect 
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
                foreach(AvailableChampion champOnField in GameState.Instance.PlayerChampions)
                {
                    if(champOnField.Champion.ChampionName.Equals(availableChampion.Item1))  
                    {
                        //shields[availableChampion].transform.position = champOnField.transform.position;
                        shields[availableChampion].transform.position = new Vector3(champOnField.transform.position.x, champOnField.transform.position.y + 9f, champOnField.transform.position.z);
                        if (GameState.Instance.PlayerChampion.Champion.ChampionName.Equals(availableChampion.Item1))
                        {
                            shields[availableChampion].gameObject.SetActive(true);
                        }
                        else
                        {
                            shields[availableChampion].gameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                foreach(AvailableChampion champOnField in GameState.Instance.OpponentChampions)
                {
                    if(champOnField.Champion.ChampionName.Equals(availableChampion.Item1))  
                    {
                        // shields[availableChampion].transform.position = champOnField.transform.position;
                        shields[availableChampion].transform.position = new Vector3(champOnField.transform.position.x, champOnField.transform.position.y + 9f, champOnField.transform.position.z);
                        if (GameState.Instance.OpponentChampion.Champion.ChampionName.Equals(availableChampion.Item1))
                        {
                            shields[availableChampion].gameObject.SetActive(true);
                        }
                        else
                        {
                            shields[availableChampion].gameObject.SetActive(false);
                        }
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
          
          Vector3 shieldPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 9f, gameObject.transform.position.z);
            GameObject toStore = Instantiate(shieldPrefab, shieldPos, Quaternion.identity); //the GO should have Shieldeffect script
            shields.Add(tupleShields, toStore);            
        }
            
        //champions.shield = shiledAmount;
    }
    public void DestroyShield(Tuple<string,bool> champion)
    {   //shiled effect 0 procent
        //this champion's shiled should be destroys 
        
        shieldToGo = shields[champion];
        shieldToGo.GetComponent<ShieldEffect>().Disslove(); 
        shields.Remove(champion);
        //apply the fade out effect
    }

    public void GainHealingEffect(GameObject go)
    {
        Instantiate(healingPrefab, go.transform.position, Quaternion.identity);
    }

    public void GainCultistAttackEffect() { 
        Instantiate(cultistAttackPrefab, targetPos.position, Quaternion.identity);
    }

    public void DiscardCardEffect(GameObject card)
    {
      //  card.GetComponent<CardDissolve>().SetDissolveState(true);
    }


    public void PlayAttackEffect(AvailableChampion holder)
    {
    
        switch (holder.Champion.ChampionName)
        {
            case "Cultist":
                GainCultistAttackEffect();
                break;
            case "Builder": break;
           
            case "Duelist":
                holder.GetComponentInChildren<ParticleSystem>().Play();
                break;
            case "Graverobber": break;
            case "TheOneWhoDraws": break;
            case "Shanker":
                holder.GetComponentInChildren<EffectChampions>().attackVFX.Play();
                break;
        }
        onHit.Play();
    }

    public void PlayDeathEffect(AvailableChampion holder)
    {
        if (holder.Champion.ChampionName.Equals("Shanker"))
            return;

           holder.GetComponentInChildren<EffectChampions>().StartDisolve();

    }
    
}
