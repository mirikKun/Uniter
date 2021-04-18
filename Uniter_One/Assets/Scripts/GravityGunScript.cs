using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GravityGunScript : MonoBehaviour
{ 
   
    public Camera fpCam;
    public ParticleSystem fireLight;
    public GameObject shootLight;
    public Transform player; 
    public Transform fpCamera;
   
    public Vector3 currentGravity = new Vector3(0, -1, 0);
    public float gravity = 30f;
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = currentGravity * gravity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ChangeGravity();
        }
    }

    void ChangeGravity()
    {
        fireLight.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpCam.transform.position, fpCam.transform.forward, out hit))
        {
            player.GetComponent<FpMovement>().SwitchGravity(hit.normal,hit.point);
            fpCamera.GetComponent<FPcamera>().GravitySwitch(hit.point);
            Physics.gravity = -hit.normal * gravity;
            GameObject shoot = Instantiate(shootLight, hit.point, Quaternion.LookRotation(hit.normal));
            StartCoroutine(fpCam.GetComponent<Shake>().CameraShake(0.45f, 0.45f));
            Destroy(shoot, 2);
        }
    }


}