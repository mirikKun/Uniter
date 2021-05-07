using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    public int outerRadius = 5;
    public int enemyCount = 20;
    public int gravityGunCount = 10;
    private bool[,,] _busyPoints;
    private int[] _size;

    public GameObject enemy;
    public GameObject gravityGun;
    public GameObject cube;

    void Awake()
    {
        if (!FillOptions.defaultRoom)
        {
            enemyCount = FillOptions.enemyCount;
        }
    }

    public void SetRoom(bool[,,] busy, int[] size)
    {
        _busyPoints = busy;
        _size = size;
        if(FillOptions.join)
            return;
        EnemySpawner();
        for (int i = 0; i < gravityGunCount; i++)
        {
            GravityGunSpawn();
        }
    }

    public Vector3 GetPlayerPosition()
    {
        Vector3Int newPlace = FindPlaceToSpawn();
        if (!_busyPoints[newPlace.x, newPlace.y - 1, newPlace.z])
        {
            PhotonNetwork.Instantiate(cube.name, new Vector3(newPlace.x * _size[3] + transform.position.x,
                (newPlace.y - 1) * _size[3] + transform.position.y,
                newPlace.z * _size[3] + transform.position.z), Quaternion.identity);
        }

        _busyPoints[newPlace.x, newPlace.y - 1, newPlace.z] = true;
        return new Vector3(newPlace.x * _size[3] + transform.position.x,
            newPlace.y * _size[3] + transform.position.y,
            newPlace.z * _size[3] + transform.position.z);
    }

    private Vector3Int FindPlaceToSpawn()
    {
        int x = Random.Range(_size[0] / outerRadius, _size[0] - _size[0] / outerRadius);
        int y = Random.Range(_size[1] / outerRadius, _size[1] - _size[1] / outerRadius);
        int z = Random.Range(_size[2] / outerRadius, _size[2] - _size[2] / outerRadius);
        Vector3Int dir = NewDirection();

        Debug.Log(_size[0]+" "+_size[1]+" "+_size[2]+" coord  "+x+" "+y+" "+z);
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
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3Int newPlace = FindPlaceToSpawn();
            GameObject newEnemy = PhotonNetwork.Instantiate(enemy.name, new Vector3(newPlace.x * _size[3] + transform.position.x,
                newPlace.y * _size[3] + transform.position.y,
                newPlace.z * _size[3] + transform.position.z), Quaternion.identity);
            newEnemy.GetComponent<EnemyMover>().BeginMovement(_busyPoints, _size, newPlace);
        }
    }

    public void GravityGunSpawn()
    {
        Vector3Int newPlace = FindPlaceToSpawn();
        PhotonNetwork.Instantiate(gravityGun.name, new Vector3(newPlace.x * _size[3] + transform.position.x,
            newPlace.y * _size[3] + transform.position.y,
            newPlace.z * _size[3] + transform.position.z), Quaternion.identity);
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