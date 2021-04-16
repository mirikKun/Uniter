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

    void Start()
    {
    }


    void Update()
    {
        if (!Input.GetButtonDown("Fire1") || (!(Time.time >= _fireTime))) return;
        _fireTime = Time.time + 1f / fireRate;
        Shoot();
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpCam.transform.position, fpCam.transform.forward, out hit, range))
        {
                GameObject bullet = Instantiate(projectile, startPoint.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().velocity = fpCam.transform.forward * bulletSpeed;
                Destroy(bullet, 5);
            
        }
    }
}