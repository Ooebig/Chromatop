using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    private CharacterController controller;
    [SerializeField] private float speed;
    [SerializeField] private Transform cam;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float rotationspeed;
    Vector3 velocity;

    [Header("Aiming")]
    public LayerMask whatIsAimMask;
    public Transform aimTransform;
    private Camera mainCamera;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, 0f, y).normalized;

        controller.Move(move * speed * Time.deltaTime);

        if (move.magnitude > 0)
        {
            Quaternion toRotate = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationspeed * Time.deltaTime);
        }

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    private void UpdateAim()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.yellow);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
}

//www.youtube.com/watch?v=Ax94kLWkugg
