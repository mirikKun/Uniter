using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class EnemySpawner : MonoBehaviour
{
    private bool[,,] freePoints;
    public int count = 20;
    private int[] size;
    private int scaler;
    public float speed = 10;

    public GameObject enemy;

    private GameObject enemys;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Set(bool[,,] busy, int xSize, int ySize, int zSize, int scale)
    {
        freePoints = busy;
        scaler = scale;
        size = new int[3] {xSize, ySize, zSize};
        for (int i = 0; i < count; i++)
        {
            int x = Random.Range(xSize / 10, xSize - xSize / 10);
            int y = Random.Range(ySize / 10, ySize - ySize / 10);
            int z = Random.Range(zSize / 10, zSize - zSize / 10);
            int[] dir = NewDirection();
            while (freePoints[x, y, z])
            {
                x += dir[0];
                y += dir[1];
                z += dir[2];
            }
            enemys = Instantiate(enemy, new Vector3(x * scale + transform.position.x,
                y * scale + transform.position.y,
                z * scale + transform.position.z), Quaternion.identity, transform);
            freePoints[x, y, z] = true;
            StartCoroutine(MoveCoroutine(enemys, new int[3] {x, y, z}));
        }
    }

    int[] NewDirection()
    {
        int newDir = Random.Range(0, 6);
        switch (newDir)
        {
            case 0:
                return new int[]{1, 0, 0};
            case 1:
                return new int[]{-1, 0, 0};
            case 2:
                return new int[]{0,1, 0};
            case 3:
                return new int[]{0, -1, 0};
            case 4:
                return new int[]{0, 0, 1};
            default:
                return new int[]{0, 0, -1};
        }
    }

    IEnumerator MoveCoroutine(GameObject enemyObject, int[] coord)
    {
        while (true)
        {
            float pause = Random.Range(0f,1f);
            yield return new WaitForSeconds(pause);
            int lenght = 0;
            int[] newDir = NewDirection();
            
            lenght = FindLenght(coord, newDir);
            int randLenght = Random.Range(0, lenght + 1);
            for (int i = 1; i < randLenght+1; i++)
            {
                freePoints[coord[0]+newDir[0]*i, coord[1]+newDir[1]*i, coord[2]+newDir[2]*i]=true;
            }
            
            enemyObject.GetComponent<EnemyMovement>().SetRot(new Vector3(newDir[0],newDir[1],newDir[2]));
            Vector3 newPos = enemyObject.transform.position + new Vector3(newDir[0],newDir[1],newDir[2]) * (randLenght * scaler);
            
            while (Vector3.Distance(enemyObject.transform.position,newPos)>0.05f)
            {
                enemyObject.transform.position=Vector3.MoveTowards(enemyObject.transform.position,newPos,speed*Time.deltaTime);
                yield return null;
            }
            for (int i = 0; i < randLenght; i++)
            {
                freePoints[coord[0]+newDir[0]*i, coord[1]+newDir[1]*i, coord[2]+newDir[2]*i]=false;
            }
            coord[0] += newDir[0] * randLenght;
            coord[1] += newDir[1] * randLenght;
            coord[2] += newDir[2] * randLenght;
            pause = Random.Range(0f,1f);
            yield return new WaitForSeconds(pause);
        }
    }

    int FindLenght(int[] coord, int[] newDir)
    {
        
        int xDir = newDir[0];
        int yDir = newDir[1];
        int zDir = newDir[2];
        int lenght = 1;
        if (xDir == 1 || xDir == -1)
        {
            while (!freePoints[coord[0] + xDir * lenght, coord[1], coord[2]] &&
                   coord[0] + xDir * lenght > 0 &&
                   coord[0] + xDir * lenght < size[0] - 1)
            {
                lenght++;
            }
            return lenght -1;
        }

        if (yDir == 1 || yDir == -1)
        {

            while (!freePoints[coord[0], coord[1] + yDir * lenght, coord[2]] &&
                   coord[1] + yDir * lenght> 0 &&
                   coord[1] + yDir * lenght < size[1])
            {
                lenght++;
            }
            return lenght -1;
        }
        while (!freePoints[coord[0] , coord[1], coord[2]+ zDir * lenght] &&
               coord[2] + zDir * lenght > 0 &&
               coord[2] + zDir * lenght < size[2])
        {
            lenght++;
        }
        return lenght -1;

    }
}