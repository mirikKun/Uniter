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
    [Range(0, 9999)]
    public int offset=0;
 
    public GameObject cube;
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
        for(int x=0;x<xSize;x=x+2)
        {
            for(int y=0; y<xSize;y=y+2)
            {
                for(int z=0;z<zSize;z=z+2)
                {
                    if(AreInstantiate( PerlinNoise3D(x,y,z)))
                        {
                        Instantiate(cube, new Vector3(x, y, z), Quaternion.identity, transform);
                    }

                }
            }
        }

    }


     float PerlinNoise3D(float x, float y, float z)
    {
        float xy = CustomNoize(x, y);
        float xz = CustomNoize(x, z);
        float yz = CustomNoize(y, z);
        float yx = CustomNoize(y, x);
        float zx = CustomNoize(z, x);
        float zy = CustomNoize(z, y);

        return (xy + xz + yz + yx + zx + zy) / 6*multiplyOut;
    }
    float CustomNoize(float x,float y)
    {
        return Mathf.PerlinNoise(x*multiplyIn + offset, y*multiplyIn + offset);
    }

    bool AreInstantiate(float noiseValue)
        {
        if (noiseValue > 0.5f)
            return true;
        return false;
}
}
