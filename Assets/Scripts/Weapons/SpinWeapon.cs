using UnityEngine;

public class SpinWeapon : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float orbitDistance = 2f;

    private float currentAngle;

    private void Update()
    {
        if (player == null)
            return;

        currentAngle += rotateSpeed * Time.deltaTime;
        float angle = currentAngle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(
            Mathf.Cos(angle) * orbitDistance,
            0f,
            Mathf.Sin(angle) * orbitDistance
        );

        transform.position = player.position + offset;
    }
}