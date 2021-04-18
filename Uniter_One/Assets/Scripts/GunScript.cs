using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float bulletSpeed = 40;
    
    public Camera fpCam;
    public Transform startPoint;
    public GameObject projectile;
    public float fireRate = 0.5f;
    private float _fireTime = 0f;
    public bool automatic;
    public SkillController sc;
    
    void Update()
    {
        if ((!automatic&&!Input.GetButtonDown("Fire1")) ||(automatic&&!Input.GetButton("Fire1")) || (!(Time.time >= _fireTime))) return;
        _fireTime = Time.time + 1f / fireRate;
        Shoot();
        if (sc == null) return;
        _fireTime = Time.time + 1f / fireRate;
        sc.OuterStartCoroutine(0,1f / fireRate);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(projectile, startPoint.position, Quaternion.LookRotation(fpCam.transform.forward));
        bullet.GetComponent<Rigidbody>().velocity = fpCam.transform.forward * bulletSpeed;
        Destroy(bullet, 5);
    }
    
}