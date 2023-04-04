using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basic_control : MonoBehaviour
{

    public enum Movement 
        {
            RUN,
            WALK,
            REST
        }
    [System.NonSerialized]
    public Movement state;
    private float[,] table = { {100f,10f }, // A, DA, max_V
                               {5f,8f },
                               {0f,0f },};
    
    private float global_DA = 0;
    private float normal = 0f;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        state = Movement.REST;
        rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float frontwalk = Input.GetAxis("Vertical"); //y
        float sidewalk = Input.GetAxis("Horizontal");//x 
        Vector2 input = new Vector2(sidewalk, frontwalk);
        state = (input.magnitude > 0.5) ? Movement.RUN : (input.magnitude <= 0.01) ? Movement.REST : Movement.WALK;
        Vector2 acceleration = (input==Vector2.zero) ? Vector2.zero : Acceleration(new Vector2(sidewalk,frontwalk));
        rb.velocity = cal_velocity(acceleration,rb.velocity);

    }

    private Vector2 Acceleration(Vector2 input)
    {
        float angle =Mathf.Atan2(input.y,input.x);
        Vector2 temp = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return table[(int) state,0]*temp;
    }
                
    private Vector3 cal_velocity(Vector2 A,Vector3 cur_v)
    {
        float max_velocity = table[(int) state,1];
        Vector2 v2_cur_v = new Vector2(cur_v.x, cur_v.z);
        Vector2 drag = v2_cur_v.normalized *global_DA;
        Vector2 new_v = v2_cur_v + (A - drag)*Time.deltaTime ;
        new_v = (max_velocity< new_v.magnitude) ? new_v*(max_velocity/new_v.magnitude) : new_v;// how to scale A such that cur_v + A .mag == max;
        return new Vector3(new_v.x, normal*Time.deltaTime+cur_v.y, new_v.y);
    }
     void OnCollisionEnter(Collision collision)
    {
        normal = 9.81f;
    }

     void OnCollisionExit(Collision collision)
    {
        normal = 0f;
        Debug.Log("ca");
    }
}
