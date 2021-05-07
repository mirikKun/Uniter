using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun;
public class DamageGun : Gun
{
    [SerializeField] private SkillController sc;
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float bulletSpeed = 40;
    [SerializeField] private float fireRate = 0.5f;
    
    private float _fireTime = 0f;
    private PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();

    }
    public override void Shoot()
    {
        if(!(Time.time >= _fireTime)) return;
        _fireTime = Time.time + 1f / fireRate;
        PV.RPC("ProjectileCreate",RpcTarget.All,startPoint.position,fpCamera.forward);
        if (!sc) return;
        sc.OuterStartCoroutineCooldown(0,1f / fireRate);
    }
    [PunRPC]
    private void ProjectileCreate(Vector3 pos,Vector3 rot)
    {
        GameObject bullet =Instantiate(projectile, pos, Quaternion.LookRotation(rot));
        bullet.GetComponent<Rigidbody>().velocity = rot * bulletSpeed;
        Destroy(bullet, 5);
    }
    
}