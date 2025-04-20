using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;
    public enum StoryState {Menu,Tutorial,Streets,StreetsPartTwo,Subway }
    public static StoryState State;
    //State for debugging 
    [SerializeField] private StoryState DebugState;
    public static bool LevelSkipped = false;

    //Tutorial Stuff
    public GameObject TutorialCharacter;
    [SerializeField] private GameObject AlertIcon;
    public int CurrentState;
    public bool HitMiscItem;
    public Animator Animator;
    public NavMeshAgent Agent;
    [SerializeField] private GameObject PlaceToMove;
    //Camp Dynamite 
    public GameObject Leo;
    [SerializeField] private GameObject Megan;
    [SerializeField] private GameObject CampDynamiteLoadTrigger;
    
    //Streets Part Two 
    [SerializeField] private GameObject Rubble,NewRubble;
    [SerializeField] private GameObject Dynamite;
    //Subway 
    public string PressedButtonName;
    [SerializeField] private bool SceneLoaded=false;
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

    // Update is called once per frame
    void Update()
    {
        StoryLoop();
        DebugState= State;
    }
    private void StoryLoop()
    {
        switch (State)
        {
            case StoryState.Tutorial:
                {
                    if (TutorialCharacter == null) break;

                    float distanceToPlayer = Vector3.Distance(TutorialCharacter.transform.position, PlayerController.Instance.PlayerTransform.position);

                    CharacterHolder character = TutorialCharacter.GetComponent<CharacterHolder>();
                    AudioSource source = TutorialCharacter.GetComponent<AudioSource>();

                    switch (CurrentState)
                    {
                        case 0:
                            AlertIcon.transform.position = TutorialCharacter.transform.position + new Vector3(0,1.6f,0);
                            if (distanceToPlayer < 3f && character.ReadyForVoiceline)
                            {
                                SoundManager.Instance.PlayVoiceLine(source, character, 0, false);
                            }
                            break;

                        case 1:
                            Animator.SetBool("IsIdle", false);
                            Animator.SetBool("IsWalking", true);
                            Agent.SetDestination(PlaceToMove.transform.position);
                            AlertIcon.transform.position = TutorialCharacter.transform.position + new Vector3(0, 2.3f, 0);
                            UiManager.Instance.TutorialCanvas.GetComponent<Canvas>().enabled = true;
                            if (Vector3.Distance(Agent.transform.position, PlaceToMove.transform.position) < 0.2f)
                            {
                                Animator.SetBool("IsIdle", true);
                                Animator.SetBool("IsWalking", false);

                                if (distanceToPlayer < 2f && character.ReadyForVoiceline)
                                {
                                    SoundManager.Instance.PlayVoiceLine(source, character, 1, false);
                                    AlertIcon.SetActive(false);
                                }
                            }
                            break;

                        case 2:
                            if (distanceToPlayer < 2f && character.ReadyForVoiceline)
                            {
                                SoundManager.Instance.PlayVoiceLine(source, character, 2, false);
                            }
                            break;

                        case 3:
                            var weapon = WeaponManager.instance.HeldWeapon;
                            if (weapon != null && weapon.GetComponentInParent<XrWeaponPickup>().CurrentMag != null)
                            {
                                var mag = weapon.GetComponentInParent<XrWeaponPickup>().CurrentMag;
                                if (mag.MaxAmmo > mag.BulletNumber && character.ReadyForVoiceline)
                                {
                                    int line = HitMiscItem ? 3 : 4;
                                    SoundManager.Instance.PlayVoiceLine(source, character, line, false);
                                }
                            }
                            break;

                        case 4:
                            if (character.ReadyForVoiceline)
                            {
                                SoundManager.Instance.PlayVoiceLine(source, character, 5, false);
                            }
                            break;

                        case 5:
                            Vector3 loadZoneVec = new Vector3(10, 2, 2);
                            AlertIcon.transform.position = loadZoneVec;
                            AlertIcon.SetActive(true);
                            if (Vector3.Distance(PlayerController.Instance.PlayerTransform.position, loadZoneVec) < 3)
                            {
                                SavingAndLoading.Instance.SaveIngameData(new Vector3(12, 0.1f, 3));
                                CurrentState = 0;
                                State = StoryState.Streets;
                                SceneManager.LoadScene("OpenWorldMain");
                            }
                            break;
                    }

                    break;
                }
            case StoryState.Streets: 
                {
                    switch (CurrentState)
                    { 
                        case 0: 
                            {
                                if (CampDynamiteLoadTrigger != null && Vector3.Distance(PlayerController.Instance.PlayerTransform.position, CampDynamiteLoadTrigger.transform.position) < 2)
                                {
                                    CurrentState++;

                                    SavingAndLoading.Instance.SaveIngameData(new Vector3(149.6411f, -0.03065634f, 40.60464f));
                                    SceneManager.LoadScene("CampDynamite");
                                }
                                break;
                            }
                        case 1: 
                            {
                                if (Leo != null)
                                {
                                    if (Vector3.Distance(PlayerController.Instance.PlayerTransform.position, Leo.transform.position) < 3)
                                    {
                                        SoundManager.Instance.PlayVoiceLine(Leo.GetComponentInChildren<AudioSource>(), Leo.GetComponentInChildren<CharacterHolder>(), 0, false);
                                    }
                                }
                                break;
                            }
                        case 2: 
                            {
                                if (Vector3.Distance(PlayerController.Instance.PlayerTransform.position, Megan.transform.position) < 2.2f)
                                {
                                    SavingAndLoading.Instance.SaveIngameData(new Vector3(83.357f, 1, 27.313f));
                                    CurrentState = 0;
                                    State = StoryState.StreetsPartTwo;
                                    SceneManager.LoadScene("OpenWorldMain");
                                }
                                break;
                            }
                    }
                    break;
                }
            case StoryState.StreetsPartTwo:
                {
                    switch (CurrentState)
                    { 
                        case 0: 
                            {
                                if (Rubble != null && NewRubble != null)
                                {
                                    Vector3 RubbleOriginalPos = Rubble.transform.position;
                                    Rubble.transform.position = new Vector3(200, 0, 0);
                                    NewRubble.transform.position = RubbleOriginalPos;
                                    CurrentState++;
                                }
                                break;
                            }
                        case 1: 
                            {
                                if (Vector3.Distance(PlayerController.Instance.PlayerTransform.position, new Vector3(55.193f, 1, -56)) < 2)
                                {
                                    SavingAndLoading.Instance.SaveIngameData(new Vector3(-4.317f, 0, -27.59f));
                                    CurrentState = 0;
                                    State = StoryState.Subway;
                                    SceneManager.LoadScene("SubwayScene");
                                }
                                break;
                            }
                    }
                    break;
                }
            case StoryState.Subway: 
                {
                    if (PressedButtonName == "RickCampButton" && !SceneLoaded)
                    {
                        Debug.Log("Loading : " + PressedButtonName);
                        SceneLoaded= true;
                        SceneManager.LoadScene("CampDynamiteRuined");
                    }
                    else if (PressedButtonName == "CampDynamiteButton" && !SceneLoaded)
                    {
                        Debug.Log("Loading : " + PressedButtonName);
                        SceneLoaded = true;
                        SceneManager.LoadScene("HomeSceneRuined");
                    }
                    break;
                }
        }
    }
    public void StoryManagerSetup()
    {
        if (GameObject.Find("Ch35_nonPBR") != null)
        {
            TutorialCharacter = GameObject.Find("Ch35_nonPBR");
            Agent = TutorialCharacter.GetComponentInChildren<NavMeshAgent>();
            Animator = TutorialCharacter.GetComponentInChildren<Animator>();
        }
        if (GameObject.Find("Leo") != null)
        {
            Leo = GameObject.Find("Leo");
        }
        if (GameObject.Find("Megan") != null)
        {
            Megan = GameObject.Find("Megan");
        }
        if (GameObject.Find("CampDynamiteLoadTrigger") != null)
        {
            CampDynamiteLoadTrigger = GameObject.Find("CampDynamiteLoadTrigger");
        }
        if (GameObject.Find("Rubble") != null)
        {
            Rubble = GameObject.Find("Rubble");
        }
        if (GameObject.Find("NewRubble") != null)
        {
            NewRubble = GameObject.Find("NewRubble");
        }
        if (GameObject.Find("Dynamite") != null)
        {
            Dynamite = GameObject.Find("Dynamite");
        }
        if (GameObject.Find("AlertIconParent 1") != null)
        {
            AlertIcon = GameObject.Find("AlertIconParent 1");
        }
        if (GameObject.Find("PlaceToMove") != null)
        {
            PlaceToMove = GameObject.Find("PlaceToMove");
        }
    }
}
