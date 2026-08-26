using System;
using System.Collections.Generic;
using UnityEngine;



public class PlayerStats : MonoBehaviour
{

    [Header("Health")]
    [SerializeField] private float baseMaxHealth = 100f;
    [SerializeField] private float baseHealthRegeneration = 0f;
    [SerializeField] private float baseDamageReduction = 0f;

    [Header("Movement")]
    [SerializeField] private float baseMovementSpeed = 6f;

    [Header("Offense")]
    [SerializeField] private float baseDamage = 75f;
    [SerializeField] private float baseKnockback = 0f;
    [SerializeField] private float baseCriticalChance = 0.025f;
    [SerializeField] private float baseCriticalDamage = 1.5f;
    [SerializeField] private float baseCooldown = 1f;
    [SerializeField] private float baseAttackInterval = 1f;
    [SerializeField] private float baseWeaponSize = 1f;
    [SerializeField] private float baseWeaponSpeed = 1f;
    [SerializeField] private float baseWeaponDuration = 1f;
    [SerializeField] private float baseProjectileCount = 1f; //Most stats do nothing for now, and are set to default values. Will need to be implemented later.

    [Header("Progression")]
    [SerializeField] private float baseExperienceGain = 1f;
    [SerializeField] private float basePickupRange = 2f;
    [SerializeField] private float baseLuck = 1f;

    [Header("Calculated Stats - Runtime")]
    [SerializeField] private float displayedMaxHealth;
    [SerializeField] private float displayedHealthRegeneration;
    [SerializeField] private float displayedMovementSpeed;
    [SerializeField] private float displayedDamage;
    [SerializeField] private float displayedDamageReduction;
    [SerializeField] private float displayedKnockback;
    [SerializeField] private float displayedCriticalChance;
    [SerializeField] private float displayedCriticalDamage;
    [SerializeField] private float displayedCooldown;
    [SerializeField] private float displayedAttackInterval;
    [SerializeField] private float displayedWeaponSize;
    [SerializeField] private float displayedWeaponSpeed;
    [SerializeField] private float displayedWeaponDuration;
    [SerializeField] private float displayedProjectileCount;
    [SerializeField] private float displayedExperienceGain;
    [SerializeField] private float displayedPickupRange;
    [SerializeField] private float displayedLuck;
    

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

    public float DamageReduction =>
    Mathf.Clamp(
        Calculate(
            StatType.DamageReduction,
            baseDamageReduction
        ),
        0f,
        0.75f
    );

    public float MovementSpeed =>
        Calculate(
            StatType.MovementSpeed,
            baseMovementSpeed
        );

    public float Damage =>
        Calculate(
            StatType.Damage,
            baseDamage
        );

    public float CriticalChance =>
        Calculate(
            StatType.CriticalChance,
            baseCriticalChance
        );

    public float CriticalDamage =>
        Calculate(
            StatType.CriticalDamage,
            baseCriticalDamage
        );

    public float HealthRegeneration =>
        Calculate(
            StatType.HealthRegeneration,
            baseHealthRegeneration
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
        ); //Yeah I'm not adding all the stats rn, I'm pretty rushed as is. If they aren't here, they probably aren't used yet

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
        displayedDamage = Damage;
        displayedDamageReduction = DamageReduction;
        displayedExperienceGain = ExperienceGainMultiplier;
        displayedPickupRange = PickupRange;
    }
}