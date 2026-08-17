using System.Collections.Generic;
using UnityEngine;

public class StatSystem : MonoBehaviour
{
    private readonly List<ActiveStatModifier> activeModifiers =
        new List<ActiveStatModifier>();

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

        float finalValue =
            (baseValue + flatBonus) *
            (1f + percentageBonus);

        return Mathf.Max(0f, finalValue);
    }

    public void AddModifiers(
        Object source,
        IEnumerable<StatModifier> modifiers
    )
    {
        if (source == null || modifiers == null)
            return;

        foreach (StatModifier modifier in modifiers)
        {
            activeModifiers.Add(
                new ActiveStatModifier(
                    source,
                    modifier
                )
            );
        }
    }

    public void RemoveModifiers(Object source)
    {
        activeModifiers.RemoveAll(
            activeModifier =>
                activeModifier.source == source
        );
    }
}
