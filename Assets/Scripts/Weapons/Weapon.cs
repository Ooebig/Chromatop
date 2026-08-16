using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public List<WeaponStats> stats;
    public int weaponLevel;
    public bool statsUpdated;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

[System.Serializable]
public class WeaponStats
{
    public float speed;
    public float damage;
    public float range;
    public float timeBetweenAttacks;
    public float amount;
    public float duration;
}