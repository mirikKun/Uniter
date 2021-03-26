using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float damage = 10;
    public float range = 100;
    public Camera fpCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
            {
            Shoot();
        }
    }
    void  Shoot()
    {
        RaycastHit hit;
       if( Physics.Raycast(fpCam.transform.position, fpCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
