using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGunScript : MonoBehaviour
{
    public float range = 100f;
    public Camera fpCam;
    public ParticleSystem fireLight;
    public GameObject shootLight;
    public Transform player;
    public float blinkNear=4;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
  
        if (Input.GetButtonDown("Fire1"))
        {
            ChangeGravity();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Blink();
        }

    }
    void ChangeGravity()
    {
        fireLight.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpCam.transform.position, fpCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            player.GetComponent<ControllerScript>().SwitchGravity(hit.normal);
            GameObject shoot=   Instantiate(shootLight, hit.point, Quaternion.LookRotation(hit.normal));
            
            Destroy(shoot, 2);
        }
    }
    void Blink()
    {

        fireLight.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpCam.transform.position, fpCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            player.GetComponent<ControllerScript>().Blink(hit.point- fpCam.transform.forward*blinkNear);
            GameObject shoot = Instantiate(shootLight, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(shoot, 2);
        }
    }
}
