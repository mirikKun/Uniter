using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun;
public class DamageGun : Gun
{
    public float damage = 10f;
    public float bulletSpeed = 40;
    
    public Transform startPoint;
    public GameObject projectile;
    public float fireRate = 0.5f;
    public float _fireTime = 0f;
    public SkillController sc;
    private PhotonView PV;
    void Awake()
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
    void ProjectileCreate(Vector3 pos,Vector3 rot)
    {
        GameObject bullet =Instantiate(projectile, pos, Quaternion.LookRotation(rot));
        bullet.GetComponent<Rigidbody>().velocity = rot * bulletSpeed;
        Destroy(bullet, 5);
    }
    
}