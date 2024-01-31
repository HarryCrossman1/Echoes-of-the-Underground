using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie_Behaviour : MonoBehaviour
{
    public General_Referance General_referance;
    private NavMeshAgent Zombie_Agent;
    // Start is called before the first frame update
    void Awake()
    {
        General_referance = GameObject.FindObjectOfType<General_Referance>();
        Zombie_Agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Chase(General_referance.Player);
    }
    private void Chase(GameObject Target)
    {
        Zombie_Agent.destination = Target.transform.position;
    }
 
}
