using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed = 10f;

    [SerializeField] private float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position +=
            transform.forward * moveSpeed * Time.deltaTime;
    }

    public void SetLifeTime(float newLifeTime)
    {
        lifeTime = newLifeTime;
    }
}