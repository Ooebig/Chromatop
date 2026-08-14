using UnityEngine;

public class damage : MonoBehaviour
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] int team;

    [SerializeField] gameManager.ColorType dmgColor;
    [SerializeField] Rigidbody rb;

    [SerializeField] float damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] float bulletSpeed;
    [SerializeField] int bulletDestroyTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = transform.forward * bulletSpeed;
        Destroy(gameObject, bulletDestroyTime);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || other.gameObject.layer == ignoreLayer) return;

        iDamage dmg = other.GetComponent<iDamage>();

        if (dmg != null && dmg.Team != team)
        {
            dmg.takeDamage(damageAmount, dmgColor);
            Destroy(gameObject);
        }
        else if (dmg == null)
        {
            Destroy(gameObject);
        }
    }
}