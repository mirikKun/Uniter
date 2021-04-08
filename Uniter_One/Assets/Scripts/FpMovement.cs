﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpMovement : MonoBehaviour
{
     public CharacterController controller;
    public Transform groundCheck;
    public float gravity = 20f;
    public LayerMask groundMasc;
    public float groundDistance = 0.5f;
    public float speed = 12f;
    public float jumpHeight = 10;

    
    private Vector3 gravityVelocity;
    private Vector3 gravityDirection = new Vector3(0, -1, 0);
    private bool isGrounded;

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

    public void Place(bool[,,] points,Transform generatorPoint,int scale,GameObject cube)
    {
        Debug.Log(transform.position);
        Debug.Log(generatorPoint.position);
        int xPos = (int)(transform.position.x-generatorPoint.position.x+(float)scale/2)/scale;
        int yPos = (int)(transform.position.y-generatorPoint.position.y+(float)scale/2)/scale;
        int zPos = (int)(transform.position.z-generatorPoint.position.z+(float)scale/2)/scale;
        Debug.Log(xPos+" "+yPos+ ' '+zPos);
        int shift = 0;
        while (!points[xPos, yPos, zPos])
        {
            shift++;
            yPos++;
        }
        Blink(transform.position + new Vector3(0, shift*scale, 0));
        if (yPos == 0 || points[xPos, yPos - 1, zPos])
            Instantiate(cube, new Vector3(
                xPos*scale+generatorPoint.position.x, 
                (yPos - 1)*scale+generatorPoint.position.y,
                zPos*scale+generatorPoint.position.z)
                ,Quaternion.identity,generatorPoint);
            
        Debug.Log(shift);
        

    }
    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        var transform1 = transform;
        Vector3 move = transform1.right * x + transform1.forward * z;
        controller.Move(speed  *  Time.deltaTime*move);
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
        gravityVelocity +=   gravity * Time.deltaTime*gravityDirection;
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