using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class camera : MonoBehaviour
{
    public float sensitivity = 5f;
    public float maxYAngle = 90f;
    public float radius = 8f;
    private float x_offset = 1f;
    private float y_offset = 3.5f;
    private Vector3 currentRotation;
    public GameObject lookAt;
    private Vector3 previous;
    private LayerMask wall;
    public Transform future_camera;

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
        future_camera.position = previous + lookAt.transform.position;
        Vector3 Y_dir = new Vector3(0, transform.position.y - future_camera.position.y, 0);
        RaycastHit hit;
        Debug.DrawRay(future_camera.position, (transform.position - future_camera.position), Color.red,1);
        if (Physics.Linecast(future_camera.position,transform.position,out hit, wall))
        {
            future_camera.position = new Vector3(future_camera.position.x, hit.point.y + Y_dir.normalized.y*0.1f , future_camera.position.z);
        }
        future_camera.LookAt(lookAt.transform);
        /*Ray[] raylist = mapping(Camera.main.transform);
        foreach(Ray ray in raylist)
        {
            RaycastHit Rhit;
            Debug.DrawRay(future_camera.position, ray.direction, Color.yellow);
            if (Physics.Raycast(new_pos, ray.direction, out Rhit,0.1f, wall))
            {
                new_pos -= ray.direction.normalized * 0.1f;
            }
        }*/
        transform.position = future_camera.position;
        transform.LookAt(lookAt.transform);
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

    private Ray[] mapping(Transform cam)
    {
        return   new Ray[] { new Ray(cam.position, cam.up),
                 new Ray(cam.position, -cam.up),
                 new Ray(cam.position, cam.right),
                 new Ray(cam.position, -cam.right),
                 new Ray(cam.position, cam.forward),
                 new Ray(cam.position, -cam.forward)
    };
    }

    public RaycastHit max(RaycastHit[] list)
    {
        RaycastHit h = list[0];
        foreach (RaycastHit i in list)
            h = (i.distance > h.distance) ? i : h;
        return h;
    }
}
