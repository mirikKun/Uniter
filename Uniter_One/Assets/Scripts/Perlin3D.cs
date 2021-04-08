using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Perlin3D : MonoBehaviour
{
    public int xSize = 20;
    public int ySize = 20;
    public int zSize = 20;
    public float multiplyIn = 1;
    public float bounce = 0.5f;
    [Range(0, 9999)] public int offset = 0;


    public GameObject cube;
    public int cubeScale = 5;

    public bool lightEvalable = true;
    public int lightPeriod = 13;

    public GameObject lightCube;
    public Material material;


    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        CreateRoom();
    }

    void PutCharacter(bool[,,] busy)
    {
        player.GetComponent<FpMovementRigid>().Place(busy, transform, cubeScale, cube);
    } 

    // Update is called once per frame
    void Update()
    {
    }

    void CreateRoom()
    {
        List<CombineInstance> combine = new List<CombineInstance>();

        MeshFilter blockMesh = Instantiate(cube, Vector3.zero, Quaternion.identity).GetComponent<MeshFilter>();
        bool[,,] busy = new bool[xSize, ySize, zSize];
        for (int x = 0, i = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    if (PerlinNoise3D(x * multiplyIn + offset, y * multiplyIn + offset, z * multiplyIn + offset) >=
                        bounce)
                    {
                        i++;
                        blockMesh.transform.position = new Vector3(x * cubeScale + transform.position.x,
                            y * cubeScale + transform.position.y,
                            z * cubeScale + transform.position.z);
                        if (i % lightPeriod == 0 && lightEvalable)
                            Instantiate(lightCube, new Vector3(x * cubeScale + transform.position.x,
                                y * cubeScale + transform.position.y,
                                z * cubeScale + transform.position.z), Quaternion.identity, transform);
                        else
                            combine.Add(new CombineInstance
                                {mesh = blockMesh.sharedMesh, transform = blockMesh.transform.localToWorldMatrix});
                    }
                    else
                    {
                        busy[x, y, z] = true;
                    }
                }
            }
        }

        //  Destroy(blockMesh);
        List<List<CombineInstance>> combineList = new List<List<CombineInstance>>();
        int vertexCount = 0;
        combineList.Add(new List<CombineInstance>());
        for (int i = 0; i < combine.Count; i++)
        {
            vertexCount += combine[i].mesh.vertexCount;
            if (vertexCount > 60000)
            {
                vertexCount = 0;
                combineList.Add(new List<CombineInstance>());
                i--;
            }
            else
            {
                {
                    combineList.Last().Add(combine[i]);
                }
            }
        }


        foreach (List<CombineInstance> list in combineList)
        {
            GameObject g = new GameObject("Meshy");
            g.transform.parent = transform;
            MeshFilter mf = g.AddComponent<MeshFilter>();
            MeshRenderer mr = g.AddComponent<MeshRenderer>();
            g.AddComponent<MeshCollider>();
            g.layer = LayerMask.NameToLayer("Ground");
            mr.material = material;
            mf.mesh.CombineMeshes(list.ToArray());
            g.AddComponent<MeshCollider>();
            g.isStatic = true;
        }

        PutCharacter(busy);
    }

    float PerlinNoise3D(float x, float y, float z)
    {
        float xy = Mathf.PerlinNoise(x, y);
        float xz = Mathf.PerlinNoise(x, z);
        float yz = Mathf.PerlinNoise(y, z);
        float yx = Mathf.PerlinNoise(y, x);
        float zx = Mathf.PerlinNoise(z, x);
        float zy = Mathf.PerlinNoise(z, y);
        return (xy + xz + yz + yx + zx + zy) / 6;
    }
}