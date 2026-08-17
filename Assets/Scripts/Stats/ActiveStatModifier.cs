using UnityEngine;

public class ActiveStatModifier
{
    public Object source;
    public StatModifier modifier;

    public ActiveStatModifier(
        Object source,
        StatModifier modifier
    )
    {
        this.source = source;
        this.modifier = modifier;
    }
}