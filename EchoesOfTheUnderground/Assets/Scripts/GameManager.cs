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
    
    public bool IsIdle;
    public int HordeDifficulty;
    private bool MovementExecuted;
    private int CurrentDifficulty;
    [SerializeField] private Transform[] SetPoints;
    [SerializeField] private Transform[] ZombieSpawnPoints;
    [SerializeField] private int SetPointTracker,SpawnTracker;
    void Awake()
    {
        instance = this;
        CurrentDifficulty = PlayerPrefs.GetInt("Difficulty");
        PoolZombies(ZombiePoolAmount);
        SpawnZombies(ZombieSpawnPoints[5], 3);
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
            int rand = UnityEngine.Random.Range(0, ZombiePrefabs.Length);
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
                CurrentZomb.GetComponent<Zombie_Behaviour>().ZombieInRange = false;
                CurrentZomb.GetComponent<Zombie_Behaviour>().HasAtacked = false;
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

        navMeshAgent.speed = UnityEngine.Random.Range(obj.GetComponent<Zombie_Behaviour>().SpeedMin, obj.GetComponent<Zombie_Behaviour>().SpeedMax);
        navMeshAgent.acceleration = UnityEngine.Random.Range(1f, 1.5f);
        navMeshAgent.angularSpeed = UnityEngine.Random.Range(75, 165);
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
        if (SetPointTracker == SetPoints.Length+1)
        {
            HighScoreManager.instance.Save();
            SceneManager.LoadScene("MenuScene");
        }
        if (!IsActive)
        {
            for (int i = 0; i < ZombieSpawnPoints.Length; i++)
            {
                //check if the spawn point is too close 
                int rand = UnityEngine.Random.Range(0, ZombieSpawnPoints.Length);
                while (Vector3.Distance(ZombieSpawnPoints[rand].position, PlayerController.instance.PlayerTransform.position) > 10f && SpawnTracker < 3)
                {
                    // if good then spawn here 
                    SpawnTracker++;
                    float Difficulty = Math.Squared(CurrentDifficulty) + SetPointTracker;
                    double DifficultyModified = Math.SquareRoot((double)Difficulty, 0.001);
                    float SpawnNum = math.ceil((float)DifficultyModified);
                    
                    if (SpawnNum < 1)
                    { 
                        SpawnNum = 1;
                    }
                    SpawnZombies(ZombieSpawnPoints[rand], (int)SpawnNum);
                    Debug.Log(SpawnNum);
                    IsActive = true;
                    break;
                }
                
            }
            SpawnTracker = 0;
            MovePoints();
        }
    }
}
