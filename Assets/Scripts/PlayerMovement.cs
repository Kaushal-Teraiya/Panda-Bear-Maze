using UnityEngine;

public class MobilePlayerMovement : MonoBehaviour
{
    public FloatingJoystick joystick;
    public Transform cameraTransform;

    public float moveSpeed = 7f;
    public float sprintMultiplier = 1.5f;

    public float rotationSpeed = 12f;
    public float directionSmoothTime = 0.12f;

    public bool sprintActive = false;

    CharacterController controller;
    Animator animator;

    Vector3 currentMoveDirection;
    Vector3 moveDirectionVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float keyboardX = Input.GetAxis("Horizontal");
        float keyboardZ = Input.GetAxis("Vertical");

        float joystickX = joystick.Horizontal;
        float joystickZ = joystick.Vertical;

        float horizontal =
            Mathf.Abs(keyboardX) > Mathf.Abs(joystickX)
            ? keyboardX
            : joystickX;

        float vertical =
            Mathf.Abs(keyboardZ) > Mathf.Abs(joystickZ)
            ? keyboardZ
            : joystickZ;

        Vector3 inputDirection =
            new Vector3(horizontal, 0f, vertical);

        float moveAmount = inputDirection.magnitude;

        animator.SetFloat("Speed", moveAmount);

        if (moveAmount > 0.1f)
        {
            float targetAngle =
                Mathf.Atan2(inputDirection.x, inputDirection.z)
                * Mathf.Rad2Deg
                + cameraTransform.eulerAngles.y;

            Quaternion targetRotation =
                Quaternion.Euler(0f, targetAngle, 0f);

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );

            Vector3 targetMoveDirection =
                targetRotation * Vector3.forward;

            currentMoveDirection =
                Vector3.SmoothDamp(
                    currentMoveDirection,
                    targetMoveDirection,
                    ref moveDirectionVelocity,
                    directionSmoothTime
                );


            float speed = moveSpeed;

            if (sprintActive)
                speed *= sprintMultiplier;


            controller.Move(
                currentMoveDirection * speed * Time.deltaTime
            );
        }
    }
}