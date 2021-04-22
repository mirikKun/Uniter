using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class FlyingGun : MonoBehaviour
{
    public GameObject gun;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
             other.GetComponent<AddGun>().AddingGun(gun);
             Destroy(gameObject);
        }
    }

}
