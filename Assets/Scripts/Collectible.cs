using UnityEngine;
using System.Collections;

public class CollectibleContainer : MonoBehaviour
{
    public enum CollectibleType
    {
        Key,
        Bomb,
        Heart,
        Banana
    }

    [Header("Collectible Type")]
    public CollectibleType collectibleType;

    [Header("Prefabs")]
    public GameObject glowSpherePrefab;
    public GameObject pickedObjectPrefab;

    [Header("References")]
    public Transform player;
    public Transform handSocket;
    public Animator playerAnimator;

    [Header("Movement")]
    public MonoBehaviour playerMovementScript;

    public PickupUIButton pickupUIButton;

    [Header("Settings")]
    public float pickupDistance = 2f;
    public float attachDelay = 0.9f;

    public string pickupTrigger = "Pickup";
    public string pickupStateName = "Pickup";

    public Renderer glowSphereRenderer;

    GameObject glowSphere;
    GameObject pickedObject;

    bool pickedUp = false;
    bool contentsSpawned = false;


    void Start()
    {
        SpawnContentsOnce();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        pickupUIButton.gameObject.SetActive(false);
    }


    void SpawnContentsOnce()
    {
        if (contentsSpawned) return;

        contentsSpawned = true;


        // Visible collectible in world
        glowSphere = Instantiate(
            glowSpherePrefab,
            transform.position,
            Quaternion.identity,
            transform
        );

        glowSphereRenderer =
            glowSphere.GetComponentInChildren<Renderer>();


        // Hidden object used only during pickup animation
        pickedObject = Instantiate(
            pickedObjectPrefab,
            transform.position,
            Quaternion.identity,
            transform
        );

        pickedObject.SetActive(false);
    }


    public void ApplyRandomColor(Color color)
    {
        if (glowSphereRenderer == null)
            return;

        Material mat = glowSphereRenderer.material;

        float emissionIntensity = 7f;

        if (mat.HasProperty("_BaseColor"))
            mat.SetColor("_BaseColor", color);

        if (mat.HasProperty("_Color"))
            mat.SetColor("_Color", color);

        mat.EnableKeyword("_EMISSION");

        mat.SetColor(
            "_EmissionColor",
            color * Mathf.Pow(2f, emissionIntensity)
        );

        mat.globalIlluminationFlags =
            MaterialGlobalIlluminationFlags.RealtimeEmissive;
    }


    void Update()
    {
        if (pickedUp)
            return;

        float distance =
            Vector3.Distance(player.position, glowSphere.transform.position);

        bool isClosest =
            pickupUIButton.currentContainer == null ||
            pickupUIButton.currentContainer == this ||
            Vector3.Distance(
                player.position,
                pickupUIButton.currentContainer.glowSphere.transform.position
            ) > distance;

        if (distance <= pickupDistance && isClosest)
        {
            pickupUIButton.gameObject.SetActive(true);

            pickupUIButton.currentContainer = this;

            if (Input.GetKeyDown(KeyCode.E))
                Pickup();
        }
        else if (pickupUIButton.currentContainer == this)
        {
            pickupUIButton.gameObject.SetActive(false);

            pickupUIButton.currentContainer = null;
        }
    }


    public void Pickup()
    {
        if (pickedUp)
            return;

        pickedUp = true;

        playerMovementScript.enabled = false;

        playerAnimator.SetTrigger(pickupTrigger);

        StartCoroutine(UnlockMovementWhenAnimationEnds());

        glowSphere.SetActive(false);

        Invoke(nameof(AttachObjectToHand), attachDelay);
    }


    public void PickupFromUIButton()
    {
        if (!pickedUp)
            Pickup();
    }


    IEnumerator UnlockMovementWhenAnimationEnds()
    {
        yield return null;

        while (!playerAnimator
            .GetCurrentAnimatorStateInfo(0)
            .IsName(pickupStateName))
            yield return null;

        while (playerAnimator
            .GetCurrentAnimatorStateInfo(0)
            .normalizedTime < 1f)
            yield return null;

        playerMovementScript.enabled = true;

        HandleCollectibleEffect();
    }


    public virtual void HandleCollectibleEffect()
    {
        if (collectibleType == CollectibleType.Key)
        {
            Inventory.Instance.AddKey();
        }
        else if (collectibleType == CollectibleType.Bomb)
        {
            StartCoroutine(ExplodePlayer());
            return;
        }
        else if (collectibleType == CollectibleType.Heart)
        {
            Inventory.Instance.AddBubble();
        }
        else if (collectibleType == CollectibleType.Banana)
        {
            Inventory.Instance.AddBanana();
        }

        Destroy(pickedObject);
        Destroy(glowSphere);
        Destroy(gameObject);
    }


    IEnumerator ExplodePlayer()
    {
        yield return new WaitForSeconds(3f);

        PlayerRagdollController ragdoll =
            player.GetComponent<PlayerRagdollController>();

        if (ragdoll != null)
        {
            ragdoll.ActivateRagdoll(
                pickedObject.transform.position,
                800f
            );
        }

        Destroy(pickedObject);
        Destroy(gameObject);
    }


    void AttachObjectToHand()
    {
        pickedObject.SetActive(true);

        pickedObject.transform.SetParent(handSocket);

        pickedObject.transform.localPosition = Vector3.zero;


        if (pickedObject.name.Contains("Heart"))
        {
            pickedObject.transform.localRotation =
                Quaternion.Euler(-90f, 90f, 0f);

            pickedObject.transform.localScale =
                Vector3.one * 2.19f;
        }
        else if (pickedObject.name.Contains("Banana"))
        {
            pickedObject.transform.localPosition =
                new Vector3(0.104f, 0.379f, 0.143f);

            pickedObject.transform.localRotation =
                Quaternion.Euler(66.221f, -131.9f, 80.415f);

            pickedObject.transform.localScale =
                new Vector3(0.0414f, 0.0414f, 0.0414f);
        }
        else
        {
            pickedObject.transform.localRotation =
                Quaternion.identity;
        }

        pickupUIButton.gameObject.SetActive(false);

        pickupUIButton.currentContainer = null;
    }
}