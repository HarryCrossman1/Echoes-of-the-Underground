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
        GenerateGrid(WorldSizeX, WorldSizeZ);
    }

    private void GenerateGrid(int worldSizeX,int worldSizeZ)
    {
        for (int i = 0; i < worldSizeX; i++)
        {
            for (int l = 0; l < worldSizeZ; l++)
            {
                //Spawn the blocks 
                Vector3 pos = new Vector3(gameObject.transform.position.x + (i * GridSpacing), 0, gameObject.transform.position.z + (l * GridSpacing));
                GameObject Grid = Instantiate(Block, pos, Quaternion.identity);
                //Random gen the object placement (scene)
                int RandomObject = Random.Range(0, PlaceableSceneObjects.Length);
                float RandomPlacement = Random.Range(0, ObjectDensity + 1);
                int threshold = ObjectDensity / 2;

                if (RandomPlacement <= threshold && !JustGenerated)
                {
                    JustGenerated = true;
                    float RandomY = Random.Range(0, 360);
                    GameObject SceneObj = Instantiate(PlaceableSceneObjects[RandomObject], Grid.transform.position, Quaternion.Euler(0,RandomY,0));
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
}
