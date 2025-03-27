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
            GameObject Ins_Obj = Instantiate(ZombiePrefabs[rand],new Vector3(15,0.1f,3.6f),quaternion.identity);

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
            // Reset values 
            CurrentZomb.GetComponent<Zombie_Behaviour>().ZombieCurrentHealth = CurrentZomb.GetComponent<Zombie_Behaviour>().ZombieHealth;
            CurrentZomb.GetComponent<Zombie_Behaviour>().IsStunned = false;
            CurrentZomb.GetComponent<Zombie_Behaviour>().HasAtacked = false;
            ModifyCurrentZombie(CurrentZomb);
            CurrentZomb.SetActive(true);
            CurrentZomb.GetComponent<NavMeshAgent>().isStopped = false;
            CurrentZomb.GetComponent<Zombie_Behaviour>().ZombieCalledOnStart();
            // Remove blood 
            foreach (GameObject bloodObj in BulletWounds)
            {
                bloodObj.SetActive(false);
            }
            //Clear the list
            BulletWounds.Clear();
            //Set Animatons
            CurrentZomb.GetComponent<Animator>().SetBool("Walking", true);
            CurrentZomb.GetComponent<Animator>().SetBool("Idle", false);
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
           // Debug.Log("Zombies spawning = " + RandomZombieAmount);
            for (int i = 0; i < RandomZombieAmount; i++)
            {
                //Choose a random spawn point on z 
                Vector3 RandomSpawn = new Vector3(PlayerController.instance.PlayerTransform.position.x + (UnityEngine.Random.Range(20, 55)), 0, PlayerController.instance.transform.position.z + (UnityEngine.Random.Range(-10,10)));
                Vector3 RandomSpawnBackup = new Vector3(PlayerController.instance.PlayerTransform.position.x + (UnityEngine.Random.Range(5, 10)), 0, PlayerController.instance.transform.position.z + (UnityEngine.Random.Range(-5, 5)));
                NavMeshHit hit;
                if (NavMesh.SamplePosition(RandomSpawn, out hit, 0.5f, NavMesh.AllAreas))
                {
                    SpawnZombies(hit.position);
                    break;
                }
                else if(NavMesh.SamplePosition(RandomSpawnBackup, out hit, 0.5f, NavMesh.AllAreas))
                {
                    SpawnZombies(hit.position);
                }


            }
            
        }

    }
}
