using UnityEngine;

/// Handles top-down player movement.
/// Uses a CharacterController on the X/Z plane.
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Fallback Movement")]
    [Tooltip("Used only when PlayerStats cannot be found.")]
    [SerializeField] private float fallbackMoveSpeed = 6f;

    private CharacterController characterController;
    private PlayerStats playerStats;

    private Vector3 moveDirection;

    public float CurrentMoveSpeed =>
        playerStats != null
            ? playerStats.MovementSpeed
            : fallbackMoveSpeed;

    private void Awake()
    {
        characterController =
            GetComponent<CharacterController>();

        playerStats = GetComponent<PlayerStats>();

        if (playerStats == null)
        {
            playerStats =
                GetComponentInParent<PlayerStats>();
        }
    }

    private void Update()
    {
        PlayerMovementInput();
    }

    private void PlayerMovementInput()
    {
        float horizontalInput =
            Input.GetAxisRaw("Horizontal");

        float verticalInput =
            Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(
            horizontalInput,
            0f,
            verticalInput
        );

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }

        characterController.Move(
            moveDirection *
            CurrentMoveSpeed *
            Time.deltaTime
        );
    }

    /// Adds a permanent runtime movement-speed modifier.
    public void IncreaseMoveSpeed(float amount)
    {
        if (amount <= 0f)
            return;

        if (playerStats == null)
        {
            fallbackMoveSpeed += amount;
            return;
        }

        StatModifier speedModifier =
            new StatModifier
            {
                statType = StatType.MovementSpeed,
                modifierType = StatModifierType.Flat,
                value = amount
            };

        playerStats.AddModifiers(
            this,
            new[] { speedModifier }
        );

        Debug.Log(
            $"Move speed increased to: {CurrentMoveSpeed}",
            this
        );
    }

    public Vector3 GetMoveInput()
    {
        return moveDirection;
    }

    public float GetMoveSpeed()
    {
        return CurrentMoveSpeed;
    }
}