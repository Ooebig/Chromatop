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
}


