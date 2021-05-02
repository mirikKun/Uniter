using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;

public class GravitySwitchGun : Gun
{
    public Transform player;
    public GameObject boom;
    public Spawner spawner;
   
    public Vector3 currentGravity = new Vector3(0, -1, 0);
    public LayerMask layerMask;
    public float gravity = 30f;
    private bool shooted;
    

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
    

    public IEnumerator Disappear(float time)
    {
        Transform trans = transform;
        yield return new WaitForSeconds(time) ;
        if(!trans) yield break;

        Destroy(Instantiate(boom, trans.position, Quaternion.identity, player),3);
        spawner.GravityGunSpawn();
        Destroy(gameObject);
        
    }

    public override void Shoot()
    {
        if(shooted) return;
        RaycastHit hit;
        if (Physics.Raycast(fpCamera.position, fpCamera.forward, out hit,layerMask))
        {
            shooted = true;
            Physics.gravity = -hit.normal * gravity;
            // foreach (var player in PhotonNetwork.PlayerList)
            // {
            //     player.
            // }
            player.GetComponent<FpController>().SwitchGravity(hit.normal);
            
            StartCoroutine(Disappear(0));
        }
    }
}