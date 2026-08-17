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

    private void Update()
    {
        // Temporary testing input.
        if (Input.GetKeyDown(KeyCode.T))
        {
            takeDamage(
                10f,
                gameManager.ColorType.GREY
            );
        }
    }

    public void takeDamage(
        float amount,
        gameManager.ColorType dmgColor
    )
    {
        float damage = gameManager.damageCalc(
            amount,
            gameManager.ColorType.GREY,
            dmgColor
        );

        if (playerStats != null)
        {
            damage *=
                1f - playerStats.DamageReduction;
        }

        currentHealth = Mathf.Max(
            0f,
            currentHealth - damage
        );

        if (currentHealth <= 0f)
        {
            gameObject.SetActive(false);
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