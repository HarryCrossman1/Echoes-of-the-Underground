//using AcmLib;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] ZombiePrefabs;
    [SerializeField] public List<GameObject> ZombiePool = new List<GameObject>();
    [SerializeField] public List<GameObject> ActiveZombies = new List<GameObject>();
    public List<GameObject> BulletWounds = new List<GameObject>();
    public int ZombiePoolAmount;
    private int RandomZombieAmount;

    public bool HasZombies;
    public static bool FixedSpawns=true;
    [SerializeField] public GameObject[] FixedSpawnsLocations; 
    private bool MovementExecuted;
    private int CurrentDifficulty;

    public bool IsActive;

    // Values For Difficulty 
    public float AccuracyRating = 100f;
    void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
    }
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        MovementExecuted = false;
        RandomZombieAmount = UnityEngine.Random.Range(1, ZombiePoolAmount);
        if (HasZombies)
        {
            ZombiePool.Clear();
            PoolZombies(ZombiePoolAmount);
        }
    }
    void Update()
    {
        GameplayLoop();
    }

    private void PoolZombies(int ZombieAmount)
    {
        for (int i = 0; i < ZombieAmount; i++)
        {
            int rand = UnityEngine.Random.Range(0, ZombiePrefabs.Length);
            GameObject Ins_Obj = Instantiate(ZombiePrefabs[rand]);

            Ins_Obj.SetActive(false);
            ZombiePool.Add(Ins_Obj);
        }
    }
    private void SpawnZombies(Vector3 SpawnPoint)
    {
        GameObject CurrentZomb = GetPooledZombie();
        if (CurrentZomb != null)
        {
            CurrentZomb.transform.position = SpawnPoint;
            CurrentZomb.SetActive(true);

            var zombie = CurrentZomb.GetComponent<Zombie_Behaviour>();

            // Reset values
            zombie.ZombieCurrentHealth = zombie.ZombieHealth;
            zombie.IsStunned = false;
            zombie.HasAtacked = false;

            // Audio & Anim
            zombie.PlayZombieAudio(zombie.AmbientAudio, true);
            SoundManager.instance.GetAudioSources();
            SoundManager.instance.SetAudioSources();
            zombie.ZombieCalledOnStart();

            ModifyCurrentZombie(CurrentZomb);
            CurrentZomb.GetComponent<NavMeshAgent>().isStopped = false;

            foreach (GameObject bloodObj in BulletWounds)
            {
                bloodObj.SetActive(false);
            }
            BulletWounds.Clear();

            var anim = zombie.ZombieAnimator;
            anim.SetBool("Walking", true);
            anim.SetBool("Idle", false);
        }
    }
    private void ModifyCurrentZombie(GameObject obj)
    {
        NavMeshAgent navMeshAgent = obj.GetComponent<NavMeshAgent>();

        if (navMeshAgent == null) { navMeshAgent = obj.AddComponent<NavMeshAgent>(); }

        navMeshAgent.acceleration = UnityEngine.Random.Range(obj.GetComponent<Zombie_Behaviour>().AccelMin, obj.GetComponent<Zombie_Behaviour>().AccelMax);
    }

    public GameObject GetPooledZombie()
    {
        for (int i = 0; i < ZombiePool.Count; i++)
        {
            if (!ZombiePool[i].activeInHierarchy)
            {
                return ZombiePool[i];
            }
        }
        return null;
    }
    private void GameplayLoop()
    {
        if (!IsActive)
        {
            if (!FixedSpawns)
            {
                // Debug.Log("Zombies spawning = " + RandomZombieAmount);
                for (int i = 0; i < RandomZombieAmount; i++)
                {
                    //Choose a random spawn point on z 
                    Vector3 RandomSpawn = new Vector3(PlayerController.instance.PlayerTransform.position.x + (UnityEngine.Random.Range(18, 30)), 0, PlayerController.instance.transform.position.z + (UnityEngine.Random.Range(-10, 10)));
                    Vector3 RandomSpawnBackup = new Vector3(PlayerController.instance.PlayerTransform.position.x + (UnityEngine.Random.Range(5, 10)), 0, PlayerController.instance.transform.position.z + (UnityEngine.Random.Range(-5, 5)));
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(RandomSpawn, out hit, 0.5f, NavMesh.AllAreas))
                    {
                        SpawnZombies(hit.position);
                        break;
                    }
                    else if (NavMesh.SamplePosition(RandomSpawnBackup, out hit, 0.5f, NavMesh.AllAreas))
                    {
                        SpawnZombies(hit.position);
                    }


                }
            }
            else
            {
                for (int i = 0; i < RandomZombieAmount; i++)
                {
                    int Rand = UnityEngine.Random.Range(0, FixedSpawnsLocations.Length);
                    SpawnZombies(FixedSpawnsLocations[Rand].transform.position);
                }
            }
        }

    }
    public void DebugAgents()
    {
        foreach (GameObject gb in ZombiePool)
        {
            NavMeshAgent agent = gb.GetComponent<NavMeshAgent>();
            Debug.Log($"{agent.name} - isOnNavMesh: {agent.isOnNavMesh}, hasPath: {agent.hasPath}, pathStatus: {agent.pathStatus}, remainingDistance: {agent.remainingDistance}");
        }
    }
}
