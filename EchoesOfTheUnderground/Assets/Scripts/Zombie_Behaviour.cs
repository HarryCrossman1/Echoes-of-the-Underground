using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie_Behaviour : MonoBehaviour
{
    public static Zombie_Behaviour instance;
    private NavMeshAgent Zombie_Agent;
    public int ZombieHealth { get; set; }
    // Start is called before the first frame update
    void Awake()
    {
        instance= this;
        Zombie_Agent = gameObject.GetComponent<NavMeshAgent>();
        ZombieHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
       DeathCheck();
    }
    public void Chase(NavMeshAgent ZombieAgent,GameObject Target)
    {
        ZombieAgent.destination = Target.transform.position;
    }

    public void DeathCheck()
    {
        if (ZombieHealth <= 0) { gameObject.SetActive(false); }
    }
}
