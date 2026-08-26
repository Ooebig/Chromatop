using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private int team;

    [Header("Damage")]
    [SerializeField] public gameManager.ColorType dmgColor;
    [SerializeField] public float damageAmount;
    [SerializeField] public float damageRate;
    [SerializeField] public bool destroyOnImpact;

    [Header("Projectile")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] public float bulletSpeed;
    [SerializeField] public float bulletDestroyTime;


    private Dictionary<iDamage, float> damageTimers =
        new Dictionary<iDamage, float>(); //dictionary so each thing can be hit individually with its own timer


    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

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

    public void Configure(
        float newDamage,
        float newSpeed,
        float newLifetime,
        gameManager.ColorType newColor,
        int ownerTeam,
        Material newMaterial
    )
    {
        damageAmount = newDamage;
        bulletSpeed = newSpeed;
        bulletDestroyTime = newLifetime;
        dmgColor = newColor;
        team = ownerTeam;

        MeshRenderer meshRenderer =
            GetComponentInChildren<MeshRenderer>(true);

        if (meshRenderer != null && newMaterial != null)
        {
            meshRenderer.material = newMaterial;
        }

        ApplyVelocity();
    }

    private void ApplyVelocity()
    {
        if (rb != null && bulletSpeed != 0f)
        {
            rb.linearVelocity =
                transform.forward * bulletSpeed;
        }
    }

    private bool TryGetDamageTarget(
    Collider other,
    out iDamage target
)
    {
        target = null;

        // Pickup detection colliders are not combat hitboxes.
        if (other.GetComponentInParent<PickupRangeController>() != null)
        {
            return false;
        }

        if ((ignoreLayer.value &
             (1 << other.gameObject.layer)) != 0)
        {
            return false;
        }

        target = other.GetComponentInParent<iDamage>();

        if (target == null || target.Team == team)
        {
            return false;
        }

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!destroyOnImpact)
            return;

        if (!TryGetDamageTarget(other, out iDamage target))
            return;

        target.takeDamage(
            damageAmount,
            dmgColor
        );

        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (destroyOnImpact)
            return;

        if (!TryGetDamageTarget(other, out iDamage target))
            return;

        if (!damageTimers.ContainsKey(target))
        {
            damageTimers[target] = 0f;
        }

        if (Time.time < damageTimers[target])
            return;

        target.takeDamage(
            damageAmount,
            dmgColor
        );

        damageTimers[target] =
            Time.time + Mathf.Max(0.01f, damageRate);
    }

    private void OnTriggerExit(Collider other)
    {
        iDamage target =
            other.GetComponentInParent<iDamage>();

        if (target != null)
        {
            damageTimers.Remove(target);
        }
    }
}