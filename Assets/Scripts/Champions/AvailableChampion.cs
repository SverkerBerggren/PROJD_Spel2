using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AvailableChampion : MonoBehaviour
{
	// Start is called before the first frame update
	public Champion champion;

	public new string name;
    public int health;
	public int maxHealth;
    public int shield;

    public GameObject meshToShow;
    public GameObject builderMesh;
    public GameObject cultistMesh;
    public GameObject graverobberMesh;
    private bool wantToSeInfoOnChamp = false;

    private float timer = 0f;
    private float timeBeforeShowing = 0.5f;

    public SpriteRenderer champCard;
    //private ArmorEffect armorEffect;



    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text shieldText;
    [SerializeField] private TMP_Text passiveEffect;

    [NonSerialized] public GameObject targetingEffect;

    //public SpriteRenderer artwork;

    private void Awake()
	{
        name = champion.name;
        //artwork.sprite = champion.artwork;
        passiveEffect.text = champion.passiveEffect;
        health = champion.health;
        maxHealth = champion.maxHealth;
		//InvokeRepeating(nameof(Deal5Damage), 5, 2);
	}

	private void Start()
	{
        maxHealth = health;
        if (transform.Find("ArmorEffect") != null)
           // armorEffect = transform.Find("ArmorEffect").GetComponent<ArmorEffect>();
        SetWichMeshToShowOnStart();

        if (transform.Find("TargetingEffect") != null)
        {
            targetingEffect = transform.Find("TargetingEffect").gameObject;
            targetingEffect.SetActive(false);
            GameState.Instance.targetingEffect = targetingEffect;
        }
    }

    private void SetWichMeshToShowOnStart()
    {
        switch(meshToShow.name)
        {
            case "Builder":
                builderMesh.SetActive(true);
                champion.animator = builderMesh.GetComponentInChildren<Animator>();
                break;
            case "Cultist":
                cultistMesh.SetActive(true);
                champion.animator = cultistMesh.GetComponentInChildren<Animator>();
                break;
            case "Graverobber":
                graverobberMesh.SetActive(true);
                champion.animator = graverobberMesh.GetComponentInChildren<Animator>();
                break;
        }
    }

    /*
    public void ChangeChampion(Champion champion, int currentHealth, int currentShield)
    {
        this.champion = champion;
        Awake();
        health = currentHealth;
        shield = currentShield;
    }
    */

    private void OnMouseEnter()
    {
        wantToSeInfoOnChamp = true;
    }

    private void OnMouseExit()
    {
        wantToSeInfoOnChamp = false;
        champCard.sprite = null;
        timer = 0f;
    }

    private void UpdateTextOnCard()
    {
        if (champion == null) return;

        name = champion.name;
        health = champion.health;
        maxHealth = champion.maxHealth;
        shield = champion.shield;
        passiveEffect.text = champion.passiveEffect;
        healthText.text = health + "/" + maxHealth;
        //shieldText.text = shield + "";
	}

    public void FixedUpdate()
	{
        if (champion.destroyShield)
        {
            //armorEffect.DamageArmor(10);
            print("RUns");
        }
            
		UpdateTextOnCard();

        if (meshToShow.name != champion.name)
            SwapMesh();

        if (wantToSeInfoOnChamp)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= timeBeforeShowing)
                champCard.sprite = champion.artwork;
        }
	}

    private void SwapMesh()
    {
        switch (champion.name)
        {
            case "Builder":
                builderMesh.SetActive(true);
                cultistMesh.SetActive(false);
                graverobberMesh.SetActive(false);
                champion.animator = builderMesh.GetComponentInChildren<Animator>();
                meshToShow = builderMesh;
                break;
            case "Cultist":
                builderMesh.SetActive(false);
                cultistMesh.SetActive(true);
                graverobberMesh.SetActive(false);
                champion.animator = cultistMesh.GetComponentInChildren<Animator>();
                meshToShow = cultistMesh;
                break;
            case "Graverobber":
                builderMesh.SetActive(false);               
                cultistMesh.SetActive(false);
                graverobberMesh.SetActive(true);
                champion.animator = graverobberMesh.GetComponentInChildren<Animator>();
                meshToShow = graverobberMesh;
                break;
        }
    }
}
