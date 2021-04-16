﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GrapGun : MonoBehaviour
{
    private LineRenderer lr;


    public LayerMask groundLayer;

    public Transform hookTip, fpCamera, player;

    private SpringJoint joint;
    private Vector3 grapPoint;
    
    private Rigidbody rb;
    private CharacterController _controller;
    private FpMovement fpm;
    private FpMovementRigid fpmr;
    
    
    public float maxDist = 100f;
    public float spring = 10f;
    public float damper = 10f;
    public float massScale = 10f;

    // Start is called before the first frame update
    void Awake()
    {
        rb = player.GetComponent<Rigidbody>();
        fpm = player.GetComponent<FpMovement>();
        fpmr = player.GetComponent<FpMovementRigid>();
        _controller = player.GetComponent<CharacterController>();
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update() 
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartGrap();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            StopGrap();
        }
    }

    void LateUpdate()
    {
        DrawRope();
    }

    void StartGrap()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpCamera.position, fpCamera.forward, out hit,maxDist,groundLayer))
        {
            fpm.enabled = false;
            _controller.enabled = false;
            rb.isKinematic = false;
            fpmr.enabled = true;
            rb.velocity = _controller.velocity;
            
            grapPoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapPoint;
            
            float distance = Vector3.Distance(player.position, grapPoint);
            
            joint.maxDistance = distance * 0.8f;
            joint.minDistance = distance * 0.25f;

            joint.spring = spring;
            joint.damper = damper;
            joint.massScale = massScale;

            lr.positionCount = 2;
        }
    }

    void DrawRope()
    {
        if (!joint)
            return;
        lr.SetPosition(0, hookTip.position);
        lr.SetPosition(1, grapPoint);
    }

    void StopGrap()
    {
        if (!joint)
            return;
        lr.positionCount = 0;
        Destroy(joint);
        Vector3 vel = rb.velocity;
        rb.isKinematic = true;
        fpmr.enabled = false;
        _controller.enabled = true;
        fpm.enabled = true;
        fpm.gravityVelocity=new Vector3(Mathf.Abs(Physics.gravity.x)*vel.x,Mathf.Abs(Physics.gravity.y)*vel.y,Mathf.Abs(Physics.gravity.z)*vel.z)*1.1f/Vector3.Magnitude(Physics.gravity);
       
    }
}