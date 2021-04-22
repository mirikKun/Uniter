﻿using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Spawner : MonoBehaviour
{
    public int count = 20;
    private bool[,,] _busyPoints;
    private int[] _size;

    public GameObject enemy;
    public GameObject gravityGun;
    public GameObject cube;

    public void SetRoom(bool[,,] busy, int[] size)
    {
        _busyPoints = busy;
        _size = size;
        EnemySpawner();
        GravityGunSpawn();
    }

    public Vector3 GetPlayerPosition()
    {
       Vector3Int newPlace = FindPlaceToSpawn();
        if ( !_busyPoints[newPlace.x, newPlace.y - 1, newPlace.z])
        {
            Instantiate(cube, new Vector3(newPlace.x * _size[3] + transform.position.x,
                (newPlace.y-1) * _size[3] + transform.position.y,
                newPlace.z * _size[3] + transform.position.z), Quaternion.identity, transform);
            
        }
        _busyPoints[newPlace.x, newPlace.y-1, newPlace.z] = true;
        return new Vector3(newPlace.x * _size[3] + transform.position.x,
            newPlace.y * _size[3] + transform.position.y,
            newPlace.z * _size[3] + transform.position.z);
    }

    private Vector3Int FindPlaceToSpawn()
    {
        int x = Random.Range(_size[0] / 10, _size[0] - _size[0] / 10);
        int y = Random.Range(_size[1] / 10, _size[1] - _size[1] / 10);
        int z = Random.Range(_size[2] / 10, _size[2] - _size[2] / 10);
        Vector3Int dir = NewDirection();

        while (_busyPoints[x, y, z])
        {
            x += dir.x;
            y += dir.y;
            z += dir.z;
        }

        _busyPoints[x, y, z] = true;
        return new Vector3Int(x, y, z);
    }

    private void EnemySpawner()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3Int newPlace = FindPlaceToSpawn();
            GameObject newEnemy = Instantiate(enemy, new Vector3(newPlace.x * _size[3] + transform.position.x,
                newPlace.y * _size[3] + transform.position.y,
                newPlace.z * _size[3] + transform.position.z), Quaternion.identity, transform);
            newEnemy.GetComponent<EnemyMover>().BeginMovement(_busyPoints, _size, newPlace);
        }
    }

    public void GravityGunSpawn()
    {
        Vector3Int newPlace = FindPlaceToSpawn();
        Instantiate(gravityGun, new Vector3(newPlace.x * _size[3] + transform.position.x,
            newPlace.y * _size[3] + transform.position.y,
            newPlace.z * _size[3] + transform.position.z), Quaternion.identity, transform);
    }


    Vector3Int NewDirection()
    {
        int newDir = Random.Range(0, 6);
        switch (newDir)
        {
            case 0:
                return new Vector3Int(1, 0, 0);
            case 1:
                return new Vector3Int(-1, 0, 0);
            case 2:
                return new Vector3Int(0, 1, 0);
            case 3:
                return new Vector3Int(0, -1, 0);
            case 4:
                return new Vector3Int(0, 0, 1);
            default:
                return new Vector3Int(0, 0, -1);
        }
    }
}