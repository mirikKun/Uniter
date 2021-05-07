using System;
using UnityEngine;
public class BulletHit : MonoBehaviour
{
    public GameObject mussleFlash;
    public GameObject hitFlash;
    
    public float damage;
    public String targetTag;
    private void Start()
    {
        GameObject flash = Instantiate(mussleFlash, transform.position, transform.localRotation);
        Destroy(flash, 1);
    }

    private void OnCollisionEnter(Collision other)
    {
        ContactPoint contact = other.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        if (other.gameObject.CompareTag(targetTag))
        {
            other.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage);
        }
        GameObject hit = Instantiate(hitFlash, pos, rot);
        Destroy(hit, 3);
        Destroy(gameObject);
    }
}