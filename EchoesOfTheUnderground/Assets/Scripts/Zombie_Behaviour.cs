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
    public bool HasAtacked;
   [SerializeField] public bool IsStunned;
    public float AccelMin,AccelMax;
    private bool HasChecked = false;
    private Vector3 OldPosition;

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
    }
    private void Start()
    {
        ZombieCalledOnStart();
    }
    public void ZombieCalledOnStart()
    {
        InvokeRepeating("Chase", 1, 0.1f);
        InvokeRepeating("CheckPosition", 5, 3);
        Zombie_Agent.autoRepath = true;
    }
    // Update is called once per frame
    void Update()
    {
        DeathCheck();
        CheckAttackRange(PlayerController.instance.PlayerTransform.gameObject);     
    }
    public void CheckAttackRange(GameObject Target)
    {
        if (Vector3.Distance(gameObject.transform.position, Target.transform.position) > 0.5f)
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
    private void CheckPosition()
    {
        if (!HasChecked)
        {
            OldPosition = transform.position;
            HasChecked = true;
            
            return;
        }
        else
        {
            if (Vector3.Distance(OldPosition, transform.position) < 1)
            {
                Debug.Log("OldPosition = "+ OldPosition + "New Position =" + transform.position);
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
    protected void PlayZombieAudio(AudioClip clip,bool loop)
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
