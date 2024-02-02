using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public GameManager instance;
    public GameObject ZombiePrefab;
    private GameObject CurrentZombie;
    private List<GameObject> ZombieList = new List<GameObject>();
    public int ZombieAmount { get { return ZombieAmount; } set { if (ZombieAmount > 24) ZombieAmount = 24; } }
    
   [SerializeField] private Vector3 ZombieSpawnPoint;
    public bool IsIdle;
    public int HordeDifficulty;
    private bool MovementExecuted;
    void Awake()
    {
        instance = this;
        SpawnZombie(ZombieSpawnPoint, ZombieAmount);
    }
    private void Start()
    {
        MovementExecuted= false;
        SetToLocation();
    }
    // Update is called once per frame
    void Update()
    {
        HordeActive();
    }

    private void SpawnZombie(Vector3 SpawnPoint, int ZombieAmount)
    {
        for (int i = 0; i < ZombieAmount; i++)
        {
           GameObject Ins_Obj = Instantiate(ZombiePrefab, SpawnPoint, transform.rotation);
           ModifyCurrentZombie(Ins_Obj);
           ZombieList.Add(Ins_Obj);
        }
    }
    private void ModifyCurrentZombie(GameObject obj)
    { 
        NavMeshAgent navMeshAgent = obj.GetComponent<NavMeshAgent>();
        if (navMeshAgent == null) { navMeshAgent = obj.AddComponent<NavMeshAgent>(); }

        float speed = Random.Range(1f, 1.5f);
        float accel = Random.Range(1f, 2f);
        float angle_speed = Random.Range(45, 120);
        navMeshAgent.speed = speed;
        navMeshAgent.acceleration = accel;
        navMeshAgent.angularSpeed= angle_speed;
    }
    private void HordeActive()
    {
        //Change after test later 
        if (IsIdle == true)
        {
            foreach (GameObject Zombie in ZombieList)
            {
                Zombie_Behaviour.instance.Chase(Zombie.GetComponent<NavMeshAgent>(), General_Referance.instance.Player);
            }
        }
        else
        {
            if (!MovementExecuted) { StartCoroutine(GoToPoint(15)); MovementExecuted = true; }
        }
    }
    private Vector3 FindPointOnNavmesh()
    {
        float Randx = Random.Range(-23, 17);
        float Randz = Random.Range(-50, 45);
        Vector3 newvec = new Vector3(Randx, 0, Randz);

        if (NavMesh.SamplePosition(newvec, out NavMeshHit hit, 1,NavMesh.AllAreas))
        {
            return newvec;
        }
        return gameObject.transform.position;
    }
    private IEnumerator GoToPoint(float Seconds)
    {
        yield return new WaitForSeconds(Seconds);
        SetToLocation();
        MovementExecuted = false;
    }
    private void SetToLocation()
    {
        foreach (GameObject Zombie in ZombieList)
        {
            Zombie.GetComponent<NavMeshAgent>().SetDestination(FindPointOnNavmesh());
        }
    }
}
