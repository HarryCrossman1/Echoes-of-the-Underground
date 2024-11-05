using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class StalkerBehaviour : MonoBehaviour
{
    public static StalkerBehaviour instance;
    [SerializeField] private NavMeshAgent StalkerAgent;
    public int ZombieCurrentHealth,ZombieHealth;
    public bool HasAtacked,ZombieInRange;
   [SerializeField] public bool IsStunned;
    [SerializeField] private bool ReachedDestination;
    [SerializeField] private bool RunningAway;
    private Vector3 StoredPos;
    [SerializeField] private float StalkingAccuracy;
    [HideInInspector] public float CurrentStalkingAccuracy;
    [SerializeField] private float ViewCone;
    [SerializeField] private float ViewRange;
    private enum BehaviourState {Stalking,Inspecting,Chase,Attacking,Reset }
   [SerializeField] private BehaviourState CurrentState;

    // Store Animations 
    [SerializeField] private AnimationClip Attacking, Hit, Dead;
    private Animator StalkerAnimator;
    private int AniamtionFrames = 110;
    private int ElapsedFrames;
    // Zombie Audio
    [SerializeField] private AudioSource ZombieSource;
    [SerializeField] private AudioClip AttackAudio, ShotAudio,DeathAudio,AmbientAudio;
    void Awake()
    {
        StalkerAgent = GetComponent<NavMeshAgent>();
        ZombieSource = GetComponent<AudioSource>();

        StalkerAnimator = GetComponent<Animator>();
        instance = this;
    }
    private void Start()
    {
        CurrentStalkingAccuracy = StalkingAccuracy;
    }
    // Update is called once per frame
    void Update()
    {
        DeathCheck();
        StateMachine();
    }
    public void Chase(NavMeshAgent ZombieAgent, GameObject Target)
    {
        if (Vector3.Distance(gameObject.transform.position, Target.transform.position) > 2f)
        {
            ZombieInRange = false;
            if (!IsStunned)
            ZombieAgent.destination = Target.transform.position;
        }
        else
        {
            ZombieAgent.destination = gameObject.transform.position;
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
    private void StateMachine()
    {
        switch (CurrentState)
        {
            case BehaviourState.Stalking:
                {
                    if (SightCheck(PlayerController.instance.transform.position,ViewRange))
                    {
                        ReachedDestination = true;
                        StalkerAnimator.SetBool("Crawling", false);
                        StalkerAnimator.SetBool("Inspecting", true);
                        StoredPos = PlayerController.instance.transform.position;
                        CurrentState = BehaviourState.Inspecting;
                        break;
                    }
                    if (ReachedDestination)
                    {
                        EditDetails(0.65f, 11f, 1f);
                        StoredPos = PlayerController.instance.transform.position + new Vector3(Random.insideUnitSphere.x * CurrentStalkingAccuracy, 0, Random.insideUnitSphere.z * CurrentStalkingAccuracy);
                     
                        if (NavmeshCheck(StoredPos))
                        {
                            StalkerAgent.SetDestination(StoredPos);
                            ReachedDestination = false;
                        }
                        break;
                    }
                    if (Vector3.Distance(StoredPos,transform.position)<1f)
                    { 
                        ReachedDestination = true;
                    }
                    break;
                }
            case BehaviourState.Inspecting:
                {
                    EditDetails(0.8f, 4f, 2.5f);
                    if (SightCheck(PlayerController.instance.transform.position,ViewRange))
                    {
                        CurrentState = BehaviourState.Chase;
                    }
                    if (ReachedDestination)
                    {
                        StalkerAgent.SetDestination(StoredPos);
                        ReachedDestination = false;
                        break;
                    }
                    if (Vector3.Distance(StoredPos, transform.position) < 1f)
                    {
                        ReachedDestination = true;
                        StalkerAnimator.SetBool("Crawling", true);
                        StalkerAnimator.SetBool("Inspecting", false);
                        CurrentState = BehaviourState.Stalking;
                    }
                    break;
                }
               
            case BehaviourState.Chase:
                {
                    StalkerAnimator.SetBool("Sprinting", true);
                    StalkerAnimator.SetBool("Inspecting", false);
                    EditDetails(0, 0, 7);
                    StalkerAgent.SetDestination(PlayerController.instance.transform.position);
                    if (Vector3.Distance(PlayerController.instance.transform.position, transform.position) < 2f)
                    {
                        CurrentState = BehaviourState.Attacking;
                    }
                    break;
                }
            case BehaviourState.Attacking:
                {
                    transform.SetParent(PlayerController.instance.transform);
                    StalkerAgent.enabled = false;

                    Attack();
                    break;
                }
            case BehaviourState.Reset:
                {
                    if (RunningAway)
                    {
                        RunningAway = false;
                       // StoredPos = ChooseRandomVec(20, 20);
                        StalkerAgent.SetDestination(StoredPos);
                    }
                    StalkerAgent.enabled = true;
                    // REMEMBER TO ADD THE CORRECT NUMBERS LATER 
                    if (NavmeshCheck(StoredPos))
                    {
                        if (Vector3.Distance(transform.position, StoredPos) < 1f)
                        {
                           
                            CurrentState = BehaviourState.Stalking;
                            StalkerAnimator.SetBool("Sprinting", false);
                            StalkerAnimator.SetBool("Crawling", true);
                            ReachedDestination = true;
                            RunningAway = true;
                            CurrentStalkingAccuracy = StalkingAccuracy;
                            break;
                        }
                    }
                    break;
                }
        }
    }
    private bool NavmeshCheck(Vector3 OriginalPosition)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(OriginalPosition, out hit,0.1f, NavMesh.AllAreas))
        {
            return true;
        }
        else
        {
            return false;
        }
       
    }
    //private Vector3 ChooseRandomVec(float RanMaxX,float RanMaxZ)
    //{
    //    float RandomX = Random.Range(0, RanMaxX);
    //    float RandomZ = Random.Range(0, RanMaxZ);
    //    Vector3 Vec = new Vector3(RandomX, 0, RandomZ);

    //    if (Vector3.Distance(Vec, PlayerController.instance.transform.position) < 12f)
    //    {
    //        return Vec;
    //    }
    //}
    private void EditDetails(float viewCone, float viewRange, float Speed)
    {
        ViewCone = viewCone;
        ViewRange = viewRange;
        StalkerAgent.speed = Speed;
    }
    protected virtual bool SightCheck(Vector3 PlayerPosition,float Range)
    {
        Vector3 Forward = transform.forward;
        Vector3 ToPlayer = (PlayerPosition - transform.position).normalized;

        if (Vector3.Dot(Forward, ToPlayer) > ViewCone && Vector3.Distance(PlayerPosition,transform.position) < Range || Vector3.Distance(PlayerPosition, transform.position) < 2 )
        {
            Debug.DrawLine(transform.position, ToPlayer, Color.black);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, ToPlayer, out hit))
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
    private IEnumerator StartAttack(float cooldown) 
    {
        if (AttackAudio != null)
        {
            PlayZombieAudio(AttackAudio, false);
        }
        StalkerAnimator.SetBool("Sprinting", false);
        StalkerAnimator.SetBool("Attacking", true);
        //PlayerController.instance.PlayerDeathCheck();
        yield return new WaitForSeconds(cooldown);
        StalkerAnimator.SetBool("Sprinting", true);
        StalkerAnimator.SetBool("Attacking", false);
        StalkerAgent.enabled = true;
        PlayerController.instance.PlayerHealth--;
        HighScoreManager.instance.CurrentHighScore -= 100;
        //Could bug If player kills during anim
        HasAtacked = false;
        CurrentState = BehaviourState.Reset;

 
    }
    public void DeathCheck()
    {
        if (ZombieCurrentHealth <= 0)
        {
            PlayZombieAudio(DeathAudio, false);
            StalkerAgent.isStopped=true;
            StartCoroutine(WaitForAnim(3f));

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
            
         
            StartCoroutine(StunTimer(Hit.length));
        }
        
    }
    private IEnumerator StunTimer(float TimerLength)
    {
        StalkerAgent.isStopped = true;
        PlayZombieAudio(ShotAudio,false);
        yield return new WaitForSeconds(TimerLength);
        PlayZombieAudio(AmbientAudio,true);

        StalkerAgent.isStopped = false;
        IsStunned = false;
        if (ZombieInRange)
        {
         
        }
        else
        {
            
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
