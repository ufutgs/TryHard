using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class camera : MonoBehaviour
{
    public float sensitivity = 5f;
    public float maxYAngle = 80f;
    public float radius = 8f;
    public float x_offset = 1f;
    public float y_offset = 3f;
    private Vector3 currentRotation;
    public Transform lookAt;
    private Vector3 previous;
    private LayerMask wall;


    // Start is called before the first frame update
    void Start()
    {
        wall = LayerMask.GetMask("wall");
        currentRotation = transform.transform.localRotation.eulerAngles;
        Camera.main.transform.LookAt(lookAt.transform);
        /*Initialization*/
        float R = radius * Mathf.Cos(currentRotation.x * Mathf.Deg2Rad) + x_offset;
        float angle = (currentRotation.y - 90) * Mathf.Deg2Rad;
        previous = new Vector3(R * Mathf.Cos(angle), radius * Mathf.Sin(currentRotation.x * Mathf.Deg2Rad) + y_offset, R * Mathf.Sin(angle));
        
    }
    void Update()
    {
        
        CircleAround();
        Vector3 position = previous + lookAt.position;
        position = Vector3.MoveTowards(wall_collide(lookAt.position, position),wall_collide(transform.position,position),0.001f);
        transform.position = position;
        transform.LookAt(lookAt.transform);
       // transform.rotation = Quaternion.Euler(currentRotation.x, transform.rotation.y, transform.rotation.z);
    }

    private void CircleAround()
    {
        if (Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0) return;
        /*Calculating orbit of camera movement around lookAt */
        currentRotation.y -= Input.GetAxis("Mouse X") * sensitivity;
        if (currentRotation.y < -360) currentRotation.y += 360;
        currentRotation.x -= Input.GetAxis("Mouse Y") * sensitivity;
        currentRotation.x = Mathf.Clamp(currentRotation.x, -maxYAngle, maxYAngle);
        float R = radius * Mathf.Cos(currentRotation.x * Mathf.Deg2Rad) + x_offset;
        float angle = (currentRotation.y - 90) * Mathf.Deg2Rad;
        previous = new Vector3(R * Mathf.Cos(angle), radius * Mathf.Sin(currentRotation.x * Mathf.Deg2Rad) + y_offset, R * Mathf.Sin(angle));

    }
    /// <param name="start"> start point.</param>
    /// <param name="end">end point. It is recommend to use future_camera as end.</param>
    private Vector3 wall_collide(Vector3 start, Vector3 end)
    {
        Vector3 diff = end - start;
        RaycastHit hit;
       // Debug.DrawRay(end, (start - end), Color.red, 1);
        if (Physics.Linecast(start, end, out hit, wall))
        {
           return new Vector3(hit.point.x - Mathf.Sign(diff.x) * 0.1f, hit.point.y - Mathf.Sign(diff.y) * 0.1f, hit.point.z - Mathf.Sign(diff.z) * 0.1f);
        }
        return end;
    }

}
