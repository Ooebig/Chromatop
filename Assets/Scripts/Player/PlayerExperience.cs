using UnityEngine;
using UnityEngine.SceneManagement;

/// Tracks player XP and level.
/// Leveling up now happens automatically without opening a menu.
public class PlayerExperience : MonoBehaviour
{
    [Header("Experience Settings")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int xpToNextLevel = 200;
    [SerializeField] private float xpRequirementMultiplier = 1.25f;

    /// Adds XP to the player.
    /// Called by XP orbs.
    public void AddXP(int amount)
    {
        currentXP += amount;

        Debug.Log("XP gained: " + amount + ". Current XP: " + currentXP + " / " + xpToNextLevel);

        CheckForLevelUp();
    }

    /// Checks if the player has enough XP to level up.
    private void CheckForLevelUp()
    {
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            currentLevel++;

            xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpRequirementMultiplier);

            Debug.Log("LEVEL UP! New level: " + currentLevel);
        }
    }
}