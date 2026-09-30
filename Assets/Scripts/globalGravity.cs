using UnityEngine;

public class PhysicsScaler : MonoBehaviour
{
    void Awake()
    {
        Physics.gravity = new Vector3(0f, -80f, 0f);
    }
}