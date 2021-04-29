using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public override void Shoot()
    {
        if(!(Time.time >= _fireTime)) return;
        _fireTime = Time.time + 1f / fireRate;
        GameObject bullet = PhotonNetwork.Instantiate(projectile.name, startPoint.position, Quaternion.LookRotation(fpCamera.forward));
        bullet.GetComponent<Rigidbody>().velocity = fpCamera.forward * bulletSpeed;
        Destroy(bullet, 5);
        if (!sc) return;
        sc.OuterStartCoroutineCooldown(0,1f / fireRate);
    }
    
}