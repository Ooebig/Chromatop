using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float baseMaxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Movement")]
    [SerializeField] private float baseMovementSpeed = 5f;

    [Header("Progression")]
    [SerializeField] private float baseExperienceGain = 1f;
    [SerializeField] private float basePickupRange = 2f;

    private readonly List<ActiveStatModifier> activeModifiers =
        new List<ActiveStatModifier>();

    public event Action<float, float> HealthChanged;
    public event Action StatsChanged;

    public float CurrentHealth => currentHealth;

    public float MaxHealth =>
        Calculate(StatType.MaxHealth, baseMaxHealth);

    public float MovementSpeed =>
        Calculate(StatType.MovementSpeed, baseMovementSpeed);

    public float ExperienceGainMultiplier =>
        Calculate(
            StatType.ExperienceGain,
            baseExperienceGain
        );

    public float PickupRange =>
        Calculate(StatType.PickupRange, basePickupRange);

    public float DamageReduction =>
        Mathf.Clamp(
            Calculate(StatType.DamageReduction, 0f),
            0f,
            0.75f
        );

    private void Awake()
    {
        currentHealth = MaxHealth;
    }

    public float Calculate(
        StatType statType,
        float baseValue
    )
    {
        float flatBonus = 0f;
        float percentageBonus = 0f;

        foreach (ActiveStatModifier activeModifier in activeModifiers)
        {
            StatModifier modifier =
                activeModifier.modifier;

            if (modifier.statType != statType)
                continue;

            switch (modifier.modifierType)
            {
                case StatModifierType.Flat:
                    flatBonus += modifier.value;
                    break;

                case StatModifierType.Percentage:
                    percentageBonus += modifier.value;
                    break;
            }
        }

        return Mathf.Max(
            0f,
            (baseValue + flatBonus) *
            (1f + percentageBonus)
        );
    }

    public void AddModifiers(
        UnityEngine.Object source,
        IEnumerable<StatModifier> modifiers
    )
    {
        if (source == null || modifiers == null)
            return;

        foreach (StatModifier modifier in modifiers)
        {
            if (modifier == null)
                continue;

            activeModifiers.Add(
                new ActiveStatModifier(
                    source,
                    modifier
                )
            );
        }

        ClampCurrentHealth();
        StatsChanged?.Invoke();
    }

    public void RemoveModifiers(
        UnityEngine.Object source
    )
    {
        if (source == null)
            return;

        activeModifiers.RemoveAll(
            modifier => modifier.source == source
        );

        ClampCurrentHealth();
        StatsChanged?.Invoke();
    }

    public void TakeDamage(float incomingDamage)
    {
        if (incomingDamage <= 0f)
            return;

        float finalDamage =
            incomingDamage * (1f - DamageReduction);

        currentHealth = Mathf.Max(
            0f,
            currentHealth - finalDamage
        );

        HealthChanged?.Invoke(
            currentHealth,
            MaxHealth
        );

        if (currentHealth <= 0f)
        {
            Die();
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

        HealthChanged?.Invoke(
            currentHealth,
            MaxHealth
        );
    }

    public void RestoreFullHealth()
    {
        currentHealth = MaxHealth;

        HealthChanged?.Invoke(
            currentHealth,
            MaxHealth
        );
    }

    private void ClampCurrentHealth()
    {
        currentHealth = Mathf.Clamp(
            currentHealth,
            0f,
            MaxHealth
        );

        HealthChanged?.Invoke(
            currentHealth,
            MaxHealth
        );
    }

    private void Die()
    {
        Debug.Log("Player died.", this);
        gameObject.SetActive(false);
    }
}
