using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private float global_DA = 1f;
    private Rigidbody rb;
    private Animator animator;
    private KeyCode[] keybind = {KeyCode.LeftShift};
    private string[] table = { "Sprint"}; //transition table
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Debug.Log(rb.velocity.magnitude);
        float frontwalk = Input.GetAxisRaw("Vertical");
        float sidewalk = Input.GetAxisRaw("Horizontal");
        if (frontwalk != 0 || sidewalk != 0)
        {
            //set all related transtition into true or false
            animator.SetBool("Run", true);
        }
        else 
        {
            //set all related transtition into true or false
            animator.SetBool("Run", false);
        }
        for(int i = 0; i<keybind.Length;i++)
        {
            if (Input.GetKey(keybind[i]))
            {
                animator.SetBool(table[i], true);
            }
            else 
            {
                animator.SetBool(table[i], false);
            }
        }
        
    }
    public void Moving(float max_speed,float A)
    {
        float frontwalk = Input.GetAxisRaw("Vertical"); //y
        float sidewalk = Input.GetAxisRaw("Horizontal");//x 
        Vector2 input = new Vector2(sidewalk, frontwalk);
        //calculation
        Vector2 acceleration = (input == Vector2.zero) ? Vector2.zero : Acceleration(new Vector2(sidewalk, frontwalk),A);
        rb.velocity = cal_velocity(acceleration, rb.velocity,max_speed);
    }
    private Vector2 Acceleration(Vector2 input, float A)
    {
        float angle = Mathf.Atan2(input.y, input.x);
        Vector2 temp = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        float cam_rotation = -Camera.main.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        temp = new Vector2(temp.x * Mathf.Cos(cam_rotation) - temp.y * Mathf.Sin(cam_rotation),
                          temp.x * Mathf.Sin(cam_rotation) + temp.y * Mathf.Cos(cam_rotation));
        if (input.magnitude >= 0.9)
            transform.rotation = Quaternion.Euler(new Vector3(0, -cam_rotation * Mathf.Rad2Deg, 0));
        return A * temp;
    }

    private Vector3 cal_velocity(Vector2 A, Vector3 cur_v ,float max_speed)
    {
        Vector2 v2_cur_v = new Vector2(cur_v.x, cur_v.z);
        Vector2 drag = v2_cur_v.normalized * global_DA;
        Vector2 new_v = v2_cur_v + (A - drag) * Time.deltaTime;
        new_v = (max_speed < new_v.magnitude) ? new_v * (max_speed / new_v.magnitude) : new_v;// how to scale A such that cur_v + A .mag == max;
        return new Vector3(new_v.x, cur_v.y, new_v.y);
    }

    public void reset_speed()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }



    /*    private enum State
        {
            Run,
            Idle,
            Sprint,
            Run_attack,
            Attack_follow_1,
            Attack_follow_2,
            Dash,
            Can_attack_follow,
            Can_dash_follow
        }

        private string[] stateKey = 
        { 
            "Run",
            "Idle",
            "Sprint" ,
            "Run Attack" ,
            "Attack follow 1" ,
            "Attack follow 2" ,
            "Dash" ,
            "Can attack follow" ,
            "Can dash follow"
        };*/
    // Start is called before the first frame update


}
