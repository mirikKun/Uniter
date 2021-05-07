using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    
    [SerializeField] private float speed = 10;
    [SerializeField] private EnemyShooting enemyShooting;
    
    private bool[,,] _busyPoints;
    private int[] _size;
    

    public void BeginMovement(bool[,,] freePoints, int[] size, Vector3Int coord)
    {
        _busyPoints = freePoints;
        _size = size;
        StartCoroutine(MoveCoroutine(coord));
    }

    private Vector3Int NewDirection()
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

    private IEnumerator MoveCoroutine(Vector3Int coord)
    {
        while (true)
        {
            float pause = Random.Range(0f, 1f);
            yield return new WaitForSeconds(pause);
            int lenght = 0;
            Vector3Int newDir = NewDirection();

            lenght = FindLenght(coord, newDir);
            int randLenght = Random.Range(0, lenght + 1);
            for (int i = 1; i < randLenght + 1; i++)
            {
                _busyPoints[coord.x + newDir.x * i, coord.y + newDir.y * i, coord.z
                 + newDir.z * i] = true;
            }

            enemyShooting.SetRot(new Vector3(newDir.x, newDir.y, newDir.z));
            Vector3 newPos = transform.position +
                             new Vector3(newDir.x, newDir.y, newDir.z) * (randLenght * _size[3]);

            while (Vector3.Distance(transform.position, newPos) > 0.05f)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
                yield return null;
            }

            for (int i = 0; i < randLenght; i++)
            {
                _busyPoints[coord.x + newDir[0] * i, coord.y + newDir[1] * i, coord.z
                 + newDir[2] * i] = false;
            }

            coord.x += newDir[0] * randLenght;
            coord.y += newDir[1] * randLenght;
            coord.z
             += newDir[2] * randLenght;
            pause = Random.Range(0f, 1f);
            yield return new WaitForSeconds(pause);
        }
    }

    private int FindLenght(Vector3Int coord, Vector3Int newDir)
    {
        int lenght = 1;
        if (newDir.x != 0)
        {
            while (!_busyPoints[coord.x + newDir.x * lenght, coord.y, coord.z
            ] &&
                   coord.x + newDir.x * lenght > 0 &&
                   coord.x + newDir.x * lenght < _size[0] - 1)
            {
                lenght++;
            }

            return lenght - 1;
        }

        if (newDir.y != 0)
        {
            while (!_busyPoints[coord.x, coord.y + newDir.y * lenght, coord.z
            ] &&
                   coord.y + newDir.y * lenght > 0 &&
                   coord.y + newDir.y * lenght < _size[1] - 1)
            {
                lenght++;
            }

            return lenght - 1;
        }

        while (!_busyPoints[coord.x, coord.y, coord.z
         + newDir.z * lenght] &&
               coord.z
                + newDir.z * lenght > 0 &&
               coord.z
                + newDir.z * lenght < _size[2] - 1)
        {
            lenght++;
        }

        return lenght - 1;
    }
}