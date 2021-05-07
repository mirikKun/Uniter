using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Photon.Pun;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int outerFactor = 5;
    [SerializeField] private int enemyCount = 20;
    [SerializeField] private int gravityGunCount = 10;


    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject gravityGun;
    [SerializeField] private GameObject cube;

    private bool[,,] _busyPoints;
    private int[] _size;

    private void Awake()
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
        if (FillOptions.join)
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
        if (!_busyPoints[newPlace.x, newPlace.y - 1, newPlace.z] && newPlace.y - 1 > 1)
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
        Vector3Int newPoint = RandomPoint();

        while (_busyPoints[newPoint.x, newPoint.y, newPoint.z])
        {
            newPoint = RandomPoint();
        }

        _busyPoints[newPoint.x, newPoint.y, newPoint.z] = true;
        return newPoint;
    }

    private Vector3Int RandomPoint()
    {
        int x = Random.Range(_size[0] / outerFactor, _size[0] - _size[0] / outerFactor);
        int y = Random.Range(_size[1] / outerFactor, _size[1] - _size[1] / outerFactor);
        int z = Random.Range(_size[2] / outerFactor, _size[2] - _size[2] / outerFactor);
        return new Vector3Int(x, y, z);
    }

    private void EnemySpawner()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3Int newPlace = FindPlaceToSpawn();
            GameObject newEnemy = PhotonNetwork.Instantiate(enemy.name, new Vector3(
                newPlace.x * _size[3] + transform.position.x,
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
}