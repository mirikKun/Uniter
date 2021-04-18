using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform player;
    public float distanceToFind = 30;
    public float bulletSpeed = 30;
    public float smoothing = 0.3f;
    private Vector3 movingDir;
    private bool[,,] freePoints;
    public GameObject projectile;
    public Transform barrel;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public float fireRate = 3f;
    private float _fireTime = 0f;
    private int[] pos = new int[3];

    private float distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer < distanceToFind)
        {
            if (!Physics.Raycast(transform.position, player.position - transform.position, distanceToPlayer,
                groundLayer))
            {
                Vector3 relativePos = player.position - transform.position;
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
            Vector3 relativePos = movingDir;
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
        movingDir = dir;
    }
    

    Vector3 newVector3()
    {
        return new Vector3();
    }

    IEnumerator Movement(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05)
        {
            transform.position = Vector3.Lerp(transform.position, target, smoothing * Time.deltaTime);
            yield return null;
        }
    }
}