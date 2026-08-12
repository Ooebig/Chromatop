using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    private CharacterController controller;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravity = 9.81f;

    private CharacterController characterController;
    private Camera mainCamera;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private Vector3 targetPoint;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        mainCamera = Camera.main;
    }

    void Update()
    {
        PlayerMovementInput();
        PlayerAiming();
        ApplyGravity();
    }

    private void PlayerMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        // Keep movement on the X/Z ground plane
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        moveDirection =
            cameraRight * horizontalInput +
            cameraForward * verticalInput;

        moveDirection.Normalize();

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void PlayerAiming()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, transform.position);

        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            targetPoint = ray.GetPoint(rayDistance);

            Vector3 lookDirection = targetPoint - transform.position;
            lookDirection.y = 0f; // Prevent rotation in the y-axis

            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = targetRotation;
            }
        }
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y <0)
        {
            velocity.y = -2f; // Small negative value to keep the player grounded
        }

        velocity.y -= gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}

//www.youtube.com/watch?v=Ax94kLWkugg
//medium.com/@cwagoner78/3d-top-down-shooter-that-follows-mouse-for-aiming-project-log-d1956ddaba3
