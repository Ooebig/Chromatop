using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float moveSpeed;
    public Vector3 offset; //= new Vector3(0, 20, 0);

    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, player.position + offset, moveSpeed * Time.deltaTime);

    }
}
