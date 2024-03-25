using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie_Behaviour : MonoBehaviour
{
    public static Zombie_Behaviour instance;
    [SerializeField] private NavMeshAgent Zombie_Agent;
    private Animator ZombieAnimator;
    public int ZombieHealth; //{ get; set; }
    private bool HasAtacked;
    // Start is called before the first frame update
    void Awake()
    {
        ZombieAnimator = GetComponent<Animator>();
        instance = this;
        ZombieHealth = 100;
    }
    private void Start()
    {
        ZombieAnimator.SetBool("Walking", true);
        ZombieAnimator.SetBool("Idle", false);
    }
    // Update is called once per frame
    void Update()
    {
        DeathCheck();
        Chase(Zombie_Agent, PlayerController.instance.PlayerTransform.gameObject);
    }
    public void Chase(NavMeshAgent ZombieAgent, GameObject Target)
    {
        if (Vector3.Distance(gameObject.transform.position, Target.transform.position) > 1.5f)
        {
            ZombieAgent.destination = Target.transform.position;
        }
        else
        {
            ZombieAgent.SetDestination(gameObject.transform.position);
            Attack();
        }
    }
    private void Attack()
    {
        if (!HasAtacked)
        { 
            HasAtacked= true;
            StartCoroutine(StartAttack(2));
        }
    }
    private IEnumerator StartAttack(float cooldown) 
    {
        ZombieAnimator.SetBool("Attacking", true);
        ZombieAnimator.SetBool("Walking", false);
        PlayerController.instance.PlayerHealth--;
        yield return new WaitForSeconds(cooldown);
        HasAtacked= false;
    }
    public void DeathCheck()
    {
        if (ZombieHealth <= 0)
        {
            Zombie_Agent.SetDestination(gameObject.transform.position);
            StartCoroutine(WaitForAnim(2f));
            ZombieAnimator.SetBool("Dead", true);
            ZombieAnimator.SetBool("Walking", false);
            ZombieAnimator.SetBool("Attacking", false);
           
        }
    }
    private IEnumerator WaitForAnim(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
        CheckActiveZombies();
    }
    public void ShotStun()
    {
        Zombie_Agent.speed = (Zombie_Agent.speed / 2);
        StartCoroutine(StunTimer(1));
    }
    private IEnumerator StunTimer(float TimerLength)
    { 
        yield return new WaitForSeconds(TimerLength);
        Zombie_Agent.speed = (Zombie_Agent.speed * 1.5f);
    }
    private void CheckActiveZombies()
    {
        for (int i = 0; i < GameManager.instance.ZombiePool.Count; i++)
        {
            // check if there are any zombies active in the hierarchy 
            if (GameManager.instance.ZombiePool[i].activeInHierarchy)
            {
                GameManager.instance.IsActive = true;
                break;
            }
            else
            {
                // if none are active 
                GameManager.instance.IsActive = false;
            }
        }
    }
}
