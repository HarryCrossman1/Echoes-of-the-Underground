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
    public static ProceduralGeneration Instance;
    [SerializeField] private NavMeshSurface[] NavmeshSurface;
    [SerializeField] private int WorldSizeX;
    [SerializeField] private int WorldSizeZ;
    [SerializeField] private float GridSpacing;
    [Range(0,100)] public int CarChance;
    [Range(0, 100)] public int PickupChance;
    
    [SerializeField] private GameObject[] PlaceableSceneObjects;
    [SerializeField] private GameObject[] PlaceablePickups;
    [SerializeField] private GameObject[] KeyItems;
    private int ItemsIndex;
    [HideInInspector] public List<GameObject> spawnedBlocks = new List<GameObject>();
    [HideInInspector] public List<GameObject> spawnedObjects = new List<GameObject>();
    [SerializeField] private string saveFilePath;
    private void Awake()
    {
        Instance = this;
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
                    if (Random.Range(0, 100) < CarChance)
                    {
                        GameObject gridblock = Instantiate(PlaceableSceneObjects[Random.Range(0, 2)], pos, Quaternion.Euler(0, Random.Range(0, 360), 0));
                        spawnedBlocks.Add(gridblock);
                    }
                    // Spawn the item pickups
                    else if (Random.Range(0, 100) < PickupChance)
                    {
                        GameObject Pickup = Instantiate(PlaceablePickups[Random.Range(0, PlaceablePickups.Length)], pos, Quaternion.Euler(0, Random.Range(0, 360), 0));
                        spawnedObjects.Add(Pickup);
                    }
                }
                if (worldSizeZ == WorldSizeZ && ItemsIndex < KeyItems.Length)
                {
                   
                    for (int a = ItemsIndex; a < KeyItems.Length; a++)
                    {
                        Vector3 RandomPos = new Vector3(gameObject.transform.position.x + (Random.Range(0, WorldSizeX) * GridSpacing), 0, gameObject.transform.position.z + (Random.Range(0, WorldSizeZ) * GridSpacing));
                        NavMeshHit RandHit;
                        if (NavMesh.SamplePosition(RandomPos, out RandHit, 0.1f, NavMesh.AllAreas))
                        {
                            GameObject Item = Instantiate(KeyItems[ItemsIndex], RandomPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
                            Item.AddComponent<KeyItemsCollider>();
                            spawnedObjects.Add(Item);
                            ItemsIndex++;
                        }
                    }
                }
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
        saveFilePath = Path.Combine(Application.persistentDataPath, "WorldData.json");
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
        if (saveFilePath != null)
        {
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Saving world to: " + saveFilePath);
        }
        else
            Debug.Log("No date to save, try running the game then saving");
        
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
    public void DeleteWorld()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            saveFilePath = null;
            Debug.Log("File deleted");
        }
        else
            Debug.Log("No file to delete");
    }
    public void OpenFile()
    {
        if (saveFilePath != null)
        {
            try
            {
                System.Diagnostics.Process.Start(saveFilePath);
            }
            catch
            {
                Debug.LogError("System error, no save file path present");
            }
        }
        else
            Debug.Log("File has been deleted, so it can't open");
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
        foreach (GameObject KeyItem in KeyItems)
        {
            if (KeyItem.name == NewName)
            {
                return KeyItem;
            }
        }
        return null;
    }
}
