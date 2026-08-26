using UnityEngine;

/// Tracks player XP and level.
/// Experience-gain relics are calculated through PlayerStats.
public class PlayerExperience : MonoBehaviour
{
    [Header("Experience Settings")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXP;
    [SerializeField] private int xpToNextLevel = 200;

    [Min(1f)]
    [SerializeField] private float xpRequirementMultiplier = 1.25f;

    private PlayerStats playerStats;

    public int CurrentLevel => currentLevel;
    public int CurrentXP => currentXP;
    public int XPToNextLevel => xpToNextLevel;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();

        if (playerStats == null)
        {
            playerStats =
                GetComponentInParent<PlayerStats>();
        }
    }

    /// Adds XP after applying the player's experience multiplier.
    public void AddXP(int baseAmount)
    {
        if (baseAmount <= 0)
            return;

        float experienceMultiplier =
            playerStats != null
                ? playerStats.ExperienceGainMultiplier
                : 1f;

        int finalAmount = Mathf.Max(
            1,
            Mathf.RoundToInt(
                baseAmount * experienceMultiplier
            )
        );


        Data.totalExperience += finalAmount;
        currentXP += finalAmount;

        Debug.Log(
            $"XP gained: {finalAmount}. " +
            $"Current XP: {currentXP}/{xpToNextLevel}",
            this
        );

        CheckForLevelUp();
    }

    private void CheckForLevelUp()
    {
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            currentLevel++;

            xpToNextLevel = Mathf.RoundToInt(
                xpToNextLevel *
                xpRequirementMultiplier
            );

            Debug.Log(
                $"LEVEL UP! New level: {currentLevel}",
                this
            );
        }
        gameManager.instance.updatePlayerEXP(currentXP, xpToNextLevel);
    }
}