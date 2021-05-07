using System;
using System.Collections;
using UnityEngine;


public class GravitySwitchGun : Gun
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject boom;
    [SerializeField] private Spawner spawner;
   
    [SerializeField] private Vector3 currentGravity = new Vector3(0, -1, 0);
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float gravity = 30f;
    private bool _shot;
    

    public void SetPlayer(Transform newPlayer, Transform newCamera)
    {
        player = newPlayer;
        fpCamera = newCamera;
    }
    private void Start()
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
        if(_shot) return;
        RaycastHit hit;
        if (Physics.Raycast(fpCamera.position, fpCamera.forward, out hit,200,layerMask))
        {
            if (CheckVector(hit.normal))
                return;
            _shot = true;
            Physics.gravity = -hit.normal * gravity;
            player.GetComponent<FpController>().SwitchGravity(hit.normal);
            StartCoroutine(Disappear(0));
        }
    }

    private bool CheckVector(Vector3 vector)
    {
        if (Math.Abs(vector.x) != 1f)
            return false;
        if (Math.Abs(vector.y) != 1f)
            return false;
        if (Math.Abs(vector.z) != 1f)
            return false;
        return true;
    }
}