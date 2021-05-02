using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Photon.Pun;
public class GrapGun : Gun
{
    private LineRenderer lr;


    public LayerMask groundLayer;
    public GameObject particle;
    public Transform hookTip, player;
    private SkillController sc;

    private SpringJoint joint;
    private Vector3 grapPoint;

    private FpController fpm;

    public float maxDist = 100f;
    public float spring = 5f;
    public float damper = 15f;
    public float massScale = 5f;
    public float cooldown = 1.5f;
    private float _fireTime = 0f;
    private PhotonView PV;

    // Start is called before the first frame update
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        fpm = player.GetComponent<FpController>();
        lr = GetComponent<LineRenderer>();
        sc = player.GetComponent<SkillController>();
           
    }
    void LateUpdate()
    {
        DrawRope();
    }

    public override void Shoot()
    {
        if (!(Time.time >= _fireTime)) return;
        RaycastHit hit;
        if (Physics.Raycast(fpCamera.position, fpCamera.forward, out hit, maxDist, groundLayer))
        {
            fpm.GrapModeOn();

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
            PV.RPC("ParticleOn",RpcTarget.All,true);
        }
        
    }

    void DrawRope()
    {
        if (!joint)
            return;
        lr.SetPosition(0, hookTip.position);
        lr.SetPosition(1, grapPoint);
    }

    [PunRPC]
    void ParticleOn(bool enable)
    {
        particle.SetActive(enable);
    }
    public void StopGrap()
    {
        if (!joint)
            return;
        _fireTime = Time.time + cooldown;
        StartCoroutine(sc.StartCooldown(1, cooldown));
        lr.positionCount = 0;
        Destroy(joint);
        fpm.GrapModeOff();
        PV.RPC("ParticleOn",RpcTarget.All,false);
    }
}