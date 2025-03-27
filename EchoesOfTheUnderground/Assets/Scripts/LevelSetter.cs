using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.AI;

public class LevelSetter : MonoBehaviour
{
    public static LevelSetter Instance;
    private string LevelName;
   [SerializeField] private GameObject SoundManagerPrefab,SavingAndLoadingPrefab,UiManagerPrefab,StoryManagerPrefab,GameManagerPrefab;
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
                    StoryManager.instance.State = StoryManager.StoryState.Tutorial;
                    break;
                }
            case "OpenWorldMain":
                {
                   
                    StoryManager.instance.State = StoryManager.StoryState.Streets;
                    SoundManagerSetup();
                    UiManagerSetup();
                    StoryManagerSetup();
                    SavingAndLoading.instance.LoadIngameData();
                    GameManager.instance.HasZombies= true;
                    GameManager.instance.IsActive= false;
                    Debug.Log(GameManager.instance.HasZombies);
                    GameManager.instance.Init();
                    break;
                }
            case "CampDynamite":
                {
                    InstatiateCoreManagers();
                    GameManager.instance.HasZombies= false;
                    break;
                }
            case "SubwayScene":
                {
                    
                    break;
                }
        }
    }
    private void InstatiateCoreManagers()
    {
        if (UiManager.instance == null)
        {
            Instantiate(UiManagerPrefab);
        }
        if (SoundManager.instance == null)
        {
            Instantiate(SoundManagerPrefab);
        }
        if (GameManager.instance == null)
        {
            Instantiate(GameManagerPrefab);
        }
        if (StoryManager.instance == null)
        {
            Instantiate(StoryManagerPrefab);
        }
        if (SavingAndLoading.instance == null)
        {
            Instantiate(SavingAndLoadingPrefab);
        }
    }
    private void UiManagerSetup()
    {
        if (GameObject.Find("DeathCanvas")!=null)
        { 
            UiManager.instance.DeathCanvas = GameObject.Find("DeathCanvas").GetComponent<Canvas>();
        }
        if (GameObject.Find("Health") != null)
        {
            UiManager.instance.HealthText = GameObject.Find("Health").GetComponent<TextMeshProUGUI>();
            if (UiManager.instance.HealthText != null)
            {
                StartCoroutine(SetHealthTextOnStart());
            }
        }
    }
    private IEnumerator SetHealthTextOnStart()
    {
        yield return new WaitUntil(() => PlayerController.instance != null);
        UiManager.instance.HealthText.text = PlayerController.instance.PlayerHealth.ToString();
    }
    private void SoundManagerSetup()
    {
        // Get the sources
        if (GameObject.Find("Pistol") != null)
        {
            SoundManager.instance.GunSource = GameObject.Find("Pistol").GetComponent<AudioSource>();
        }
        if (GameObject.Find("Watch") != null)
        {
            SoundManager.instance.WatchSource = GameObject.Find("Watch").GetComponent<AudioSource>();
        }
        if (GameObject.Find("GenericSource") != null)
        {
            SoundManager.instance.GenericSource = GameObject.Find("GenericSource").GetComponent<AudioSource>();
       
        }
        if (GameObject.Find("AmbientSource") != null)
        {
            SoundManager.instance.AmbienceSource = GameObject.Find("AmbientSource").GetComponent<AudioSource>();
        }
        // Load the settings if they exist 
        SavingAndLoading.instance.LoadSettings();

        //Set the sliders for astethic purposes, actual volume set in the loadsettings()
        UiManager.instance.SetSoundSlider();
        // Get and set the audiosources ingame
        SoundManager.instance.GetAudioSources();
        SoundManager.instance.SetAudioSources();
    }
    private void StoryManagerSetup()
    {
        if (GameObject.Find("RickTutorialCharacter") != null)
        {
            StoryManager.instance.TutorialCharacter = GameObject.Find("RickTutorialCharacter");
            StoryManager.instance.agent = StoryManager.instance.TutorialCharacter.GetComponentInChildren<NavMeshAgent>();
            StoryManager.instance.animator = StoryManager.instance.TutorialCharacter.GetComponentInChildren<Animator>();
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
        if (GameObject.Find("Dynamite") != null)
        {
            StoryManager.instance.Dynamite = GameObject.Find("Dynamite");
        }
        if (GameObject.Find("Dynamite") != null)
        {
            StoryManager.instance.Dynamite = GameObject.Find("Dynamite");
        }
        if (GameObject.Find("Dynamite") != null)
        {
            StoryManager.instance.Dynamite = GameObject.Find("Dynamite");
        }
        if (GameObject.Find("PlaceToMove") != null)
        {
            StoryManager.instance.PlaceToMove = GameObject.Find("PlaceToMove");
        }
    }
}
