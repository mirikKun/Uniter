using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float smoothing = 7f;
    private bool[,,] freePoints;

    private int[] pos = new int[3];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (freePoints != null)
        {
            
        }
    }



    Vector3 NewDirection()
    {
        int newDir = Random.Range(0, 5);
        switch (newDir)
        {
            case 0:
                return new Vector3(1, 0, 0);
            case 1:
                return new Vector3(-1, 0, 0);
            case 2:
                return new Vector3(0, 1, 0);
            case 3:
                return new Vector3(0, -1, 0);
            case 4:
                return new Vector3(0, 0, 1);
            default:
                return new Vector3(0, 0, -1);
        }
    }
    Vector3 newVector3()
    {
        return new Vector3();
    }
    IEnumerator Movement(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target)>0.05)
        {
            transform.position = Vector3.Lerp(transform.position, target,smoothing* Time.deltaTime);
            yield return null;
        }
    }
}
