using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private bool[,,] freePoints;
    public int count = 20;

    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Set(bool[,,] busy, int xSize, int ySize, int zSize, int scale)
    {
        for (int i = 0; i < count; i++)
        {
            int x = Random.Range(xSize / 10, xSize - xSize / 10);
            int y = Random.Range(ySize / 10, ySize - ySize / 10);
            int z = Random.Range(zSize / 10, zSize - zSize / 10);
            Vector3 dir = NewDirection();
            while (busy[x, y, z])
            {
                x += (int) dir.x;
                y += (int) dir.y;
                z += (int) dir.z;
            }
            Instantiate(enemy, new Vector3(x * scale + transform.position.x,
                y * scale + transform.position.y,
                z * scale + transform.position.z), Quaternion.identity, transform);
            busy[x, y, z] = true;
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
}