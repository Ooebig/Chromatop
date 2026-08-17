using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float baseMaxHealth = 100f;

    [Header("Movement")]
    [SerializeField] private float baseMovementSpeed = 6f;

    [Header("Progression")]
    [SerializeField] private float baseExperienceGain = 1f;
    [SerializeField] private float basePickupRange = 2f;

    [Header("Calculated Stats - Runtime")]
    [SerializeField] private float displayedMaxHealth;
    [SerializeField] private float displayedMovementSpeed;
    [SerializeField] private float displayedExperienceGain;
    [SerializeField] private float displayedPickupRange;
    [SerializeField] private float displayedDamageReduction;

    private readonly List<ActiveStatModifier> activeModifiers =
        new List<ActiveStatModifier>();

    public event Action StatsChanged;

    private void Awake()
    {
        RefreshDisplayedStats();
    }

    public float MaxHealth =>
        Calculate(
            StatType.MaxHealth,
            baseMaxHealth
        );

    public float MovementSpeed =>
        Calculate(
            StatType.MovementSpeed,
            baseMovementSpeed
        );

    public float ExperienceGainMultiplier =>
        Calculate(
            StatType.ExperienceGain,
            baseExperienceGain
        );

    public float PickupRange =>
        Calculate(
            StatType.PickupRange,
            basePickupRange
        );

    public float DamageReduction =>
        Mathf.Clamp(
            Calculate(
                StatType.DamageReduction,
                0f
            ),
            0f,
            0.75f
        );

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

            if (modifier == null ||
                modifier.statType != statType)
            {
                continue;
            }

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

        float finalValue =
            (baseValue + flatBonus) *
            (1f + percentageBonus);

        return Mathf.Max(0f, finalValue);
    }

    public void AddModifiers(
    UnityEngine.Object source,
    IEnumerable<StatModifier> modifiers
)
    {
        if (source == null || modifiers == null)
            return;

        RemoveModifiersFromList(source);

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

        RefreshDisplayedStats();
        StatsChanged?.Invoke();
    }

    public void RemoveModifiers(
    UnityEngine.Object source
)
    {
        if (source == null)
            return;

        bool removed =
            RemoveModifiersFromList(source);

        if (removed)
        {
            RefreshDisplayedStats();
            StatsChanged?.Invoke();
        }
    }

    private bool RemoveModifiersFromList(
        UnityEngine.Object source
    )
    {
        int removedAmount =
            activeModifiers.RemoveAll(
                modifier =>
                    modifier.source == source
            );

        return removedAmount > 0;
    }

    private void OnValidate()
    {
        baseMaxHealth =
            Mathf.Max(1f, baseMaxHealth);

        baseMovementSpeed =
            Mathf.Max(0f, baseMovementSpeed);

        baseExperienceGain =
            Mathf.Max(0f, baseExperienceGain);

        basePickupRange =
            Mathf.Max(0f, basePickupRange);

        StatsChanged?.Invoke();

        RefreshDisplayedStats();
    }

    private void RefreshDisplayedStats()
    {
        displayedMaxHealth = MaxHealth;
        displayedMovementSpeed = MovementSpeed;
        displayedExperienceGain = ExperienceGainMultiplier;
        displayedPickupRange = PickupRange;
        displayedDamageReduction = DamageReduction;
    }
}