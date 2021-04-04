using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perlin3D : MonoBehaviour
{
    public int xSize = 10;
    public int ySize = 10;
    public int zSize = 10;
    public float multiplyIn = 1;
    public float multiplyOut = 1;
    public float bounce = 0.5f;
    [Range(0, 9999)]
    public int offset=0;
    public int lightPeriod = 13;
 
    public GameObject cube;
    public int cubeScale=5;
    public GameObject lightCube;
    // Start is called before the first frame update
    void Start()
    {
        CreateRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CreateRoom()
    {
        for(int x=0; x<xSize;x++)
        {
            for(int y=0; y<xSize;y++)
            {
                for(int z=0;z<zSize;z++)
                {
                    if (PerlinNoise3D(x*multiplyIn+offset, y * multiplyIn + offset, z * multiplyIn + offset) >= bounce)
                    {
                         Instantiate(cube, new Vector3(x * cubeScale + transform.position.x, y * cubeScale + transform.position.y, z * cubeScale + transform.position.z), Quaternion.identity, transform);
                    }
                        
                    
                }
            }
        }

    }


     float PerlinNoise3D(float x, float y, float z)
    {
        float xy = Mathf.PerlinNoise(x, y);
        float xz = Mathf.PerlinNoise(x, z);
        float yz = Mathf.PerlinNoise(y, z);
        float yx = Mathf.PerlinNoise(y, x);
        float zx = Mathf.PerlinNoise(z, x);
        float zy = Mathf.PerlinNoise(z, y);
        Debug.Log((xy + xz + yz + yx + zx + zy) / 6 * multiplyOut);
        return (xy + xz + yz + yx + zx + zy) / 6*multiplyOut;
    }

  
}
