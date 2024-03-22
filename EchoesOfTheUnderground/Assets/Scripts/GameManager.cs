using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject ZombiePrefab;
    private GameObject CurrentZombie;
   [SerializeField] public List<GameObject> ZombiePool = new List<GameObject>();
    [SerializeField] public List<GameObject> ActiveZombies = new List<GameObject>();
    public int ZombiePoolAmount;
    
    public bool IsIdle;
    public int HordeDifficulty;
    private bool MovementExecuted;

    [SerializeField] private Transform[] SetPoints;
    [SerializeField] private Transform[] ZombieSpawnPoints;
    [SerializeField] private int SetPointTracker,SpawnTracker;
    void Awake()
    {
        instance = this;
        PoolZombies(ZombiePoolAmount);
        SpawnZombies(ZombieSpawnPoints[0], 10);
    }
    private void Start()
    {
        MovementExecuted= false;
        IsIdle = true;
        // SetToLocation();
    }
    // Update is called once per frame
    void Update()
    {
        GameplayLoop();
    }

    private void PoolZombies(int ZombieAmount)
    {
        for (int i = 0; i < ZombieAmount; i++)
        {
            GameObject Ins_Obj = Instantiate(ZombiePrefab);
            ModifyCurrentZombie(Ins_Obj);
            Ins_Obj.SetActive(false);
            ZombiePool.Add(Ins_Obj);
        }
    }
    private void SpawnZombies(Transform SpawnPoint,int ZombieAmount)
    {
        for (int i = 0; i < ZombieAmount; i++)
        {
            GameObject CurrentZomb = GetPooledZombie();
            if (CurrentZomb != null)
            {
                CurrentZomb.transform.position = SpawnPoint.position;
                CurrentZomb.GetComponent<Zombie_Behaviour>().ZombieHealth = 100;
                CurrentZomb.SetActive(true);
            }

        }
    }
    private void ModifyCurrentZombie(GameObject obj)
    { 
        NavMeshAgent navMeshAgent = obj.GetComponent<NavMeshAgent>();
       
        if (navMeshAgent == null) { navMeshAgent = obj.AddComponent<NavMeshAgent>(); }

        navMeshAgent.speed = 2.1f;//Random.Range(1f, 3f);
        navMeshAgent.acceleration = Random.Range(1f, 2f);
        navMeshAgent.angularSpeed = Random.Range(45, 95);
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


    private IEnumerator LerpToNextPoint(Transform player,Transform target)
    {
        while (Vector3.Distance(player.position, target.position) > 0.05f)
        {
            player.position = Vector3.Lerp(player.position, target.position, 1.3f * Time.deltaTime);
            yield return null;  
        }
    }
    private void MovePoints()
    {
        if (SetPointTracker < SetPoints.Length)
        {
            StartCoroutine(LerpToNextPoint(PlayerController.instance.PlayerTransform, SetPoints[SetPointTracker]));
            SetPointTracker++;
        }
    }
   [SerializeField] public bool IsActive;
    private void GameplayLoop()
    {
        if (!IsActive)//Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < ZombieSpawnPoints.Length; i++)
            {
                //check if the spawn point is too close 
                int rand = Random.Range(0, ZombieSpawnPoints.Length);
                while (Vector3.Distance(ZombieSpawnPoints[rand].position, PlayerController.instance.PlayerTransform.position) > 10f && SpawnTracker < 2)
                {
                    // if good then spawn here 
                    SpawnTracker++;
                    SpawnZombies(ZombieSpawnPoints[rand], 5);
                    IsActive = true;
                    break;
                }
                SpawnTracker = 0;
            }
            MovePoints();
        }
    }
}
