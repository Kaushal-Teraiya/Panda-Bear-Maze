using UnityEngine;

public class HeightFixer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 6.7f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
