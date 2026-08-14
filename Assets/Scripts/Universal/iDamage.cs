using UnityEngine;

public interface iDamage
{
    public void takeDamage(float amount, gameManager.ColorType dmgColor);
    int Team { get; }
}
