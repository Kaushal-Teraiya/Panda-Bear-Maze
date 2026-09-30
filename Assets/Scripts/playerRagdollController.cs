using UnityEngine;
using System.Collections;

public class PlayerRagdollController : MonoBehaviour
{
    [Header("Ragdoll Camera Offset")]
    public Vector3 ragdollCameraOffset = new Vector3(0f, 0f, -2f);
    Transform cameraOriginalParent;
    bool followRagdollCamera = false;
    public Animator animator;
    public MonoBehaviour movementScript;

    public CharacterController characterController;
    public Collider mainCapsuleCollider;

    [Header("Camera Follow")]
    public Transform cameraRig;              // camera pivot object (NOT camera itself)
    public Transform ragdollFollowBone;      // mixamorig:Spine1
    public Transform normalCameraTarget;     // original camera follow target

    Rigidbody[] ragdollBodies;

    Vector3 respawnPoint;

    void Start()
    {
        ragdollBodies = GetComponentsInChildren<Rigidbody>();

        DisableRagdoll();

        respawnPoint = transform.position;
    }

    public void ActivateRagdoll(Vector3 explosionOrigin, float force)
    {
        movementScript.enabled = false;

        animator.enabled = false;

        if (characterController != null)
            characterController.enabled = false;

        if (mainCapsuleCollider != null)
            mainCapsuleCollider.enabled = false;

        EnableRagdoll();

        SwitchCameraToRagdoll();

        foreach (Rigidbody rb in ragdollBodies)
        {
            float multiplier =
                rb.name.Contains("mixamorig:Spine1") ? 1.8f : 1f;

            rb.AddForceAtPosition(
                force * Vector3.up,
                animator.rootPosition,
                ForceMode.Impulse
            );
        }

        StartCoroutine(RespawnRoutine());
    }

    void EnableRagdoll()
    {
        foreach (Rigidbody rb in ragdollBodies)
            rb.isKinematic = false;
    }

    void DisableRagdoll()
    {
        foreach (Rigidbody rb in ragdollBodies)
            rb.isKinematic = true;
    }

    void SwitchCameraToRagdoll()
    {
        if (cameraRig != null && ragdollFollowBone != null)
        {
            cameraOriginalParent = cameraRig.parent;

            followRagdollCamera = true;
        }
    }

    void SwitchCameraBack()
    {
        followRagdollCamera = false;

        if (cameraRig != null && cameraOriginalParent != null)
        {
            cameraRig.SetParent(cameraOriginalParent);
            cameraRig.localPosition = Vector3.zero;
        }
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(5f);

        DisableRagdoll();

        animator.enabled = true;

        transform.position = respawnPoint;

        if (characterController != null)
            characterController.enabled = true;

        if (mainCapsuleCollider != null)
            mainCapsuleCollider.enabled = true;

        movementScript.enabled = true;

        SwitchCameraBack();
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        respawnPoint = checkpointPosition;
    }

    void LateUpdate()
{
    if (followRagdollCamera && cameraRig != null && ragdollFollowBone != null)
    {
        Vector3 targetPosition =
            ragdollFollowBone.position + ragdollCameraOffset;

        cameraRig.position = Vector3.Lerp(
            cameraRig.position,
            targetPosition,
            12f * Time.deltaTime
        );
    }
}
}