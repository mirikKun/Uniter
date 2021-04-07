using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGunScript : MonoBehaviour
{
    public float rangeBlink = 30;
    public Camera fpCam;
    public ParticleSystem fireLight;
    public GameObject shootLight;
    public Transform player;
    public Transform FpCamera;
    public float blinkNear=4;
    public Vector3 currentGravity = new Vector3(0, -1, 0);
    
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
        if (Physics.Raycast(fpCam.transform.position, fpCam.transform.forward, out hit))
        {
            player.GetComponent<ControllerScript>().SwitchGravity(hit.normal);
            FpCamera.GetComponent<FPcamera>().CameraSwitch();
            GameObject shoot=   Instantiate(shootLight, hit.point, Quaternion.LookRotation(hit.normal));
            
            Destroy(shoot, 2);
        }
    }
    void Blink()
    {

        fireLight.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpCam.transform.position, fpCam.transform.forward, out hit, rangeBlink))
        {
            player.GetComponent<ControllerScript>().Blink(hit.point- fpCam.transform.forward*blinkNear);
            GameObject shoot = Instantiate(shootLight, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(shoot, 2);
        }
        else
        {
            player.GetComponent<ControllerScript>().Blink(fpCam.transform.position+rangeBlink*fpCam.transform.forward);
        }
    }
}
