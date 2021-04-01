using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public float gravity = 20f;
    public LayerMask groundMasc;
    public float groundDistance = 0.5f;
    public float speed = 12f;
    public float jumpHeight = 10;


    Vector3 gravityVelocity;
    Vector3 gravityDirection = new Vector3(0, -1, 0);
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Gravity();
        Jump();
 
    }
    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }
    bool checkGravity(Vector3 velocity, Vector3 dir)
    {
        if (velocity.x * dir.x + velocity.y * dir.y + velocity.z * dir.z > 0)
            return true;
        return false;

    }
    void Gravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMasc);
        if (isGrounded && checkGravity(gravityVelocity, gravityDirection))
        {
            gravityVelocity = gravityDirection * 3;
        }
        gravityVelocity += gravityDirection * gravity * Time.deltaTime;
        controller.Move(gravityVelocity * Time.deltaTime);

    }
    void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            gravityVelocity = -gravityDirection * Mathf.Sqrt(jumpHeight * 2 * gravity);
        }

    }
    public void SwitchGravity(Vector3 direct)
    {   
        if (direct.sqrMagnitude == 1)
        {
            gravityDirection = -direct;
            if (direct.x == 1)
                direct = new Vector3(transform.eulerAngles.x, 0, -90);
            else if(direct.x==-1)
                direct = new Vector3(transform.eulerAngles.x, 0, 90);
            else if (direct.y == 1)
                direct = new Vector3(0, transform.eulerAngles.y, 0);
            else if (direct.y == -1)
                direct = new Vector3(0, transform.eulerAngles.y, 180);
            else if (direct.z == 1)
                direct = new Vector3(transform.eulerAngles.x, 90, 90);
            else if (direct.z == -1)
                direct = new Vector3(transform.eulerAngles.x, 90, -90);
            transform.eulerAngles = (direct);
        }
    }

    public void Blink(Vector3 pos)
    {
        controller.enabled = false;
        transform.position = pos;
        controller.enabled = true;
    }

    
           
}
