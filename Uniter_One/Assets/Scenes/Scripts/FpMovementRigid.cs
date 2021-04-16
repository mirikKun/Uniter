using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpMovementRigid : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 12f;
    private Vector3 move;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetInput();
    }
    private void FixedUpdate()
    {
        if(Vector3.Magnitude(rb.velocity)<30)
         rb.AddForce(speed*move,ForceMode.Impulse);
    }
    void GetInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        move = transform.right * x + transform.forward * z;
    }
}