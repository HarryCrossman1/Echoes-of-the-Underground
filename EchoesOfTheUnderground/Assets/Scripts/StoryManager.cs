using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;
    public enum StoryState {Menu,Tutorial,Streets,StreetsPartTwo,Subway }
    public static StoryState State;
    //State for debugging 
    [SerializeField] private StoryState DebugState;

    //Tutorial Stuff
    [SerializeField] public GameObject TutorialCharacter;
    private Transform TutorialCharacterRealTranform;
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
        DebugState= State;
    }
    private void StoryLoop()
    {
        switch (State)
        {
            case StoryState.Tutorial:
                {
                    if (TutorialCharacter == null) break;

                    float distanceToPlayer = Vector3.Distance(TutorialCharacter.transform.position, PlayerController.instance.PlayerTransform.position);

                    CharacterHolder character = TutorialCharacter.GetComponent<CharacterHolder>();
                    AudioSource source = TutorialCharacter.GetComponent<AudioSource>();

                    switch (CurrentState)
                    {
                        case 0:
                            if (distanceToPlayer < 3f && character.ReadyForVoiceline)
                            {
                                SoundManager.instance.PlayVoiceLine(source, character, 0, false);
                            }
                            break;

                        case 1:
                            animator.SetBool("IsIdle", false);
                            animator.SetBool("IsWalking", true);
                            agent.SetDestination(PlaceToMove.transform.position);

                            if (Vector3.Distance(agent.transform.position, PlaceToMove.transform.position) < 0.2f)
                            {
                                animator.SetBool("IsIdle", true);
                                animator.SetBool("IsWalking", false);

                                if (distanceToPlayer < 2f && character.ReadyForVoiceline)
                                {
                                    SoundManager.instance.PlayVoiceLine(source, character, 1, false);
                                }
                            }
                            break;

                        case 2:
                            if (distanceToPlayer < 2f && character.ReadyForVoiceline)
                            {
                                SoundManager.instance.PlayVoiceLine(source, character, 2, false);
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
                                    SoundManager.instance.PlayVoiceLine(source, character, line, false);
                                }
                            }
                            break;

                        case 4:
                            if (character.ReadyForVoiceline)
                            {
                                SoundManager.instance.PlayVoiceLine(source, character, 5, false);
                            }
                            break;

                        case 5:
                            Vector3 loadZoneVec = new Vector3(10, 2, 2);
                            if (Vector3.Distance(PlayerController.instance.PlayerTransform.position, loadZoneVec) < 3)
                            {
                                SavingAndLoading.instance.SaveIngameData(new Vector3(12, 0.1f, 3));
                                CurrentState = 0;
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
                        if (Vector3.Distance(PlayerController.instance.PlayerTransform.position, CampDynamiteLoadTrigger.transform.position) < 2)
                        {
                            CurrentState++;

                                SavingAndLoading.instance.SaveIngameData(new Vector3(149.6411f, -0.03065634f, 40.60464f));
                                SceneManager.LoadScene("CampDynamite");          
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
                                SavingAndLoading.instance.SaveIngameData(new Vector3(160, 1, 44));
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
                        if (Vector3.Distance(PlayerController.instance.PlayerTransform.position, new Vector3(184, 1, 162)) < 3)
                        {
                            SavingAndLoading.instance.SaveIngameData(new Vector3(149.6411f, -0.03065634f, 40.60464f));
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
