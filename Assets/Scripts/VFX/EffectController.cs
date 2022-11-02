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
  
    
    private Dictionary<string, GameObject> shields; //sort champions name and it's shiled prefab ALT sort champion ist f�r name
    private GameObject shiledToGo;
    //for controlling propety in shader graph, for simulate a fade out effec
    private Renderer shiledArmor;
    private MaterialPropertyBlock m_PropetyBlock;
    private float targetDiss;
    private float currentDiss;
    private int shaderSlide; //the max value of current effect to reach
    private int currentSilde; //the counting value
    void Start()
    {
        shields = new Dictionary<string, GameObject>();
        m_PropetyBlock = new MaterialPropertyBlock();
        targetDiss = currentDiss = 1f;
        shaderSlide = currentSilde = 0;
        //the slides max = 1, min = 0, default = 1,
      }

    // Update is called once per frame
    void Update()
    {
        if(shiledToGo != null)
        {
            //if there is a current shiled should go 

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Get space");
            Instantiate(healingPrefab);
        
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
        GameObject toStore = Instantiate(shieldPrefab,champions.transform.position,Quaternion.identity);
        shields.Add(champions.name, toStore);
        //champions.shield = shiledAmount;
    }
    public void DesstoryShield(GameObject champion)
    {   //shiled effect 0 procent
        //this champion's shiled should be destroys 
        shiledToGo = shields[champion.name];
        shaderSlide = 100;
        //apply the fade out effect
    }
    public void DamageShield()
    {
        //shiled effect 50 procent
        shaderSlide = 50;
    }

    public void ShaderAccount(int dmg)
    {

        SetShaderSlide(Mathf.Min(currentSilde + dmg, 100));
    }
    public void SetShaderSlide(int value)
    {
        currentSilde = value;
        targetDiss = (float)(shaderSlide - currentSilde) / shaderSlide;
    }
}
