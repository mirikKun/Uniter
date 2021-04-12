using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0.5f;
    public Camera fpCam;
    public ParticleSystem fireLight;
    public GameObject shootLight;
    private float _fireTime = 0f;

    void Start()
    {
    }


    void Update()
    {
        if (!Input.GetButton("Fire1") || (!(Time.time >= _fireTime))) return;
        _fireTime = Time.time + 1f / fireRate;
        Shoot();
    }

    void Shoot()
    {
        fireLight.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpCam.transform.position, fpCam.transform.forward, out hit, range))
        {   
            StartCoroutine(fpCam.GetComponent<Shake>().CameraShake(0.15f, 0.45f));
            GameObject shoot = Instantiate(shootLight, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(shoot, 2);
        }
    }
}