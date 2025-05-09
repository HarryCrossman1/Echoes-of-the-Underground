using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject[] ZombiePrefabs;
    public List<GameObject> ZombiePool = new List<GameObject>();
    public List<GameObject> ActiveZombies = new List<GameObject>();
    public List<GameObject> BulletWounds = new List<GameObject>();
    public int ZombiePoolAmount;
    public bool HasZombies;
    public static bool FixedSpawns;
    [SerializeField] private GameObject[] FixedSpawnsLocations;


    public bool IsActive;
    public bool WaitingForZombies=true;

    // Values For Difficulty 
    public float AccuracyRating = 100f;
    void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }
    private void Start()
    {
    }
    public void Init()
    {
        ZombiePool.Clear();
        //BuildLogger.Instance.BuildLog(ZombiePoolAmount.ToString());
        if (HasZombies)
        {
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
            //Choose a random prefab and instantiate it 
            int rand = UnityEngine.Random.Range(0, ZombiePrefabs.Length);
            GameObject Ins_Obj = Instantiate(ZombiePrefabs[rand]);
            // Set it to inactive and add it too the pool
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

            var Zombie = CurrentZomb.GetComponent<Zombie_Behaviour>();

            // Reset values
            Zombie.ZombieCurrentHealth = Zombie.ZombieHealth;
            Zombie.HasAtacked = false;

            // Audio & Anim
            Zombie.PlayZombieAudio(Zombie.AmbientAudio, true);
            SoundManager.Instance.GetAudioSources();
            SoundManager.Instance.SetAudioSources();
            Zombie.ZombieCalledOnStart();

            ModifyCurrentZombie(CurrentZomb);
            CurrentZomb.GetComponent<NavMeshAgent>().isStopped = false;

            foreach (GameObject BloodObj in BulletWounds)
            {
                if (BloodObj != null)
                {
                    BloodObj.SetActive(false);
                }
            }
            BulletWounds.Clear();

            var Anim = Zombie.ZombieAnimator;
            Anim.SetBool("Walking", true);
            Anim.SetBool("Idle", false);
            Anim.SetBool("Dead", false);
            Anim.SetBool("Attacking", false);
        }
    }
    private void ModifyCurrentZombie(GameObject Obj)
    {
        NavMeshAgent NavMeshAgent = Obj.GetComponent<NavMeshAgent>();

        if (NavMeshAgent == null)
        { 
            NavMeshAgent = Obj.AddComponent<NavMeshAgent>(); 
        }

        NavMeshAgent.acceleration = UnityEngine.Random.Range(Obj.GetComponent<Zombie_Behaviour>().AccelMin, Obj.GetComponent<Zombie_Behaviour>().AccelMax);
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
        if (!IsActive && Instance!=null)
        {
            if (WaitingForZombies == true && HasZombies)
            {
                StartCoroutine(WaitForZombies());
                WaitingForZombies= false;
                Debug.Log(HasZombies);
            }
        }

    }
    private IEnumerator WaitForZombies()
    {
        if (!FixedSpawns)
        {
            yield return new WaitForSeconds(12);
            if (HasZombies)
            {
                for (int i = 0; i < ZombiePoolAmount; i++)
                {
                    //Choose a random spawn point on z 
                    Vector3 RandomSpawn = new Vector3(PlayerController.Instance.PlayerTransform.position.x + (UnityEngine.Random.Range(18, 30)), 0, PlayerController.Instance.transform.position.z + (UnityEngine.Random.Range(-10, 10)));
                    Vector3 RandomSpawnBackup = new Vector3(PlayerController.Instance.PlayerTransform.position.x + (UnityEngine.Random.Range(5, 10)), 0, PlayerController.Instance.transform.position.z + (UnityEngine.Random.Range(-5, 5)));
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(RandomSpawn, out hit, 0.5f, NavMesh.AllAreas))
                    {
                        SpawnZombies(hit.position);
                    }
                    else if (NavMesh.SamplePosition(RandomSpawnBackup, out hit, 0.5f, NavMesh.AllAreas))
                    {
                        SpawnZombies(hit.position);
                    }
                }
                WaitingForZombies = true;
            }
        }
        else
        {
            yield return new WaitForSeconds(8);
            if (HasZombies)
            {
                for (int i = 0; i < ZombiePoolAmount; i++)
                {
                    int Rand = UnityEngine.Random.Range(0, FixedSpawnsLocations.Length);
                    if (FixedSpawnsLocations[Rand] != null && Vector3.Distance(PlayerController.Instance.PlayerTransform.position, FixedSpawnsLocations[Rand].transform.position) > 10)
                    {
                        SpawnZombies(FixedSpawnsLocations[Rand].transform.position);
                    }
                }
                WaitingForZombies = true;
            }
        }
    }
    public void DebugAgents()
    {
        foreach (GameObject Gb in ZombiePool)
        {
            NavMeshAgent agent = Gb.GetComponent<NavMeshAgent>();
            Debug.Log($"{agent.name} - isOnNavMesh: {agent.isOnNavMesh}, hasPath: {agent.hasPath}, pathStatus: {agent.pathStatus}, remainingDistance: {agent.remainingDistance}");
        }
    }
    public void GameManagerSetup()
    {
        if (GameObject.Find("SpawnLocationOne") != null)
        {
            FixedSpawnsLocations[0] = GameObject.Find("SpawnLocationOne");
        }
        if (GameObject.Find("SpawnLocationTwo") != null)
        {
            FixedSpawnsLocations[1] = GameObject.Find("SpawnLocationTwo");
        }
    }

}
