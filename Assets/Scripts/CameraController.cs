using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float moveSpeed;
    public Vector3 offset = new Vector3(0, 20, 0);
    public float followDistance;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
    }
}
