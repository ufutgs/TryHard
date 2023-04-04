using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public float sensitivity = 5f;
    public float maxYAngle = 90f;
    public float radius = 12f;
    public float y_radius = 8f;
    private float currentx = 0f;
    private float x_offset = 1f;
    private float y_offset = 4f;
    private Vector3 currentRotation;
    // Start is called before the first frame update
    void Start()
    {
        currentRotation = transform.transform.localRotation.eulerAngles;
    }
    void Update()
    {
        currentx = currentRotation.y + Input.GetAxis("Mouse X") * sensitivity; 
        currentRotation.y += Input.GetAxis("Mouse X") * sensitivity;
        if (currentRotation.y > 0) currentRotation.y -= 360;
        currentRotation.x -= Input.GetAxis("Mouse Y") * sensitivity;
        currentRotation.x = Mathf.Clamp(currentRotation.x, -maxYAngle, maxYAngle);
        //currentRotation = Vector3.SmoothDamp(Camera.main.transform.rotation)
         Camera.main.transform.localRotation = Quaternion.Euler(currentRotation.x, -currentRotation.y, 0);
        float R = radius *Mathf.Cos(currentRotation.x * Mathf.Deg2Rad) + x_offset;
        float angle = (currentx - 90) * Mathf.Deg2Rad;
        Camera.main.transform.transform.localPosition = new Vector3(R*Mathf.Cos(angle),y_radius*Mathf.Sin(currentRotation.x*Mathf.Deg2Rad)+y_offset, R * Mathf.Sin(angle));
    }
}
