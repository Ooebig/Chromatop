using UnityEngine;

/// Handles top-down player movement.
/// Uses a CharacterController for movement on the X/Z plane.
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]

    [Tooltip("How fast the player moves around the arena.")]
    [SerializeField] private float moveSpeed = 6f;

    private CharacterController characterController;
    private Vector3 moveDirection;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMovementInput();
    }

    private void PlayerMovementInput()
    {
        // Read A/D or Left/Right arrows.
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Read W/S or Up/Down arrows.
        float verticalInput = Input.GetAxisRaw("Vertical");

        // 3D top-down movement uses the X/Z plane.
        moveDirection = new Vector3(
            horizontalInput,
            0f,
            verticalInput
        );

        // Prevent diagonal movement from being faster.
        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }

        characterController.Move(
            moveDirection * moveSpeed * Time.deltaTime
        );
    }

    /// Increases the player's movement speed.
    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;

        Debug.Log("Move speed increased to: " + moveSpeed);
    }

    /// Returns the player's current movement direction.
    public Vector3 GetMoveInput()
    {
        return moveDirection;
    }

    /// Returns the player's current movement speed.
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}