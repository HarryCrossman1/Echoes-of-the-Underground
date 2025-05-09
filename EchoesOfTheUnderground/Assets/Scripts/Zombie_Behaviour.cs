using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie_Behaviour : MonoBehaviour
{
    [SerializeField] private NavMeshAgent Zombie_Agent;
    public Animator ZombieAnimator;
    public int zombieCurrentHealth,ZombieHealth;
    public bool HasAtacked;
    public float AccelMin,AccelMax;

    // Store Animations 
    [SerializeField] private AnimationClip Attacking, Hit, Dead;
    // Zombie Audio
    [SerializeField] private AudioSource ZombieSource;
    public AudioClip AttackAudio, ShotAudio,DeathAudio,AmbientAudio;

    public int ZombieCurrentHealth
    { 
        get => zombieCurrentHealth;
        set
        { 
            zombieCurrentHealth = value;
            OnZombieHealthChanged?.Invoke();
        }
    }
    public event Action OnZombieHealthChanged;
    void Awake()
    {
        ZombieSource = GetComponent<AudioSource>();
        ZombieAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        OnZombieHealthChanged += HandleDeathCheck;
    }
    private void OnDestroy()
    {
        OnZombieHealthChanged -= HandleDeathCheck;
    }
    public void ZombieCalledOnStart()
    {
        if (isActiveAndEnabled)
        {
            InvokeRepeating("CheckAttackRangeWrapper", 0.1f, 0.1f);
            StartCoroutine(CheckIfStuck());
            Zombie_Agent.autoRepath = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
         
    }
    public void CheckAttackRangeWrapper()
    {
        CheckAttackRange(PlayerController.Instance.PlayerTransform.gameObject);
    }
    public void CheckAttackRange(GameObject Target)
    {
        if (Vector3.Distance(gameObject.transform.position, Target.transform.position) > 1.2f)
        {
            Chase();
        }
        else
        {
            Attack();
        }
    }
    private void Chase()
    {
        Zombie_Agent.SetDestination(PlayerController.Instance.PlayerTransform.transform.position);
        ZombieAnimator.SetBool("Attacking", false);
        ZombieAnimator.SetBool("Walking", true);
    }
    private IEnumerator CheckIfStuck()
    {
        while (gameObject.activeInHierarchy)
        {
            //Set old pos and log the agent details
            NavMeshAgent Agent = GetComponent<NavMeshAgent>();
            Debug.Log($"{Agent.name} - isOnNavMesh: {Agent.isOnNavMesh}, hasPath: {Agent.hasPath}, pathStatus: {Agent.pathStatus}, remainingDistance: {Agent.remainingDistance}");
            Vector3 oldPos = transform.position;
            yield return new WaitForSeconds(3f);
            Vector3 newPos = transform.position;

            float distance = Vector3.Distance(oldPos, newPos);
            

            if (distance < 0.4f)
            {
                Debug.LogWarning("Zombie is stuck � killing.");
                ZombieCurrentHealth = 0;
            }
        }
    }
    protected void Attack()
    {
        if (!HasAtacked)
        { 
            HasAtacked= true;
            if (gameObject.activeSelf)
            {
                StartCoroutine(StartAttack(Attacking.length));
            }
        }
    }
    private IEnumerator StartAttack(float cooldown) 
    {
        Zombie_Agent.SetDestination(PlayerController.Instance.PlayerTransform.transform.position);
        ZombieAnimator.SetBool("Attacking", true);
        ZombieAnimator.SetBool("Walking", false);
        ZombieAnimator.SetBool("Stunned", false);
        yield return new WaitForSeconds(cooldown);
        if (PlayerController.Instance != null && ZombieCurrentHealth > 0)
        {
            PlayZombieAudio(AttackAudio, false);
            PlayerController.Instance.PlayerHealth--;
        }
        HasAtacked = false;
    }
    public void HandleDeathCheck()
    {
        if (ZombieCurrentHealth <= 0)
        {
            Zombie_Agent.isStopped=true;
            StartCoroutine(WaitForAnim(3f));
            ZombieAnimator.SetBool("Dead", true);
            ZombieAnimator.SetBool("Walking", false);
            ZombieAnimator.SetBool("Attacking", false);
            ZombieAnimator.SetBool("Stunned", false);
            //Cancel invokes memory reasons 
            CancelInvoke("CheckAttackRangeWrapper");
            //Stop coroutine  
            StopCoroutine(CheckIfStuck());
            StopCoroutine(StartAttack(Attacking.length));

        }
    }
    private IEnumerator WaitForAnim(float seconds)
    {
        PlayZombieAudio(DeathAudio, false);
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
        CheckActiveZombies();
    }
    //Not using for zombie in current build
    protected bool SightCheck(Vector3 PlayerPosition, GameObject Zombie, float Range, float ViewCone)
    {
        Vector3 Forward = Zombie.transform.forward;
        Vector3 ToPlayer = (PlayerPosition - Zombie.transform.position).normalized;

        if (Vector3.Dot(Forward, ToPlayer) > ViewCone && Vector3.Distance(PlayerPosition, transform.position) < Range || Vector3.Distance(PlayerPosition, transform.position) < 4)
        {
            Debug.DrawLine(transform.position, ToPlayer, Color.black);
            RaycastHit hit;
            if (Physics.Raycast(Zombie.transform.position, ToPlayer, out hit))
            {
                if (hit.collider.CompareTag("Enviroment"))
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }
        else
        {
            return false;
        }
        return false;
    }
    private void CheckActiveZombies()
    {
        for (int i = 0; i < GameManager.Instance.ZombiePool.Count; i++)
        {
            // check if there are any zombies active in the hierarchy 
            if (GameManager.Instance.ZombiePool[i].activeInHierarchy)
            {
                GameManager.Instance.IsActive = true;
                break;
            }
            else
            {
                // if none are active 
                GameManager.Instance.IsActive = false;
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
        else
        {
            ZombieSource.clip = clip;
            ZombieSource.Play();
            ZombieSource.loop = true; 
        }
    }
}
