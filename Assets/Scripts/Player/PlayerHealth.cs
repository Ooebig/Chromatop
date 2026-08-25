using UnityEngine;

public class PlayerHealth : MonoBehaviour, iDamage
{
    [Header("Fallback Health")]
    [Tooltip("Used only when PlayerStats cannot be found.")]
    [SerializeField] private float fallbackMaxHealth = 100f;

    [Header("Runtime Health")]
    [SerializeField] private float currentHealth;

    private PlayerStats playerStats;
    private float previousMaxHealth;

    int iDamage.Team => 1;

    public float CurrentHealth => currentHealth;

    public float MaxHealth =>
        playerStats != null
            ? playerStats.MaxHealth
            : fallbackMaxHealth;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();

        if (playerStats == null)
        {
            playerStats =
                GetComponentInParent<PlayerStats>();
        }

        previousMaxHealth = MaxHealth;
    }

    private void OnEnable()
    {
        if (playerStats != null)
        {
            playerStats.StatsChanged += HandleStatsChanged;
        }
    }

    private void Start()
    {
        currentHealth = MaxHealth;
        previousMaxHealth = MaxHealth;
    }

    private void OnDisable()
    {
        if (playerStats != null)
        {
            playerStats.StatsChanged -= HandleStatsChanged;
        }
    }

    public void takeDamage(
        float amount,
        gameManager.ColorType dmgColor
    )
    {

        //Debug.Log("Player Took Dmg " + amount);
        float damage = gameManager.damageCalc(
            amount,
            gameManager.instance.activeColor,
            dmgColor
        );
        //Debug.Log("A " + damage);
        if (playerStats != null)
        {
            //Debug.Log("Playerstats null");
            damage *=
                1f - playerStats.DamageReduction;
        }
        //Debug.Log("B " + damage);
        currentHealth = Mathf.Max(
            0f,
            currentHealth - damage
        );
        gameManager.instance.updatePlayerHP(currentHealth, MaxHealth);
        gameManager.instance.DamageFlash(dmgColor);
        if (currentHealth <= 0f)
        {
            Debug.Log("Playerdeath");
            gameManager.instance.youLose();
            //gameObject.SetActive(false);
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0f)
            return;

        currentHealth = Mathf.Min(
            currentHealth + amount,
            MaxHealth
        );
    }

    private void HandleStatsChanged()
    {
        float newMaxHealth = MaxHealth;
        float difference = newMaxHealth - previousMaxHealth;

        // Increasing maximum health also grants the added health.
        if (difference > 0f)
        {
            currentHealth += difference;
        }

        currentHealth = Mathf.Clamp(
            currentHealth,
            0f,
            newMaxHealth
        );

        previousMaxHealth = newMaxHealth;
    }
}