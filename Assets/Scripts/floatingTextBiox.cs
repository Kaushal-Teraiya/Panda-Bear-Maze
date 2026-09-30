using UnityEngine;
using UnityEngine.UI;

public class FloatingTextbox : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0, 2.2f, 0);

    public Camera cam;

    public Text messageText;


    void LateUpdate()
    {
        if(target == null) return;

        transform.position = target.position + offset;

        if(cam != null)
            transform.forward = cam.transform.forward;
    }


    public void SetText(string msg)
    {
        messageText.text = msg;
    }
}