using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GravitySwitchGun : MonoBehaviour
{
    public Transform player; 
    public Transform fpCamera;
    public ParticleSystem boom;
    public Spawner spawner;
   
    public Vector3 currentGravity = new Vector3(0, -1, 0);
    public float gravity = 30f;

    public void SetPlayer(Transform newPlayer, Transform newCamera)
    {
        player = newPlayer;
        fpCamera = newCamera;
    }
    void Start()
    {
        spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        Physics.gravity = currentGravity * gravity;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")&&player)
        {

            ChangeGravity();
        }
    }
    

    public IEnumerator Disappear(float time)
    {
        Transform trans = transform;
        yield return new WaitForSeconds(time) ;
        if(!trans) yield break;

        Destroy(Instantiate(boom, trans.position, Quaternion.identity, player),3);
        spawner.GravityGunSpawn();
        Destroy(trans.parent.gameObject);
        
    }

    void ChangeGravity()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(fpCamera.position, fpCamera.forward, out hit))
        {
            player.GetComponent<FpMovement>().SwitchGravity(hit.normal);
            fpCamera.GetComponent<FPcamera>().GravitySwitch(hit.point);
            
            StartCoroutine(fpCamera.GetComponent<ShakeCamera>().CameraShake(0.45f, 0.45f));
        }
        Physics.gravity = -hit.normal * gravity;
        StartCoroutine(Disappear(0));
    }
}