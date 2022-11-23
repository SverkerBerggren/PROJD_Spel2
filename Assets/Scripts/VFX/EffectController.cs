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
  
    
    private Dictionary<string, GameObject> shields; //sort champions name and it's shiled prefab ALT sort champion ist för name
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

        shields = new Dictionary<string, GameObject>();
        InvokeRepeating(nameof(ShieldPosition), 0.05f, 1f);
    }

    private void ShieldPosition()
    {
        List<string> champName = shields.Keys.ToList();
        List<GameObject> champs = shields.Values.ToList();

        for (int i = 0; i < shields.Count; i++)
        {
            if (GameState.Instance.playerChampion.champion.championName.Equals(champName[i]))
            {
                champs[i].transform.position = GameState.Instance.playerChampion.transform.position;
            }
            
            foreach (AvailableChampion champ in GameState.Instance.playerChampions)
            {
                if (champ.champion.championName.Equals(champName[i]))
                {
                    champs[i].transform.position = champ.transform.position;
                }
            }
            
        }        
    }

    //two parameters, which champion should have shiled and how much  
    //the shield shall only has tre state, on, half-on, and disappear
    public void ActiveShield(GameObject champions, int shiledAmount)
    {
        //shiled effect 100 procent
        //Set upp shield effect here at champions position 
        //can get shileds value throuht AvailableChampions.shield
        //if the champion doesn't has any shield before, instantiate a new
        //otherwise change shiled value from invisible to visible
        //ALT: set shiled as child to champion
        string champName = champions.GetComponent<AvailableChampion>().champion.championName;


        if (!shields.ContainsKey(champName))
        {
            Vector3 shieldPos = new Vector3(champions.transform.position.x, champions.transform.position.y + 3, champions.transform.position.z);
            GameObject toStore = Instantiate(shieldPrefab, shieldPos, Quaternion.identity); //the GO should have Shieldeffect script
            shields.Add(champName, toStore);            
        }
            
        //champions.shield = shiledAmount;
    }
    public void DestroyShield(Champion champion)
    {   //shiled effect 0 procent
        //this champion's shiled should be destroys 
        
        shieldToGo = shields[champion.championName];
        shieldToGo.GetComponent<Shieldeffect>().Disslove(); 
        shields.Remove(champion.championName);
        //apply the fade out effect
    }

    public void GainHealingEffect(GameObject go)
    {
        Instantiate(healingPrefab, go.transform.position, Quaternion.identity);
    }


}
