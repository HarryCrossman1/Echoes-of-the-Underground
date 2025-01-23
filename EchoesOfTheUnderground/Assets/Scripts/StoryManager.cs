using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;
    public enum StoryState {Tutorial,Streets,StreetsPartTwo,Subway }
    public StoryState State;

    //Tutorial Stuff
    [SerializeField] private GameObject AlertIcon;
    [SerializeField] private GameObject TutorialCharacter;
    [SerializeField] public int CurrentState;
    [SerializeField] public bool HitMiscItem;
    private Animator animator;
    private NavMeshAgent agent;
   [SerializeField] private bool StartLoad;
    //Camp Dynamite 
    [SerializeField] private GameObject Leo;
    [SerializeField] private GameObject Megan;
    
    //Streets Part Two 
    [SerializeField] private GameObject NewGun;
    [SerializeField] private GameObject Rubble;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        AlertIcon.GetComponentInChildren<MeshRenderer>().enabled = false;
        animator = TutorialCharacter.GetComponentInChildren<Animator>();
        agent = TutorialCharacter.GetComponentInChildren<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        StoryLoop();
    }
    private void StoryLoop()
    {
        switch (State)
        {
            case StoryState.Tutorial:
                {
                    AlertIcon.GetComponentInChildren<MeshRenderer>().enabled = true;
                    AlertIcon.transform.position = TutorialCharacter.transform.position + new Vector3(-0.13f, 2, -0.2f);

                    if (Vector3.Distance(TutorialCharacter.transform.position, PlayerController.instance.PlayerTransform.position) < 3 && CurrentState == 0)
                    {
                        SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>().character, 0, false);
                        AlertIcon.GetComponentInChildren<MeshRenderer>().enabled = false;
                    }
                    if (CurrentState == 1)
                    {
                        animator.SetBool("IsIdle", false);
                        animator.SetBool("IsWalking", true);
                        agent.SetDestination(new Vector3(-1.775f, 0, 1.825f));
                        if (Vector3.Distance(TutorialCharacter.transform.position, new Vector3(-1.775f, 0, 1.825f)) < 0.1f)
                        {
                            animator.SetBool("IsIdle", true);
                            animator.SetBool("IsWalking", false);
                            AlertIcon.GetComponentInChildren<MeshRenderer>().enabled = true;
                            if (Vector3.Distance(TutorialCharacter.transform.position, PlayerController.instance.PlayerTransform.position) < 3 && CurrentState == 1)
                            {
                                AlertIcon.GetComponentInChildren<MeshRenderer>().enabled = false;
                                SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>().character, 1, false);
                            }
                        }
                    }
                    if (Vector3.Distance(TutorialCharacter.transform.position, PlayerController.instance.PlayerTransform.position) < 3 && CurrentState == 2)
                    {
                        SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>().character, 2, false);

                    }
                    if (CurrentState == 3 && WeaponManager.instance.HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag.MaxAmmo > WeaponManager.instance.HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag.BulletNumber)
                    {
                        if (HitMiscItem)
                        {
                            SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>().character, 3, false);
                        }
                        else
                        {
                            SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>().character, 4, false);
                        }
                    }
                    if (CurrentState == 4)
                    {
                        SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>().character, 5, false);
                    }
                    if (CurrentState == 5)
                    {
                        Vector3 LoadZoneVec = new Vector3(10, 2, 2);
                        AlertIcon.transform.position = LoadZoneVec;
                        AlertIcon.GetComponentInChildren<MeshRenderer>().enabled = true;
                        if (Vector3.Distance(PlayerController.instance.PlayerTransform.position, LoadZoneVec) < 3)
                        {
                            if (StartLoad)
                            {
                                CurrentState = 0;
                                SceneManager.LoadScene("OpenWorldMain");
                                StartLoad = false;
                            }
                        }
                    }
                    break;
                }
                case StoryState.Streets: 
                {
                    if (CurrentState == 0 & Vector3.Distance(PlayerController.instance.PlayerTransform.position, new Vector3(-50, -1, 54)) < 2)
                    {
                        CurrentState++;
                        SceneManager.LoadScene("CampDynamite");
                    }
                    if (CurrentState == 1)
                    { 
                        Leo=GameObject.Find("Leo").GetComponent<GameObject>();
                        if (Vector3.Distance(PlayerController.instance.PlayerTransform.position, Leo.transform.position) < 3)
                        {
                            CurrentState++;   
                        }
                    }
                    if (CurrentState == 2)
                    {
                        if (Vector3.Distance(PlayerController.instance.PlayerTransform.position, new Vector3(-50,-1,52)) < 3)
                        { 
                            CurrentState=0;
                            SceneManager.LoadScene("OpenWorldMain");
                        }
                    }
                        break;
                }
            case StoryState.StreetsPartTwo:
                {
                    if (CurrentState == 0)
                    {
                        Instantiate(NewGun, new Vector3(-49.2f, -0.8f, 53.832f), Quaternion.identity);
                        Rubble.transform.position = new Vector3(28, 0, 0);
                        CurrentState++;
                    }
                    else if (CurrentState == 1) 
                    {
                        if (Vector3.Distance(PlayerController.instance.PlayerTransform.position, new Vector3(184, 1, 162)) < 3)
                        {
                            SceneManager.LoadScene("SubwayScene");
                        }
                    }
                    break;
                }
        }
    }
}
