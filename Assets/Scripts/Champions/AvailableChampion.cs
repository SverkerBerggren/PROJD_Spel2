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

	public string nameOfChampion;
    public int health;
	public int maxHealth;
    public int shield;

    public bool isOpponent = false;

    public GameObject meshToShow;
    private GameObject builderMesh;
    private GameObject cultistMesh;
    private GameObject graverobberMesh;
    private GameObject theOneDrawsMesh;
    private GameObject shankerMesh;
    private GameObject duelistMesh;

    private GameObject passiveTextPlayer;
    private GameObject passiveTextOpponent;

	public Animator animator;

	private bool wantToSeInfoOnChamp = false;

    private float timer = 0f;
    private float timeBeforeShowing = 0.5f;


    public SpriteRenderer champCard;
    private GameState gameState;
    //private ArmorEffect armorEffect;

    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text shieldText;
    [SerializeField] private TMP_Text passiveEffect;

    [NonSerialized] public GameObject targetingEffect;

    //public SpriteRenderer artwork;

    private void Awake()
	{
        nameOfChampion = champion.championName;
        //artwork.sprite = champion.artwork;
        passiveEffect.text = champion.passiveEffect;
        health = champion.health;
        maxHealth = champion.maxHealth;
	}

	private void Start()
	{
        gameState = GameState.Instance;
        maxHealth = health;
        if (transform.Find("ArmorEffect") != null)
           // armorEffect = transform.Find("ArmorEffect").GetComponent<ArmorEffect>();
        Invoke(nameof(SetWichMeshToShowOnStart),0.05f);

        if (transform.Find("TargetingEffect") != null)
        {
            targetingEffect = transform.Find("TargetingEffect").gameObject;
            targetingEffect.SetActive(false);
            GameState.Instance.targetingEffect = targetingEffect;
        }

        passiveTextPlayer = GameObject.Find("ChampionOpponentPassive");
        passiveTextOpponent = GameObject.Find("ChampionPlayerPassive");
        

        GetAllMeshes();
    }
    private void GetAllMeshes()
    {
        builderMesh = transform.Find("Builder").gameObject;
        cultistMesh = transform.Find("Cultist").gameObject;
        graverobberMesh = transform.Find("Graverobber").gameObject;
        theOneDrawsMesh = transform.Find("TheOneWhoDraws").gameObject;
        shankerMesh = transform.Find("Shanker").gameObject;
        duelistMesh = transform.Find("Duelist").gameObject;
    }

    private void SetWichMeshToShowOnStart()
    {
        switch(meshToShow.name)
        {
            case "Builder":
                builderMesh.SetActive(true);
                animator = builderMesh.GetComponentInChildren<Animator>();
                break;
            case "Cultist":
                cultistMesh.SetActive(true);
                animator = cultistMesh.GetComponentInChildren<Animator>();
                break;
            case "Graverobber":
                graverobberMesh.SetActive(true);
                animator = graverobberMesh.GetComponentInChildren<Animator>();
                break;
            case "Duelist":
                duelistMesh.SetActive(true);
                animator = null;
                break;
			case "TheOneWhoDraws":
				theOneDrawsMesh.SetActive(true);
				animator = null;
				break;
			case "Shanker":
				shankerMesh.SetActive(true);
				animator = null;
				break;
		}
    }

    private void OnMouseEnter()
    {
        if (GameState.Instance.playerChampion == champion || GameState.Instance.opponentChampion == champion)
        {
            passiveTextOpponent.SetActive(false);
            passiveTextPlayer.SetActive(false);
        }
        wantToSeInfoOnChamp = true;
    }

    private void OnMouseExit()
    {
        if (GameState.Instance.playerChampion == champion || GameState.Instance.opponentChampion == champion)
        {
            passiveTextOpponent.SetActive(true);
            passiveTextPlayer.SetActive(true);
        }
        wantToSeInfoOnChamp = false;
        champCard.sprite = null;
        timer = 0f;
    }

    public void UpdateTextOnCard()
    {
        if (champion == null) return;

        nameOfChampion = champion.championName;
        health = champion.health;
        maxHealth = champion.maxHealth;
        shield = champion.shield;

        champion.UpdatePassive();
        if (passiveEffect.text != null)
            passiveEffect.text = champion.passiveEffect;

        healthText.text = health + "/" + maxHealth;
        if (shieldText != null)
        {
            if (shield > 0)
            {
                shieldText.text = "Shield: " + shield;
            }
            else
            {
                shieldText.text = "";
            }
        }

	}

    public void FixedUpdate()
	{
        SwapMesh();

        if (wantToSeInfoOnChamp)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= timeBeforeShowing)
                champCard.sprite = champion.artwork;
        }
	}

    public virtual void TakeDamage(int damage)
    {

        if (shield == 0)
        {
            health -= damage;
        }
        else
        {
            if (damage >= shield)
            {
                int differenceAfterShieldDamage = damage - shield;
                shield = 0;

                ShieldEffectDestroy();

                health -= differenceAfterShieldDamage;
            }
            else
            {
                shield -= damage;
            }
        }

        if (health <= 0)
        {
            Death();
        }
    }
    public virtual void Death()
    {
        gameState.ChampionDeath(champion);
    }

    private void ShieldEffectDestroy()
    {
        Tuple<string, bool> tuple = new Tuple<string, bool>(champion.championName, isOpponent);
        EffectController.Instance.DestroyShield(tuple);
    }

    public virtual void HealChampion(int amountToHeal)
    {
        health += amountToHeal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

    }
    public virtual void GainShield(int amountToBlock)
    {
        shield += amountToBlock;
    }



    private void SwapMesh()
    {
        switch (champion.championName)
        {
            case "Builder":
                builderMesh.SetActive(true);
                cultistMesh.SetActive(false);
                graverobberMesh.SetActive(false);
                duelistMesh.SetActive(false);
                shankerMesh.SetActive(false);
                theOneDrawsMesh.SetActive(false);
                animator = builderMesh.GetComponentInChildren<Animator>();
                meshToShow = builderMesh;
                break;

            case "Cultist":
                builderMesh.SetActive(false);
                cultistMesh.SetActive(true);
                graverobberMesh.SetActive(false);
				duelistMesh.SetActive(false);
				shankerMesh.SetActive(false);
				theOneDrawsMesh.SetActive(false);
				animator = cultistMesh.GetComponentInChildren<Animator>();
                meshToShow = cultistMesh;
                break;

            case "Graverobber":
                builderMesh.SetActive(false);               
                cultistMesh.SetActive(false);
                graverobberMesh.SetActive(true);
				duelistMesh.SetActive(false);
				shankerMesh.SetActive(false);
				theOneDrawsMesh.SetActive(false);
				animator = graverobberMesh.GetComponentInChildren<Animator>();
                meshToShow = graverobberMesh;
                break;

			case "Duelist":
				builderMesh.SetActive(false);
				cultistMesh.SetActive(false);
				graverobberMesh.SetActive(false);
				duelistMesh.SetActive(true);
				shankerMesh.SetActive(false);
				theOneDrawsMesh.SetActive(false);
				meshToShow = duelistMesh;
				animator = null;
				break;

			case "Shanker":
				builderMesh.SetActive(false);
				cultistMesh.SetActive(false);
				graverobberMesh.SetActive(false);
				duelistMesh.SetActive(false);
				shankerMesh.SetActive(true);
				theOneDrawsMesh.SetActive(false);
				meshToShow = shankerMesh;
				animator = null;
				break;

			case "TheOneWhoDraws":
				builderMesh.SetActive(false);
				cultistMesh.SetActive(false);
				graverobberMesh.SetActive(false);
				duelistMesh.SetActive(false);
				shankerMesh.SetActive(false);
				theOneDrawsMesh.SetActive(true);
				meshToShow = theOneDrawsMesh;
				animator = null;
				break;
		}
    }
}
