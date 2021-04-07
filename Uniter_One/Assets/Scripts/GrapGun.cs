using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapGun : MonoBehaviour
{
    private LineRenderer lr;

    private Vector3 grapPoint;

    public LayerMask groundLayer;

    public Transform hookTip, fpCamera, player;

    private SpringJoint joint;

    public float maxDist = 100f;
    // Start is called before the first frame update
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            StartGrap();
        }
        else if(Input.GetButtonUp("Fire1"))
        {
            StopGrap();
        }
    }

    void StartGrap()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpCamera.position, fpCamera.forward, out hit, maxDist, groundLayer)) ;
        {}

    }

    void StopGrap()
    {
        
    }
}
