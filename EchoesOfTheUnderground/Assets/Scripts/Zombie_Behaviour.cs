using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie_Behaviour : MonoBehaviour
{
    [SerializeField] private NavMeshAgent Zombie_Agent;
    public Animator ZombieAnimator;
    public int ZombieCurrentHealth,ZombieHealth; //{ get; set; }
    public bool HasAtacked;
   [SerializeField] public bool IsStunned;
    public float AccelMin,AccelMax;
    private bool HasChecked = false;
    private Vector3 OldPosition;

    // Store Animations 
    [SerializeField] private AnimationClip Attacking, Hit, Dead;
    // Zombie Audio
    [SerializeField] private AudioSource ZombieSource;
    [SerializeField] public AudioClip AttackAudio, ShotAudio,DeathAudio,AmbientAudio;
    void Awake()
    {
        ZombieSource = GetComponent<AudioSource>();
        ZombieAnimator = GetComponent<Animator>();
    }
    private void Start()
    {

    }
    public void ZombieCalledOnStart()
    {
        if (isActiveAndEnabled)
        {
            InvokeRepeating("Chase", 0.1f, 0.1f);
            StartCoroutine(CheckIfStuck());
            Zombie_Agent.autoRepath = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        DeathCheck();
        CheckAttackRange(PlayerController.instance.PlayerTransform.gameObject);     
    }
    public void CheckAttackRange(GameObject Target)
    {
        if (Vector3.Distance(gameObject.transform.position, Target.transform.position) > 0.8f)
        {

        }
        else
        {
            Invoke("Attack",Attacking.length);
        }
    }
    private void Chase()
    {
        Zombie_Agent.SetDestination(PlayerController.instance.PlayerTransform.transform.position);
    }
    private IEnumerator CheckIfStuck()
    {
        while (gameObject.activeInHierarchy)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            Debug.Log($"{agent.name} - isOnNavMesh: {agent.isOnNavMesh}, hasPath: {agent.hasPath}, pathStatus: {agent.pathStatus}, remainingDistance: {agent.remainingDistance}");
            Vector3 oldPos = transform.position;
            yield return new WaitForSeconds(3f);
            Vector3 newPos = transform.position;

            float distance = Vector3.Distance(oldPos, newPos);
            

            if (distance < 0.4f)
            {
                Debug.LogWarning("Zombie is stuck — killing.");
                ZombieCurrentHealth = 0;
            }
        }
    }
    protected void Attack()
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
        yield return new WaitForSeconds(cooldown);
        PlayerController.instance.PlayerHealth--;
        PlayerController.instance.PlayerDeathCheck();
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
            //Cancel invokes memory reasons 
            CancelInvoke("Chase");
            CancelInvoke("CheckPosition");
            //Stop coroutine  
            StopCoroutine(CheckIfStuck());

        }
    }
    private IEnumerator WaitForAnim(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
        CheckActiveZombies();
    }
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
