using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Rendering.HighDefinition;

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
    private EffectController effectController;
    //private ArmorEffect armorEffect;
    [SerializeField] private GameObject sheildUIObject;
    [SerializeField] private ShieldShow shieldShow;

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
        effectController = EffectController.Instance;
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
        nameOfChampion = champion.ChampionName;
        //artwork.sprite = champion.artwork;
        passiveEffect.text = champion.PassiveEffect;
        health = champion.Health;
        maxHealth = champion.MaxHealth;

        if (gameState.playerChampion.champion == champion || gameState.opponentChampion.champion == champion)
        {
            meshToShow = Instantiate(champion.ChampionMesh, transform);

            if (meshToShow.GetComponent<Animator>() != null)
                animator = meshToShow.GetComponent<Animator>();
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

    private void ChangeChampionMesh()
    {
        Destroy(meshToShow);
        meshToShow = Instantiate(champion.ChampionMesh, transform);

        //if (meshToShow.GetComponent<Animator>() != null)
            animator = meshToShow.GetComponent<Animator>();
    }

    public void UpdateTextOnCard()
    {
        if (champion == null) return;

        nameOfChampion = champion.ChampionName;
        health = champion.Health;
        maxHealth = champion.MaxHealth;
        shield = champion.Shield;

        currentSprite.sprite = champion.ChampBackground;
        if (meshToShow != null)
        {
            string[] nameOfChampion = meshToShow.name.Split("(");
 
            if (!champion.ChampionMesh.name.Equals(nameOfChampion[0]))
                ChangeChampionMesh();
        }


        champion.UpdatePassive();
        if (passiveEffect.text != null)
            passiveEffect.text = champion.PassiveEffect;

        if (healthBarSlider == null)
            SetupHealthBar();

        if (shield == 0)        
            sheildUIObject.SetActive(false);
        else
        {
            sheildUIObject.SetActive(true);
            shieldShow.ChangeShieldTextTo(champion.Shield);
        }


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
                champCard.sprite = champion.Artwork;
        }
	}

    public virtual void TakeDamage(int damage)
    {
        if (champion.Shield == 0)
        {
            champion.Health -= damage;
        }
        else
        {
            if (damage >= champion.Shield)
            {
                int differenceAfterShieldDamage = damage - champion.Shield;
                champion.Shield = 0;
                sheildUIObject.SetActive(false);
                ShieldEffectDestroy();

                champion.Health -= differenceAfterShieldDamage;
            }
            else
            {
                champion.Shield -= damage;
                shieldShow.ChangeShieldTextTo(champion.Shield);
            }
        }

        if (champion.Health <= 0)
        {
            if ((gameState.playerChampions.Count == 1 && gameState.playerChampion.champion == champion) || (gameState.opponentChampions.Count == 1 && gameState.opponentChampion.champion == champion))
            {
                gameState.animator.enabled = true;
                Invoke(nameof(Death), 3.0f);
            }

                
            else
                Death();
        }
    }
    public void Death()
    {
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }
        effectController.PlayDeathEffect(this);
        gameState.ChampionDeath(champion);
    }

    private void ShieldEffectDestroy()
    {
        Tuple<string, bool> tuple = new Tuple<string, bool>(champion.ChampionName, isOpponent);
        EffectController.Instance.DestroyShield(tuple);
    }

    public virtual void HealChampion(int amountToHeal)
    {
        champion.Health += amountToHeal;
        if (champion.Health > champion.MaxHealth)
        {
            champion.Health = maxHealth;
        }

    }
    public virtual void GainShield(int amountToBlock)
    {
        champion.Shield += amountToBlock;
        sheildUIObject.SetActive(true);

        shieldShow.ChangeShieldTextTo(champion.Shield);
    }
}
