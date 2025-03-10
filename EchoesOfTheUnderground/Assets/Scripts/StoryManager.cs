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
    [SerializeField] private MeshRenderer[] AlertIconMesh;
    [SerializeField] private GameObject AlertIcon;
    [SerializeField] public GameObject TutorialCharacter;
    [SerializeField] public int CurrentState;
    [SerializeField] public bool HitMiscItem;
    private Animator animator;
    private NavMeshAgent agent;
   [SerializeField] private GameObject PlaceToMove;
   [SerializeField] private bool StartLoad;
    //Camp Dynamite 
    [SerializeField] private GameObject Leo;
    [SerializeField] private GameObject Megan;
    
    //Streets Part Two 
    [SerializeField] private GameObject NewGun;
    [SerializeField] private GameObject Rubble;
    //Subway 
    public string PressedButtonName;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        AlertIconState(false);
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
                    AlertIconState(true);
                    AlertIcon.transform.position = TutorialCharacter.transform.position + new Vector3(-0.13f, 2, -0.2f);

                    if (Vector3.Distance(TutorialCharacter.transform.position, PlayerController.instance.PlayerTransform.position) < 3 && CurrentState == 0)
                    {
                        SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>(), 0, false);
                        AlertIconState(false);
                    }
                    else if (CurrentState == 1)
                    {
                        animator.SetBool("IsIdle", false);
                        animator.SetBool("IsWalking", true);
                        agent.SetDestination(PlaceToMove.transform.position);
                        AlertIcon.transform.position = agent.transform.position + new Vector3(-0.13f, 2, -0.2f);
                        if (Vector3.Distance(agent.transform.position, PlaceToMove.transform.position) < 0.2f)
                        {
                            animator.SetBool("IsIdle", true);
                            animator.SetBool("IsWalking", false);
                            AlertIconState(true);
                          
                            if (Vector3.Distance(agent.transform.position, PlayerController.instance.PlayerTransform.position) < 2)
                            {
                                AlertIconState(false);
                                SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>(), 1, false);
                            }
                        }
                    }
                    else if (Vector3.Distance(agent.transform.position, PlayerController.instance.PlayerTransform.position) < 2 && CurrentState == 2)
                    {
                        SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>(), 2, false);

                    }
                    else if (WeaponManager.instance.HeldWeapon != null&&WeaponManager.instance.HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag!=null&&CurrentState == 3)
                    {
                        if (WeaponManager.instance.HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag.MaxAmmo > WeaponManager.instance.HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag.BulletNumber)
                        {
                            if (HitMiscItem)
                            {
                                SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>(), 3, false);
                            }
                            else
                            {
                                SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>(), 4, false);
                            }
                        }
                    }
                    else if (CurrentState == 4)
                    {
                        SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>(), 5, false);
                    }
                    else if (CurrentState == 5)
                    {
                        Vector3 LoadZoneVec = new Vector3(10, 2, 2);
                        AlertIcon.transform.position = LoadZoneVec;
                        AlertIconState(true);
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
    private void AlertIconState(bool state)
    {
        if (state)
        {
            for (int i = 0; i < AlertIconMesh.Length; i++)
            {
                AlertIconMesh[i].enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < AlertIconMesh.Length; i++)
            {
                AlertIconMesh[i].enabled = false;
            }
        }
    }
}
