﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private bool[,,] freePoints;
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

    public void Set(bool[,,] busy)
    {
        freePoints = busy;
    }
}
