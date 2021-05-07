using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform barrel;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
        
    [SerializeField] private float distanceToFind = 30;
    [SerializeField] private float bulletSpeed = 30;
    [SerializeField] private float smoothing = 0.3f;
    [SerializeField] private float fireRate = 3f;
    
    private float _fireTime = 0f;
    private Transform _player;
    private float _distanceToPlayer;
    private Vector3 _movingDir;
    private bool[,,] _freePoints;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (_player)
            return;
        if (other.CompareTag("Player"))
            _player = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_player || other.transform != _player)
            return;
        _player = other.transform;
    }


    // Update is called once per frame
    void Update()
    {
        if (!_player) return;

        _distanceToPlayer = Vector3.Distance(_player.position, transform.position);
        if (_distanceToPlayer < distanceToFind)
        {
            if (!Physics.Raycast(transform.position, _player.position - transform.position, _distanceToPlayer,
                groundLayer))
            {
                Vector3 relativePos = _player.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                Quaternion nextRotation =
                    Quaternion.Lerp(transform.localRotation, rotation, smoothing * Time.deltaTime);
                transform.localRotation = nextRotation;
                if (Physics.Raycast(transform.position, transform.forward, distanceToFind, playerLayer))
                {
                    if (!(Time.time >= _fireTime)) return;
                    _fireTime = Time.time + 1f / fireRate;
                    Shoot();
                }
            }
        }
        else
        {
            Vector3 relativePos = _movingDir;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            Quaternion nextRotation = Quaternion.Lerp(transform.localRotation, rotation, smoothing * Time.deltaTime);
            transform.localRotation = nextRotation;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(projectile, barrel.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        Destroy(bullet, 5);
    }

    public void SetRot(Vector3 dir)
    {
        _movingDir = dir;
    }
}