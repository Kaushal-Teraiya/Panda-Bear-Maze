using UnityEngine;

public class HideSecretCodeUIOnStart : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
}