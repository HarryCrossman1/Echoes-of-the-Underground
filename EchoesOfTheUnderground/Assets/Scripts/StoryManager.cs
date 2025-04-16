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
    [SerializeField] public GameObject TutorialCharacter;
    [SerializeField] public GameObject AlertIcon;
    [SerializeField] public int CurrentState;
    [SerializeField] public bool HitMiscItem;
    public Animator Animator;
    public NavMeshAgent Agent;
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
                    if (CurrentState == 0)
                    {
                        if (CampDynamiteLoadTrigger != null && Vector3.Distance(PlayerController.Instance.PlayerTransform.position, CampDynamiteLoadTrigger.transform.position) < 2)
                        {
                            CurrentState++;

                                SavingAndLoading.Instance.SaveIngameData(new Vector3(83.357f, 1, 27.313f));
                                SceneManager.LoadScene("CampDynamite");          
                        }
                    }
                    if (CurrentState == 1 && Leo!=null)
                    {
                        if (Vector3.Distance(PlayerController.Instance.PlayerTransform.position, Leo.transform.position) < 3)
                        {
                            CurrentState++;
                            SoundManager.Instance.PlayVoiceLine(Leo.GetComponentInChildren<AudioSource>(), Leo.GetComponentInChildren<CharacterHolder>(), 0, false);
                        }
                    }
                    if (CurrentState == 2)
                    {
                        if (Vector3.Distance(PlayerController.Instance.PlayerTransform.position, Megan.transform.position) < 3)
                        {
                                SavingAndLoading.Instance.SaveIngameData(new Vector3(160, 1, 44));
                                CurrentState = 0;
                                State = StoryState.StreetsPartTwo;
                                SceneManager.LoadScene("OpenWorldMain");
                        }
                    }
                        break;
                }
            case StoryState.StreetsPartTwo:
                {
                    if (CurrentState == 0)
                    {
                        Vector3 RubbleOriginalPos = Rubble.transform.position;
                        Rubble.transform.position = new Vector3(200, 0, 0);
                        NewRubble.transform.position = RubbleOriginalPos;
                        CurrentState++;
                    }
                    else if (CurrentState == 1) 
                    {
                        if (Vector3.Distance(PlayerController.Instance.PlayerTransform.position, new Vector3(184, 1, 162)) < 3)
                        {
                            SavingAndLoading.Instance.SaveIngameData(new Vector3(-4.317f, 0, -27.59f));
                            SceneManager.LoadScene("SubwayScene");
                        }
                    }
                    break;
                }
            case StoryState.Subway: 
                {
                    if (PressedButtonName == "RickCampButton")
                    {
                        SceneManager.LoadScene("CampDynamiteRuined");
                    }
                    else if (PressedButtonName == "CampDynamiteButton")
                    {
                        SceneManager.LoadScene("HomeSceneRuined");
                    }
                    break;
                }
        }
    }
}
