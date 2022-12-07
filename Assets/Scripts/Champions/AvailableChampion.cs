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

    private GameObject passiveTextPlayer;
    private GameObject passiveTextOpponent;
    public Image currentSprite;

	public Animator animator;

	private bool wantToSeInfoOnChamp = false;

    private float timer = 0f;
    private float timeBeforeShowing = 0.5f;


    public SpriteRenderer champCard;
    private GameState gameState;
    //private ArmorEffect armorEffect;
    [SerializeField] private GameObject sheildUIObject;

    [SerializeField] private TMP_Text passiveEffect;

    [NonSerialized] public GameObject targetingEffect;

    public GameObject imageHolder;


    [SerializeField] private GameObject healthBar;
    private Slider healthBarSlider;
    private TMP_Text healthBarText;

    //public SpriteRenderer artwork;

    private void Awake()
    {
        SetupHealthBar();
    }

    private void Start()
	{
        gameState = GameState.Instance;
        maxHealth = health;
        if (transform.Find("ArmorEffect") != null)
           // armorEffect = transform.Find("ArmorEffect").GetComponent<ArmorEffect>();

        

        if (transform.Find("TargetingEffect") != null)
        {
            targetingEffect = transform.Find("TargetingEffect").gameObject;
            targetingEffect.SetActive(false);
            GameState.Instance.targetingEffect = targetingEffect;
        }

        passiveTextPlayer = GameObject.Find("ChampionOpponentPassive");
        passiveTextOpponent = GameObject.Find("ChampionPlayerPassive");

        currentSprite = imageHolder.GetComponent<Image>();


        SetupChampion();
        //GetAllMeshes();

        /*        if ((gameState.playerChampion == this || gameState.opponentChampion == this))*/

    }


    private void SetupHealthBar()
    {
        healthBarText = healthBar.GetComponent<ChangeTextWithSlider>().textToChange;
        healthBarSlider = healthBar.GetComponent<Slider>();
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = maxHealth;
    }

    private void SetupChampion()
    {
        nameOfChampion = champion.championName;
        //artwork.sprite = champion.artwork;
        passiveEffect.text = champion.passiveEffect;
        health = champion.health;
        maxHealth = champion.maxHealth;

        meshToShow = Instantiate(champion.championMesh, transform);

        if (meshToShow.GetComponentInChildren<Animator>() != null) 
            animator = meshToShow.GetComponentInChildren<Animator>();
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

    private void ChangeChampionMesh()
    {
        Destroy(meshToShow);
        meshToShow = Instantiate(champion.championMesh, transform);
    }

    public void UpdateTextOnCard()
    {
        if (champion == null) return;

        nameOfChampion = champion.championName;
        health = champion.health;
        maxHealth = champion.maxHealth;
        shield = champion.shield;

        currentSprite.sprite = champion.champBackground;

        ChangeChampionMesh();
        champion.UpdatePassive();
        if (passiveEffect.text != null)
            passiveEffect.text = champion.passiveEffect;

        if (healthBarSlider == null)
            SetupHealthBar();

        if (shield == 0)        
            sheildUIObject.SetActive(false);
        

        healthBarSlider.maxValue = maxHealth;
        if (health <= 0)
        {
            healthBarSlider.value = 0;
            // Show X on dead champ here
        }
        else
            healthBarSlider.value = health;

        healthBarText.text = health.ToString() + "/" + maxHealth.ToString();
	}

    public void FixedUpdate()
	{
        if (wantToSeInfoOnChamp)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= timeBeforeShowing)
                champCard.sprite = champion.artwork;
        }
	}

    public virtual void TakeDamage(int damage)
    {
        if (champion.shield == 0)
        {
            champion.health -= damage;
        }
        else
        {
            if (damage >= champion.shield)
            {
                int differenceAfterShieldDamage = damage - champion.shield;
                champion.shield = 0;
                sheildUIObject.SetActive(false);
                ShieldEffectDestroy();

                champion.health -= differenceAfterShieldDamage;
            }
            else
            {
                champion.shield -= damage;
                sheildUIObject.GetComponent<ShieldShow>().ChangeShieldTextTo(champion.shield);
            }
        }

        if (champion.health <= 0)
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
        champion.health += amountToHeal;
        if (champion.health > champion.maxHealth)
        {
            champion.health = maxHealth;
        }

    }
    public virtual void GainShield(int amountToBlock)
    {
        champion.shield += amountToBlock;
        sheildUIObject.SetActive(true);
        sheildUIObject.GetComponent<ShieldShow>().ChangeShieldTextTo(champion.shield);
    }
}
