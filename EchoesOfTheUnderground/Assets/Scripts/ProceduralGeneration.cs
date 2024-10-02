using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] private GameObject Block;
    [SerializeField] private int WorldSizeX;
    [SerializeField] private int WorldSizeZ;
    [SerializeField] private int NoiseHeight;
    [SerializeField] private float GridSpacing;
    [Range(1,100)] [SerializeField] private int ObjectDensity;
    [Range(1, 100)][SerializeField] private int PickupDensity;
    [SerializeField] private GameObject[] PlaceableSceneObjects;
    [SerializeField] private GameObject[] PlaceablePickups;
    [SerializeField] private bool JustGenerated;

    private void Start()
    {
        GenerateGrid(20,20);
    }

    private void GenerateGrid(int worldSizeX,int worldSizeZ)
    {
        for (int i = 0; i < worldSizeX; i++)
        {
            for (int l = 0; l < worldSizeZ; l++)
            {
                //Spawn the blocks 
                Vector3 pos = new Vector3(i * GridSpacing, 0, l * GridSpacing);
                GameObject Grid = Instantiate(Block, pos, Quaternion.identity);
                //Random gen the object placement (scene)
                int RandomObject = Random.Range(0, PlaceableSceneObjects.Length);
                float RandomPlacement = Random.Range(0, ObjectDensity + 1);
                int threshold = ObjectDensity / 2;

                if (RandomPlacement <= threshold && !JustGenerated)
                {
                    JustGenerated = true;
                    GameObject SceneObj = Instantiate(PlaceableSceneObjects[RandomObject], Grid.transform.position, Quaternion.identity);
                    GeneratePickup(Grid);
                   
                }
                else
                { 
                    JustGenerated = false;
                }
                
;            }

        }
    }
    private void GeneratePickup(GameObject grid)
    {
        int RandomPickup = Random.Range(0, PlaceablePickups.Length);
        GameObject scenePickup = Instantiate(PlaceablePickups[RandomPickup], grid.transform.position + new Vector3(0, 10, 0), Quaternion.identity);
    }
    //private float NoiseGeneration(int x, int z, float detailScale)
    //{
    //    float xNoise = (x + transform.position.x) / detailScale;
    //    float zNoise = (z + transform.position.y) / detailScale;

    //    return Mathf.PerlinNoise(xNoise, zNoise);
    //}
}
