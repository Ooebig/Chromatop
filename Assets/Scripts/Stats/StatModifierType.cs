using System;

public enum StatModifierType
{
    Flat,
    Percentage
}

[Serializable]
public class StatModifier
{
    public StatType statType;
    public StatModifierType modifierType;

    public float value;
}