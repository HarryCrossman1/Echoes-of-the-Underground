using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie_Behaviour : MonoBehaviour
{
    public static Zombie_Behaviour instance;
    [SerializeField] private NavMeshAgent Zombie_Agent;
    public Animator ZombieAnimator;
    public int ZombieCurrentHealth,ZombieHealth; //{ get; set; }
    private bool HasAtacked,ZombieInRange;
   [SerializeField] public bool IsStunned;
    public float SpeedMin,SpeedMax;

    // Store Animations 
    [SerializeField] private AnimationClip Attacking, Hit, Dead;
    // Zombie Audio
    [SerializeField] private AudioSource ZombieSource;
    [SerializeField] private AudioClip AttackAudio, ShotAudio,DeathAudio,AmbientAudio;
    void Awake()
    {
        ZombieSource = GetComponent<AudioSource>();
        ZombieAnimator = GetComponent<Animator>();
        instance = this;
        ZombieInRange= false;
    }
    private void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        DeathCheck();
        Chase(Zombie_Agent, PlayerController.instance.PlayerTransform.gameObject);
    }
    public void Chase(NavMeshAgent ZombieAgent, GameObject Target)
    {
        if (Vector3.Distance(gameObject.transform.position, Target.transform.position) > 1f)
        {
            ZombieInRange = false;
            if (!IsStunned)
            ZombieAgent.destination = Target.transform.position;
        }
        else
        {
            ZombieAgent.SetDestination(gameObject.transform.position);
            ZombieInRange = true;
            Attack();
        }
    }
    private void Attack()
    {
        if (!HasAtacked)
        { 
            HasAtacked= true;
            StartCoroutine(StartAttack(Attacking.length));
        }
    }
    private IEnumerator StartAttack(float cooldown) 
    {
        ZombieAnimator.SetBool("Attacking", true);
        ZombieAnimator.SetBool("Walking", false);
        ZombieAnimator.SetBool("Stunned", false);
        PlayZombieAudio(AttackAudio, false);
        PlayerController.instance.PlayerDeathCheck();
        yield return new WaitForSeconds(cooldown);
        PlayerController.instance.PlayerHealth--;
        //Could bug If player kills during anim
        HasAtacked = false;
    }
    public void DeathCheck()
    {
        if (ZombieCurrentHealth <= 0)
        {
            PlayZombieAudio(DeathAudio, false);
            Zombie_Agent.isStopped=true;
            StartCoroutine(WaitForAnim(3f));
            ZombieAnimator.SetBool("Dead", true);
            ZombieAnimator.SetBool("Walking", false);
            ZombieAnimator.SetBool("Attacking", false);
            ZombieAnimator.SetBool("Stunned", false);

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
        IsStunned= true;
        if (IsStunned)
        {
            
            ZombieAnimator.SetBool("Walking", false);
            ZombieAnimator.SetBool("Attacking", false);
            ZombieAnimator.SetBool("Stunned", true);
            StartCoroutine(StunTimer(Hit.length));
        }
        
    }
    private IEnumerator StunTimer(float TimerLength)
    {
        Zombie_Agent.isStopped = true;
        PlayZombieAudio(ShotAudio,false);
        yield return new WaitForSeconds(TimerLength);
        PlayZombieAudio(AmbientAudio,true);
        
        Zombie_Agent.isStopped = false;
        IsStunned = false;
        if (ZombieInRange)
        {
            ZombieAnimator.SetBool("Stunned", false);
            ZombieAnimator.SetBool("Attacking", true);
        }
        else
        {
            ZombieAnimator.SetBool("Stunned", false);
            ZombieAnimator.SetBool("Walking", true);
        }
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
    public void PlayZombieAudio(AudioClip clip,bool loop)
    {
        if (!loop)
        {
            ZombieSource.clip = clip;
            ZombieSource.Play();
            ZombieSource.loop = false;
        }
        else { ZombieSource.loop = true; }
    }
}
