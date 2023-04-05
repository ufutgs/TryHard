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
    private float y_offset = 0f;
    private Vector3 currentRotation;
    public Transform lookAt;
    private Vector3 previous;
    private LayerMask wall;

    // Start is called before the first frame update
    void Start()
    {
        wall = LayerMask.GetMask("wall");
        currentRotation = transform.transform.localRotation.eulerAngles;
        Camera.main.transform.LookAt(lookAt);
        /*Initialization*/
        float R = radius * Mathf.Cos(currentRotation.x * Mathf.Deg2Rad) + x_offset;
        float angle = (currentRotation.y - 90) * Mathf.Deg2Rad;
        previous = new Vector3(R * Mathf.Cos(angle), radius * Mathf.Sin(currentRotation.x * Mathf.Deg2Rad) + y_offset, R * Mathf.Sin(angle));
        Transform cam = Camera.main.transform;
    }
    void Update()
    {

        CircleAround();
        Vector3 new_pos = previous + lookAt.position;
        RaycastHit[] L = Physics.RaycastAll(new Ray(new_pos, -previous.normalized), Vector3.Distance(new_pos, lookAt.position), wall);
        if (L.Length > 0)
        {
            RaycastHit hit = max(L);
            Debug.DrawRay(new_pos, hit.point, Color.green);
            Debug.Log(hit.collider.gameObject.name);
            new_pos = Vector3.MoveTowards(Camera.main.transform.position, hit.point, 0.1f);
        }
        Camera.main.transform.position = new_pos;
        Debug.DrawRay(Camera.main.transform.position, lookAt.position, Color.red);
        Camera.main.transform.LookAt(lookAt);
        Ray[] raylist = mapping(Camera.main.transform);
        foreach(Ray ray in raylist)
        {
            RaycastHit Rhit;
            Debug.DrawRay(new_pos, ray.direction, Color.yellow);
            if (Physics.Raycast(new_pos, ray.direction, out Rhit,0.1f, wall))
            {
                new_pos -= ray.direction.normalized * 0.1f;
            }
        }
        Camera.main.transform.position = new_pos;

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
