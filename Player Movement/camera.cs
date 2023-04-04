using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public float sensitivity = 5f;
    public float maxYAngle = 80f;
    public float radius = 12f;
    private float currentx = 0f;
    private Vector3 currentRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        currentx = currentRotation.y + Input.GetAxis("Mouse X") * sensitivity; 
        currentRotation.y += Input.GetAxis("Mouse X") * sensitivity;
        if (currentRotation.y > 0) currentRotation.y -= 360;
       // currentRotation.x -= Input.GetAxis("Mouse Y") * sensitivity;
        //currentRotation.x = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
        //currentRotation = Vector3.SmoothDamp(Camera.main.transform.rotation)
         Camera.main.transform.localRotation = Quaternion.Euler(currentRotation.x, -currentRotation.y, 0);
        float y = Camera.main.transform.transform.localPosition.y;
        Camera.main.transform.transform.localPosition = new Vector3(radius*Mathf.Cos((currentx-90)*Mathf.Deg2Rad),y, radius * Mathf.Sin((currentx - 90) * Mathf.Deg2Rad));
    }
}
