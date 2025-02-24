using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class WorldData
{
    public List<Vector3> BlockPositions = new List<Vector3>();
    public List<Quaternion> ObjectRotation = new List<Quaternion>();
    public List<string> ObjectType = new List<string>();
}

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] private NavMeshSurface[] NavmeshSurface;
    [SerializeField] private GameObject Block;
    [SerializeField] private int WorldSizeX;
    [SerializeField] private int WorldSizeZ;
    [SerializeField] private float GridSpacing;
    [SerializeField] [Range(0,100)] public int CarChance;
    private bool JustGenerated;
    [SerializeField] private GameObject[] PlaceableSceneObjects;
    [SerializeField] private GameObject[] PlaceablePickups;
   [SerializeField] private List<GameObject> spawnedBlocks = new List<GameObject>();
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private string saveFilePath;
    private int Index;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "worlddata.json");
        if (File.Exists(saveFilePath))
        {
            LoadWorld();
        }
        else
        {
            GenerateGrid(WorldSizeX, WorldSizeZ);
           
            GenerateNavmesh();
           
        }  
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        { 
            SaveWorld();
        }
    }

    private void GenerateGrid(int worldSizeX,int worldSizeZ)
    {
        for (int i = 0; i < worldSizeX; i++)
        {
            for (int l = 0; l < worldSizeZ; l++)
            {
                //Spawn the Cars 
                Vector3 pos = new Vector3(gameObject.transform.position.x + (i * GridSpacing), 0, gameObject.transform.position.z + (l * GridSpacing));
                NavMeshHit hit;
                if (NavMesh.SamplePosition(pos, out hit, 0.2f, NavMesh.AllAreas))
                {
                    if (JustGenerated && Random.Range(0, 100) > CarChance)
                    {
                        GameObject gridblock = Instantiate(PlaceableSceneObjects[Random.Range(0, 2)], pos, Quaternion.Euler(0,Random.Range(0,360),0));
                        spawnedBlocks.Add(gridblock);
                        JustGenerated = false;
                    }
                    // Spawn the item pickups
                    if (Random.Range(0, 100) < 30)
                    {
                       GameObject Pickup = Instantiate(PlaceablePickups[Random.Range(0, PlaceablePickups.Length)],pos, Quaternion.Euler(0, Random.Range(0, 360), 0));
                        spawnedObjects.Add(Pickup);
                    }
                }
                else 
                JustGenerated = true;
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

    public void SaveWorld()
    {
        WorldData worldData = new WorldData();
        //Cars Save 
        foreach (GameObject block in spawnedBlocks)
        {
            worldData.BlockPositions.Add(block.transform.position);
            worldData.ObjectRotation.Add(block.transform.rotation);
            worldData.ObjectType.Add(block.name);
        }
        //Objects Save 
        foreach (GameObject Pickup in spawnedObjects)
        {
            worldData.BlockPositions.Add(Pickup.transform.position);
            worldData.ObjectRotation.Add(Pickup.transform.rotation);
            worldData.ObjectType.Add(Pickup.name);
        }
        // Write that to a json file, i love data readablility !!!!
        string json = JsonUtility.ToJson(worldData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Saving world to: " + saveFilePath);
    }

    public void LoadWorld()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogError("No Save file");
            return;
        }

        // Take the data from the file 
        string json = File.ReadAllText(saveFilePath);
        WorldData worldData = JsonUtility.FromJson<WorldData>(json);

        // Now Spawn!  
        for (int i = 0; i < worldData.BlockPositions.Count; i++)
        {
            Debug.Log(worldData.ObjectType[i]);
            Debug.Log(worldData.BlockPositions[i]);
            Debug.Log(worldData.ObjectRotation[i]);

            GameObject SpawnedObject = Instantiate(ReturnPrefab(worldData.ObjectType[i]), worldData.BlockPositions[i], worldData.ObjectRotation[i]);
        }


        GenerateNavmesh();
        Debug.Log("World loaded successfully!");
    }

    public GameObject ReturnPrefab(string Name)
    {
        string NewName = Name.Replace("(Clone)", "");
        Debug.Log(NewName);
        foreach (GameObject Car in PlaceableSceneObjects)
        {
            if (Car.name == NewName)
            {
                return Car;
            }
        }
        foreach (GameObject Pickup in PlaceablePickups)
        {
            if (Pickup.name == NewName)
            {
                return Pickup;
            }
        }
        return null;
    }
}
