using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public GameObject mussleFlash;
    public GameObject hitFlash;
    
    private void Start()
    {
        GameObject flash = Instantiate(mussleFlash, transform.position, transform.localRotation);
        Destroy(flash,1);
    }

    private void OnCollisionEnter(Collision other)
    {
        

        ContactPoint contact = other.contacts[0];
        Quaternion rot=Quaternion.FromToRotation(Vector3.up,contact.normal);
        Vector3 pos = contact.point;

        GameObject hit = Instantiate(hitFlash, pos, rot);
        
        Destroy( hit,3);
if (!other.gameObject.CompareTag("Enemy")&&!other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}