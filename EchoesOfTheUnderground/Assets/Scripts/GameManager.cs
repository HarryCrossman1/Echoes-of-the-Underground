using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public GameObject ZombiePrefab;
    private GameObject CurrentZombie;

    public int ZombieAmount;
   [SerializeField] private Vector3 ZombieSpawnPoint;
    void Start()
    {
        SpawnZombie(ZombieSpawnPoint, ZombieAmount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnZombie(Vector3 SpawnPoint, int ZombieAmount)
    {
        for (int i = 0; i < ZombieAmount; i++)
        {
           GameObject Ins_Obj = Instantiate(ZombiePrefab, SpawnPoint, transform.rotation);
            ModifyCurrentZombie(Ins_Obj);
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
}
