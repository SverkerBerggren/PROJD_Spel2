using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AvailableChampion : MonoBehaviour
{
	// Start is called before the first frame update
    private GameObject passiveTextPlayer;
    private GameObject passiveTextOpponent;
    private GameState gameState;
    private EffectController effectController;
    private Slider healthBarSlider;
    private TMP_Text healthBarText;
	private Image currentSprite;

    [SerializeField] private ShieldShow shieldShow;
    [SerializeField] private TMP_Text passiveEffect;
    [SerializeField] private GameObject shieldUIObject;
    [SerializeField] private GameObject healthBar;
	[SerializeField] private GameObject imageHolder;
	[SerializeField] private GameObject meshToShow;

    [NonSerialized] public GameObject TargetingEffect;

	public Champion Champion;
	public string NameOfChampion;
    public int Health;
	public int MaxHealth;
    public int Shield;
    public bool IsOpponent = false;
	public Animator Animator;

    private void Awake()
    {
        SetupHealthBar();
    }

    private void Start()
	{
        gameState = GameState.Instance;
        effectController = EffectController.Instance;
        MaxHealth = Health;

        if (transform.Find("TargetingEffect") != null)
        {
            TargetingEffect = transform.Find("TargetingEffect").gameObject;
            TargetingEffect.SetActive(false);
			gameState.TargetingEffect = TargetingEffect;
        }

        passiveTextPlayer = GameObject.Find("ChampionOpponentPassive");
        passiveTextOpponent = GameObject.Find("ChampionPlayerPassive");

        currentSprite = imageHolder.GetComponent<Image>();
        SetupChampion();
    }

	private void SetupHealthBar()
    {
        healthBarText = healthBar.GetComponent<ChangeTextWithSlider>().TextToChange;
        healthBarSlider = healthBar.GetComponent<Slider>();
        healthBarSlider.maxValue = MaxHealth;
        healthBarSlider.value = MaxHealth;
    }

    private void SetupChampion()
    {
        NameOfChampion = Champion.ChampionName;
        passiveEffect.text = Champion.PassiveEffect;
        Health = Champion.Health;
        MaxHealth = Champion.MaxHealth;

        if (gameState.PlayerChampion.Champion == Champion || gameState.OpponentChampion.Champion == Champion)
        {
            meshToShow = Instantiate(Champion.ChampionMesh, transform);

            if (meshToShow.TryGetComponent(out Animator)) {};
        }
    }

    private void ChangeChampionMesh()
    {
        Destroy(meshToShow);
        meshToShow = Instantiate(Champion.ChampionMesh, transform);
        Animator = meshToShow.GetComponent<Animator>();
    }

	private void DamageShield(int damage)
	{
		if (damage >= Champion.Shield)
		{
			int differenceAfterShieldDamage = damage - Champion.Shield;
			Champion.Shield = 0;
			shieldUIObject.SetActive(false);
			Tuple<string, bool> tuple = new Tuple<string, bool>(Champion.ChampionName, IsOpponent);
			EffectController.Instance.DestroyShield(tuple);
			Champion.Health -= differenceAfterShieldDamage;
		}
		else
		{
			Champion.Shield -= damage;
			shieldShow.ChangeShieldTextTo(Champion.Shield);
		}
	}

	public void UpdateTextOnCard()
    {
        if (Champion == null) return;

        NameOfChampion = Champion.ChampionName;
        Health = Champion.Health;
        MaxHealth = Champion.MaxHealth;
        Shield = Champion.Shield;

        currentSprite.sprite = Champion.ChampBackground;
        if (meshToShow != null)
        {
            string[] nameOfChampion = meshToShow.name.Split("(");
 
            if (!Champion.ChampionMesh.name.Equals(nameOfChampion[0]))
                ChangeChampionMesh();
        }

        Champion.UpdatePassive();

        if (passiveEffect.text != null)
            passiveEffect.text = Champion.PassiveEffect;

        if (healthBarSlider == null)
            SetupHealthBar();

        if (Shield == 0)        
            shieldUIObject.SetActive(false);
        else
        {
            shieldUIObject.SetActive(true);
            shieldShow.ChangeShieldTextTo(Champion.Shield);
        }

        healthBarSlider.maxValue = MaxHealth;

        if (Health <= 0)
            healthBarSlider.value = 0;
        else
            healthBarSlider.value = Health;

        healthBarText.text = Health.ToString() + "/" + MaxHealth.ToString();
	}

    public virtual void TakeDamage(int damage)
    {
        if (Champion.Shield == 0)
            Champion.Health -= damage;
        else
            DamageShield(damage);

        if (Champion.Health <= 0)
        {
		    if ((gameState.PlayerChampions.Count == 1 && gameState.PlayerChampion.Champion == Champion)
            || (gameState.OpponentChampions.Count == 1 && gameState.OpponentChampion.Champion == Champion))
		    {
			    gameState.cameraAnimator.enabled = true;
			    Invoke(nameof(Death), 3.0f);
		    }
            else
                Death();
        }
    }

    public void Death()
    {
        if (Animator != null)
            Animator.SetTrigger("Death");
	    effectController.PlayDeathEffect(this);
        gameState.ChampionDeath(Champion);
    }

    public virtual void HealChampion(int amountToHeal)
    {
        Champion.Health += amountToHeal;
        if (Champion.Health > Champion.MaxHealth)
            Champion.Health = MaxHealth;
    }
    public virtual void GainShield(int amountToBlock)
    {
        Champion.Shield += amountToBlock;
        shieldUIObject.SetActive(true);

        shieldShow.ChangeShieldTextTo(Champion.Shield);
    }
}
