using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class StalkerBehaviour : Zombie_Behaviour
{
    [SerializeField] private NavMeshAgent StalkerAgent;
    public int StalkerCurrentHealth,StalkerHealth;
   // public bool HasAtacked,ZombieInRange;
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
    private void StateMachine()
    {
        switch (CurrentState)
        {
            case BehaviourState.Stalking:
                {
                    if (SightCheck(PlayerController.Instance.transform.position,this.gameObject,ViewRange,ViewCone))
                    {
                        ReachedDestination = true;
                        StalkerAnimator.SetBool("Crawling", false);
                        StalkerAnimator.SetBool("Inspecting", true);
                        StoredPos = PlayerController.Instance.transform.position;
                        CurrentState = BehaviourState.Inspecting;
                        break;
                    }
                    if (ReachedDestination)
                    {
                        EditDetails(0.65f, 11f, 1f);
                        StoredPos = PlayerController.Instance.transform.position + new Vector3(Random.insideUnitSphere.x * CurrentStalkingAccuracy, 0, Random.insideUnitSphere.z * CurrentStalkingAccuracy);
                     
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
                    if (SightCheck(PlayerController.Instance.transform.position,gameObject,ViewRange,ViewCone))
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
                    StalkerAgent.SetDestination(PlayerController.Instance.transform.position);
                    if (Vector3.Distance(PlayerController.Instance.transform.position, transform.position) < 2f)
                    {
                        CurrentState = BehaviourState.Attacking;
                    }
                    break;
                }
            case BehaviourState.Attacking:
                {
                    transform.SetParent(PlayerController.Instance.transform);
                    StalkerAgent.enabled = false;

                    Attack();
                    break;
                }
          //  case BehaviourState.Reset:
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

                  
                   // StalkerAgent.SetDestination(ChooseRandomVec());
                    //if (NavmeshCheck(NewVec))

                    //{
                    //    if (Vector3.Distance(transform.position, StoredPos) < 1f)
                    //    {
                           
                    //        CurrentState = BehaviourState.Stalking;
                    //        StalkerAnimator.SetBool("Sprinting", false);
                    //        StalkerAnimator.SetBool("Crawling", true);
                    //        ReachedDestination = true;
                    //        RunningAway = true;
                    //        CurrentStalkingAccuracy = StalkingAccuracy;
                    //        break;
                    //    }
                    //}
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

    private Vector3 ChooseRandomVec(float RanMaxX,float RanMaxZ)
    {
        float RandomX = Random.Range(0, RanMaxX);
        float RandomZ = Random.Range(0, RanMaxZ);
        return new Vector3(RandomX, 0, RandomZ);
    }
    private void EditDetails(float viewCone, float viewRange, float Speed)
    {
        ViewCone = viewCone;
        ViewRange = viewRange;
        StalkerAgent.speed = Speed;
    }
   
}
