using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;
    public enum StoryState {Menu,Tutorial,Streets,StreetsPartTwo,Subway }
    public StoryState State;

    //Tutorial Stuff
    [SerializeField] public GameObject TutorialCharacter;
    [SerializeField] public int CurrentState;
    [SerializeField] public bool HitMiscItem;
    public Animator animator;
    public NavMeshAgent agent;
   [SerializeField] public GameObject PlaceToMove;
   [SerializeField] private bool StartLoad;
    //Camp Dynamite 
    [SerializeField] public GameObject Leo;
    [SerializeField] public GameObject Megan;
    [SerializeField] public GameObject CampDynamiteLoadTrigger;
    
    //Streets Part Two 
    [SerializeField] public GameObject Rubble,NewRubble;
    [SerializeField] public GameObject Dynamite;
    //Subway 
    public string PressedButtonName;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        DontDestroyOnLoad(this);
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
                    if (TutorialCharacter != null)
                    {
                        if (Vector3.Distance(TutorialCharacter.transform.position, PlayerController.instance.PlayerTransform.position) < 3 && CurrentState == 0)
                        {
                            SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>(), 0, false);
                        }

                        else if (CurrentState == 1)
                        {
                            animator.SetBool("IsIdle", false);
                            animator.SetBool("IsWalking", true);
                            agent.SetDestination(PlaceToMove.transform.position);
                            if (Vector3.Distance(agent.transform.position, PlaceToMove.transform.position) < 0.2f)
                            {
                                animator.SetBool("IsIdle", true);
                                animator.SetBool("IsWalking", false);

                                if (Vector3.Distance(agent.transform.position, PlayerController.instance.PlayerTransform.position) < 2)
                                {
                                    SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>(), 1, false);
                                }
                            }
                        }
                        else if (Vector3.Distance(agent.transform.position, PlayerController.instance.PlayerTransform.position) < 2 && CurrentState == 2)
                        {
                            SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>(), 2, false);

                        }
                        else if (WeaponManager.instance.HeldWeapon != null && WeaponManager.instance.HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag != null && CurrentState == 3)
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
                            if (Vector3.Distance(PlayerController.instance.PlayerTransform.position, LoadZoneVec) < 3)
                            {
                                if (StartLoad)
                                {
                                    SavingAndLoading.instance.SaveIngameData(new Vector3(12, 0.1f, 3));
                                    CurrentState = 0;
                                    SceneManager.LoadScene("OpenWorldMain");
                                    StartLoad = false;
                                }
                            }
                        }
                    }
                    break;
                }
                case StoryState.Streets: 
                {
                    if (CurrentState == 0)
                    {
                        if (Vector3.Distance(PlayerController.instance.PlayerTransform.position, CampDynamiteLoadTrigger.transform.position) < 2)
                        {
                            CurrentState++;
                            if (StartLoad)
                            {
                                SavingAndLoading.instance.SaveIngameData(new Vector3(149.6411f, -0.03065634f, 40.60464f));
                                SceneManager.LoadScene("CampDynamite");
                                StartLoad= false;
                            }
                           
                        }
                    }
                    if (CurrentState == 1 && Leo!=null)
                    {
                        if (Vector3.Distance(PlayerController.instance.PlayerTransform.position, Leo.transform.position) < 3)
                        {
                            CurrentState++;
                            SoundManager.instance.PlayVoiceLine(Leo.GetComponent<AudioSource>(), Leo.GetComponent<CharacterHolder>(), 0, false);
                        }
                    }
                    if (CurrentState == 2)
                    {
                        if (Vector3.Distance(PlayerController.instance.PlayerTransform.position, Megan.transform.position) < 3)
                        {
                            if (StartLoad)
                            {
                                SavingAndLoading.instance.SaveIngameData(new Vector3(20.52f, -0.02447182f, -46.21f));
                                CurrentState = 0;
                                State = StoryState.StreetsPartTwo;
                                SceneManager.LoadScene("OpenWorldMain");
                                StartLoad = false;
                            }
                        }
                    }
                        break;
                }
            case StoryState.StreetsPartTwo:
                {
                    if (CurrentState == 0)
                    {
                        Vector3 RubbleOriginalPos = Rubble.transform.position;
                        Rubble.transform.position = new Vector3(28, 0, 0);
                        NewRubble.transform.position = RubbleOriginalPos;
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
