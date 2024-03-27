using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] ZombiePrefabs;
   [SerializeField] public List<GameObject> ZombiePool = new List<GameObject>();
    [SerializeField] public List<GameObject> ActiveZombies = new List<GameObject>();
    public List<GameObject> BulletWounds = new List<GameObject>();
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
        SpawnZombies(ZombieSpawnPoints[0], 5);
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
            int rand = Random.Range(0, ZombiePrefabs.Length);
            GameObject Ins_Obj = Instantiate(ZombiePrefabs[rand]);
            
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
                // Reset values 
                CurrentZomb.GetComponent<Zombie_Behaviour>().ZombieCurrentHealth = CurrentZomb.GetComponent<Zombie_Behaviour>().ZombieHealth;
                CurrentZomb.GetComponent<Zombie_Behaviour>().IsStunned = false;
                ModifyCurrentZombie(CurrentZomb);
                CurrentZomb.SetActive(true);
                CurrentZomb.GetComponent<NavMeshAgent>().isStopped = false;
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
    }
    private void ModifyCurrentZombie(GameObject obj)
    {
        NavMeshAgent navMeshAgent = obj.GetComponent<NavMeshAgent>();

        if (navMeshAgent == null) { navMeshAgent = obj.AddComponent<NavMeshAgent>(); }

        navMeshAgent.speed = Random.Range(obj.GetComponent<Zombie_Behaviour>().SpeedMin, obj.GetComponent<Zombie_Behaviour>().SpeedMax);
        navMeshAgent.acceleration = Random.Range(1f, 1.5f);
        navMeshAgent.angularSpeed = Random.Range(75, 165);
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
            player.position = Vector3.Lerp(player.position, target.position, 3f * Time.deltaTime);
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
                    SpawnZombies(ZombieSpawnPoints[rand], 3);
                    IsActive = true;
                    break;
                }
                
            }
            SpawnTracker = 0;
            MovePoints();
        }
    }
}
