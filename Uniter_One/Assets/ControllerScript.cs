using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public float gravity=-20f;
    public LayerMask groundMasc;
    public float groundDistance = 0.5f;
    public float speed = 12f;
    public float jumpHeight = 10;

    Vector3 gravityVelocity;
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
    void Gravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance,groundMasc);
        if(isGrounded && gravityVelocity.y<0)
        {
            gravityVelocity.y = -3;
        }
        controller.Move(gravityVelocity * Time.deltaTime);
       
    }
    void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            gravityVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        gravityVelocity.y += gravity * Time.deltaTime;
    }
}
