using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterGroundSnap : MonoBehaviour
{
    public float gravity = -20f;
    public float groundStickForce = -5f;

    float verticalVelocity;

    CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = groundStickForce;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
}