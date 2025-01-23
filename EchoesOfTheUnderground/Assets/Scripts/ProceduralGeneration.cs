using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] private NavMeshSurface[] NavmeshSurface;
    [SerializeField] private GameObject Block;
    [SerializeField] private int WorldSizeX;
    [SerializeField] private int WorldSizeZ;
    [SerializeField] private int NoiseHeight;
    [SerializeField] private float GridSpacing;
    [Range(1,2)] [SerializeField] private float ObjectDensity;
    [Range(1, 2)][SerializeField] private float PickupDensity;
    [SerializeField] private GameObject[] PlaceableSceneObjects;
    [SerializeField] private GameObject[] PlaceablePickups;
    [SerializeField] private bool JustGenerated;

    private void Awake()
    {
        GenerateGrid(WorldSizeX, WorldSizeZ);
        //NavmeshSurface.BuildNavMesh();
        GenerateNavmesh();
       
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
                float RandomPlacement = Random.Range(0, 99);
                float PlacementChance = RandomPlacement * ObjectDensity;

                if (PlacementChance >= 70 && !JustGenerated)
                {
                    NavMeshHit hit;
                    if(NavMesh.SamplePosition(pos,out hit,1,NavMesh.AllAreas))
                    {
                        JustGenerated = true;
                        float RandomY = Random.Range(0, 360);
                        GameObject SceneObj = Instantiate(PlaceableSceneObjects[RandomObject], Grid.transform.position, Quaternion.Euler(0, RandomY, 0));
                        
                    }
                    
                   
                }
                else
                { 
                    JustGenerated = false;
                    GeneratePickup(Grid);
                }
                
;            }

        }
    }
    private void GeneratePickup(GameObject grid)
    {
        int RandomPickup = Random.Range(0, PlaceablePickups.Length);
        int RandomPickupPlacement = Random.Range(0, 99);
        float PickupChance = RandomPickupPlacement * PickupDensity;
        if (PickupChance >= 70)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(grid.transform.position, out hit, 1, NavMesh.AllAreas))
            {
                float RandomY = Random.Range(0, 360);
                GameObject scenePickup = Instantiate(PlaceablePickups[RandomPickup], grid.transform.position + new Vector3(0, 5, 0), Quaternion.Euler(0, RandomY, 0));
            }
                
        }
        
    }
    private void GenerateNavmesh()
    {
        for (int i = 0; i < NavmeshSurface.Length; i++)
        {
            NavmeshSurface[i].BuildNavMesh();
        }
    }
    //private void GeneratePoint(GameObject grid,GameObject Point)
}
