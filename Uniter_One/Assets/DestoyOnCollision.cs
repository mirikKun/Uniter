using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
 Destroy(gameObject);
        }

       
    }
}