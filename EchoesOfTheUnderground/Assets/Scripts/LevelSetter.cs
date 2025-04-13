using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;

public class LevelSetter : MonoBehaviour
{
    public static LevelSetter Instance;
    private string LevelName;
   [SerializeField] private GameObject SoundManagerPrefab,SavingAndLoadingPrefab,UiManagerPrefab,StoryManagerPrefab,GameManagerPrefab;
    private bool CoroutineStarted = false;
    // Start is called before the first frame update
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
        SetLevel();
    }
    public void Start()
    {

    }
    public void SetLevel()
    {
        LevelName = SceneManager.GetActiveScene().name;
        // Across all scenes 
       
        switch (LevelName)
        {
            case "MenuScene":
                {
                    
                    break;
                }
            case "HomeScene":
                {
                    SoundManagerSetup();
                    UiManagerSetup();
                    StoryManagerSetup();
                    StoryManager.State = StoryManager.StoryState.Tutorial;
                    break;
                }
            case "OpenWorldMain":
                {
                    if (StoryManager.State == StoryManager.StoryState.Streets)
                    {
                        GameManager.Instance.IsActive = false;
                    }
                    else
                    {
                        GameManager.Instance.IsActive = false;
                    }
                    
                    SoundManagerSetup();
                    UiManagerSetup();
                    StoryManagerSetup();
                    GameManagerSetup();
                    SavingAndLoading.Instance.LoadIngameData();
                    GameManager.Instance.HasZombies= true;
                    
                    Debug.Log(GameManager.Instance.HasZombies);
                    GameManager.Instance.Init();
                    break;
                }
            case "CampDynamite":
                {
                    SoundManagerSetup();
                    UiManagerSetup();
                    StoryManagerSetup();
                    GameManager.Instance.HasZombies= false;
                    GameManager.Instance.IsActive = true;
                    break;
                }
            case "SubwayScene":
                {
                    SoundManagerSetup();
                    UiManagerSetup();
                    StoryManagerSetup();
                    GameManager.Instance.HasZombies = true;
                    GameManager.Instance.IsActive = false;
                    break;
                }
            case "CampDynamiteRunied":
                {
                    SoundManagerSetup();
                    GameManager.Instance.HasZombies = false;
                    GameManager.Instance.IsActive = true;
                    if (!CoroutineStarted)
                    {
                        StartCoroutine(EndGame());
                        CoroutineStarted = true;
                    }
                    break;
                }
            case "HomeSceneRuined":
                {
                    SoundManagerSetup();
                    GameManager.Instance.HasZombies = false;
                    GameManager.Instance.IsActive = true;
                    if (!CoroutineStarted)
                    {
                        StartCoroutine(EndGame());
                        CoroutineStarted= true;
                    }
                    break;
                }
        }
    }
    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(8);
        Application.Quit();
    }
    private void InstatiateCoreManagers()
    {
        if (UiManager.Instance == null)
        {
            Instantiate(UiManagerPrefab);
        }
        if (SoundManager.Instance == null)
        {
            Instantiate(SoundManagerPrefab);
        }
        if (GameManager.Instance == null)
        {
            Instantiate(GameManagerPrefab);
        }
        if (StoryManager.instance == null)
        {
            Instantiate(StoryManagerPrefab);
        }
        if (SavingAndLoading.Instance == null)
        {
            Instantiate(SavingAndLoadingPrefab);
        }
    }
    private void UiManagerSetup()
    {
        if (GameObject.Find("DeathCanvas")!=null)
        { 
            UiManager.Instance.DeathCanvas = GameObject.Find("DeathCanvas").GetComponent<Canvas>();
        }
        if (GameObject.Find("TutorialCanvas") != null)
        {
            UiManager.Instance.TutorialCanvas = GameObject.Find("TutorialCanvas").GetComponent<Canvas>();
        }
        if (GameObject.Find("Health") != null)
        {
            UiManager.Instance.HealthText = GameObject.Find("Health").GetComponent<TextMeshProUGUI>();
            if (UiManager.Instance.HealthText != null)
            {
                StartCoroutine(SetHealthTextOnStart());
            }
        }
        if (GameObject.Find("Panel") != null)
        {
            UiManager.Instance.Panel = GameObject.Find("Panel");
        }
        if (GameObject.Find("Panel (1)") != null)
        {
            UiManager.Instance.Panel1 = GameObject.Find("Panel (1)");
        }
        UiManager.Instance.InvokeRepeating("SwitchPanel", 0, 15.1f);
    }
    private IEnumerator SetHealthTextOnStart()
    {
        yield return new WaitUntil(() => PlayerController.Instance != null);
        UiManager.Instance.HealthText.text = PlayerController.Instance.PlayerHealth.ToString();
    }
    private void SoundManagerSetup()
    {
        // Get the sources
        if (GameObject.Find("Pistol") != null)
        {
            SoundManager.Instance.GunSource = GameObject.Find("Pistol").GetComponent<AudioSource>();
        }
        if (GameObject.Find("Watch") != null)
        {
            SoundManager.Instance.WatchSource = GameObject.Find("Watch").GetComponent<AudioSource>();
        }
        if (GameObject.Find("GenericSource") != null)
        {
            SoundManager.Instance.GenericSource = GameObject.Find("GenericSource").GetComponent<AudioSource>();
       
        }
        if (GameObject.Find("AmbientSource") != null)
        {
            SoundManager.Instance.AmbienceSource = GameObject.Find("AmbientSource").GetComponent<AudioSource>();
        }
        // Load the settings if they exist 
        if(SavingAndLoading.Instance!=null)
        SavingAndLoading.Instance.LoadSettings();

        //Set the sliders for astethic purposes, actual volume set in the loadsettings()
        if(UiManager.Instance!=null)
        UiManager.Instance.SetSoundSlider();
        // Get and set the audiosources ingame
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.GetAudioSources();
            SoundManager.Instance.SetAudioSources();
        }
    }
    private void StoryManagerSetup()
    {
        if (GameObject.Find("Ch35_nonPBR") != null)
        {
            StoryManager.instance.TutorialCharacter = GameObject.Find("Ch35_nonPBR");
            StoryManager.instance.Agent = StoryManager.instance.TutorialCharacter.GetComponentInChildren<NavMeshAgent>();
            StoryManager.instance.Animator = StoryManager.instance.TutorialCharacter.GetComponentInChildren<Animator>();
        }
        if (GameObject.Find("Leo") != null)
        {
            StoryManager.instance.Leo = GameObject.Find("Leo");
        }
        if (GameObject.Find("Megan") != null)
        {
            StoryManager.instance.Megan = GameObject.Find("Megan");
        }
        if (GameObject.Find("CampDynamiteLoadTrigger") != null)
        {
            StoryManager.instance.CampDynamiteLoadTrigger = GameObject.Find("CampDynamiteLoadTrigger");
        }
        if (GameObject.Find("Rubble") != null)
        {
            StoryManager.instance.Rubble = GameObject.Find("Rubble");
        }
        if (GameObject.Find("NewRubble") != null)
        {
            StoryManager.instance.NewRubble = GameObject.Find("NewRubble");
        }
        if (GameObject.Find("Dynamite") != null)
        {
            StoryManager.instance.Dynamite = GameObject.Find("Dynamite");
        }
        if (GameObject.Find("AlertIconParent 1") != null)
        {
            StoryManager.instance.AlertIcon = GameObject.Find("AlertIconParent 1");
        }
        if (GameObject.Find("PlaceToMove") != null)
        {
            StoryManager.instance.PlaceToMove = GameObject.Find("PlaceToMove");
        }
    }
    private void GameManagerSetup()
    {
        if (GameObject.Find("SpawnPoint1") != null)
        {
            GameManager.Instance.FixedSpawnsLocations[0] = GameObject.Find("SpawnPoint1");
        }
        if (GameObject.Find("SpawnPoint1 (1)") != null)
        {
            GameManager.Instance.FixedSpawnsLocations[1] = GameObject.Find("SpawnPoint1 (1)");
        }
    }
}
