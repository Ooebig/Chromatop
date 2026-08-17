using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, iDamage
{
    public float currentHealth, maxHealth;


    int iDamage.Team => 1;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            takeDamage(10f, gameManager.ColorType.GREY);
        }
    }


    public void takeDamage(float amount, gameManager.ColorType dmgColor)
    {
        float damage = gameManager.damageCalc(amount, gameManager.ColorType.GREY, dmgColor);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}