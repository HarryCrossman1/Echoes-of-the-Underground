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
    public int ZombieAmount;
   [SerializeField] private Vector3 ZombieSpawnPoint;
    public bool IsIdle;
    public int HordeDifficulty;
    void Awake()
    {
        instance = this;
        SpawnZombie(ZombieSpawnPoint, ZombieAmount);
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

        float speed = Random.Range(3f, 6.5f);
        float accel = Random.Range(3, 7);
        float angle_speed = Random.Range(70, 210);
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
            foreach (GameObject Zombie in ZombieList)
            {
                Zombie.GetComponent<NavMeshAgent>().SetDestination(Zombie.transform.position);
            }
        }
    }
    private Vector3 NextLocation()
    {
        float Randx = Random.Range(-63, 80);
        float Randz = Random.Range(-80, 80);
        Vector3 newvec = new Vector3(Randx, 0, Randz);
        return newvec;
    }
}
