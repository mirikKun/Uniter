using System;
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

    public float maxDist = 100f;
    public float spring = 10f;
    public float damper = 10f;
    public float massScale = 10f;

    // Start is called before the first frame update
    void Awake()
    {
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
            player.GetComponent<FpMovementRigid>().grapling = true;
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
        player.GetComponent<FpMovementRigid>().grapling = false;
        if (!joint)
            return;
        lr.positionCount = 0;
        Destroy(joint);
    }
}