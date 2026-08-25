using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] int team;

    [SerializeField] public gameManager.ColorType dmgColor;
    [SerializeField] Rigidbody rb;

    [SerializeField] public float damageAmount;
    [SerializeField] public float damageRate;
    [SerializeField] public float bulletSpeed;
    [SerializeField] public bool destroyOnImpact;
    [SerializeField] public float bulletDestroyTime;

    private Dictionary<iDamage, float> damageTimers =
        new Dictionary<iDamage, float>(); //dictionary so each thing can be hit individually with its own timer

    void Start()
    {
        if (rb != null && bulletSpeed != 0)
        {
            rb.linearVelocity = transform.forward * bulletSpeed;
        }

        if (bulletDestroyTime > 0)
        {
            Destroy(gameObject, bulletDestroyTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.isTrigger)
        //    return;

        if ((ignoreLayer.value & (1 << other.gameObject.layer)) != 0)
            return;

        iDamage dmg = other.GetComponentInParent<iDamage>();

        if (dmg == null || dmg.Team == team)
            return;

        if (!destroyOnImpact)
        {
            if (!damageTimers.ContainsKey(dmg))
            {
                damageTimers[dmg] = 0f;
            }

            if (Time.time >= damageTimers[dmg])
            {
                dmg.takeDamage(damageAmount, dmgColor);

                damageTimers[dmg] = Time.time + damageRate;
            }
            
        }
        else
        {
            dmg.takeDamage(damageAmount, dmgColor);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        iDamage dmg = other.GetComponentInParent<iDamage>();

        if (dmg != null)
        {
            damageTimers.Remove(dmg);
        }
    }
}