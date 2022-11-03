using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    //access to all effect prefab
    //Set up effect when it calls
    // Start is called before the first frame update
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private GameObject healingPrefab;
  
    
    private Dictionary<string, GameObject> shields; //sort champions name and it's shiled prefab ALT sort champion ist för name
    private GameObject shiledToGo;
    //for controlling propety in shader graph, for simulate a fade out effec

    void Start()
    {
        shields = new Dictionary<string, GameObject>();

      }

    // Update is called once per frame
    void Update()
    {

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
        GameObject toStore = Instantiate(shieldPrefab,champions.transform.position,Quaternion.identity); //the GO should have Shieldeffect script
       
        shields.Add(champions.name, toStore);
        //champions.shield = shiledAmount;
    }
    public void DesstoryShield(GameObject champion)
    {   //shiled effect 0 procent
        //this champion's shiled should be destroys 
        shiledToGo = shields[champion.name];
        shiledToGo.GetComponent<Shieldeffect>().Disslove(); 
        //apply the fade out effect
    }

    public void GainHealingEffect(GameObject go)
    {
        Instantiate(healingPrefab, champions.transform.position, Quaternion.identity);
    }


}
