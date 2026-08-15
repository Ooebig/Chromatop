using System.Collections;
using UnityEngine;

/// Handles player health, damage, temporary invincibility, and death.
///
/// The player loses health when damaged by enemies.
/// When health reaches zero, movement and auto-targeting are disabled.
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]

    [Tooltip("The player's maximum health.")]
    [SerializeField] private int maxHealth = 5;

    [Tooltip("How long the player is invincible after taking damage.")]
    [SerializeField] private float invincibilityDuration = 1f;

    // Current player health during gameplay.
    private int currentHealth;

    // Tracks whether the player is temporarily invincible.
    private bool isInvincible;

    // Tracks whether the player is already dead.
    private bool isDead;

    private void Awake()
    {
        // Start the player with full health.
        currentHealth = maxHealth;
    }

    /// Damages the player by a specific amount.
    public void TakeDamage(int damageAmount)
    {
        // Do not take damage if already dead or temporarily invincible.
        if (isDead || isInvincible)
        {
            return;
        }

        // Subtract damage from health.
        currentHealth -= damageAmount;

        Debug.Log(
            "Player took " +
            damageAmount +
            " damage. Health left: " +
            currentHealth
        );

        // Check if the player should die.
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // Temporarily prevent additional damage.
        StartCoroutine(InvincibilityTimer());
    }

    /// Handles the temporary invincibility period after taking damage.
    private IEnumerator InvincibilityTimer()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
    }

    /// Handles player death.
    private void Die()
    {
        // Prevent this function from running more than once.
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log("Game Over! Player died.");

        // Stop player movement.
        PlayerMovement movement =
            GetComponent<PlayerMovement>();

        if (movement != null)
        {
            movement.enabled = false;
        }

        // Stop automatic targeting.
        AutoTargeting autoTargeting =
            GetComponent<AutoTargeting>();

        if (autoTargeting != null)
        {
            autoTargeting.enabled = false;
        }

   
    }

    /// Returns the player's current health.
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    /// Returns the player's maximum health.
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    /// Returns whether the player is dead.
    public bool IsDead()
    {
        return isDead;
    }

    /// Increases maximum health and heals the player by the same amount.
    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;

        Debug.Log("Max health increased to: " + maxHealth);
    }

    /// Allows another system to manually control player invincibility
    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }

    /// Returns whether the player is currently invincible.
    public bool IsInvincible()
    {
        return isInvincible;
    }
}