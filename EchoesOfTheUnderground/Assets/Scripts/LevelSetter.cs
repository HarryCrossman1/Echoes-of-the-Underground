using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;

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
    }

    [System.Obsolete]
    public void Start()
    {
        SetLevel();
    }

    [System.Obsolete]
    public void SetLevel()
    {
        LevelName = SceneManager.GetActiveScene().name;
        // Across all scenes 
       
        switch (LevelName)
        {
            case "MenuScene":
                {
                    SoundManagerSetup();
                    UiManagerSetup();
                    StoryManagerSetup();
                    GameManagerSetup();
                    GameManager.Instance.HasZombies= false;
                    StoryManager.State = StoryManager.StoryState.Menu;
                    StoryManager.Instance.CurrentState = 0;
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
                    SoundManagerSetup();
                    UiManagerSetup();
                    StoryManagerSetup();
                    GameManagerSetup();
                    //SavingAndLoading.Instance.LoadIngameData();
                    GameManager.FixedSpawns = false;
                    GameManager.Instance.IsActive = false;
                    GameManager.Instance.HasZombies= true;
                    GameManager.Instance.WaitingForZombies = true;
                    GameManager.Instance.Init();
                    Debug.Log("Gamemanger Fixed Spawns" + GameManager.FixedSpawns);
                    Debug.Log("Gamemanger Is active" + GameManager.Instance.IsActive);
                    Debug.Log("Gamemanger HasZombies" + GameManager.Instance.HasZombies);
                    if (StoryManager.State == StoryManager.StoryState.Streets)
                    {
                        LevelSkipLogic(new Vector3(12, 0.1f, 3));
                    }
                    else
                    {
                        LevelSkipLogic(new Vector3(83.357f, 1, 27.313f));
                    }

                    break;
                }
            case "CampDynamite":
                {
                    SoundManagerSetup();
                    UiManagerSetup();
                    StoryManagerSetup();
                    GameManagerSetup();
                    UiManager.InCampDynamite = true;
                    GameManager.Instance.HasZombies= false;
                    GameManager.Instance.IsActive = false;
                    GameManager.Instance.Init();
                    LevelSkipLogic(new Vector3(149.6411f, -0.03065634f, 40.60464f));

                    break;
                }
            case "SubwayScene":
                {
                    SoundManagerSetup();
                    UiManagerSetup();
                    StoryManagerSetup();
                    GameManagerSetup();
                    GameManager.FixedSpawns= true;
                    GameManager.Instance.HasZombies = true;
                    GameManager.Instance.IsActive = false;
                    GameManager.Instance.WaitingForZombies = true;
                    GameManager.Instance.Init();
                    LevelSkipLogic(new Vector3(-4.317f, 0, -27.59f));
                    break;
                }
            case "CampDynamiteRuined":
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
        if (StoryManager.Instance == null)
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
        //Menu loading slider 
        if (GameObject.Find("LoadingBar") != null)
        {
            UiManager.Instance.LoadingSlider = GameObject.Find("LoadingBar").GetComponent<Slider>();
        }
        // Sound Sliders
        if (GameObject.Find("Music Vol") != null)
        {
            UiManager.Instance.MusicSlider = GameObject.Find("Music Vol").GetComponentInChildren<Slider>();
        }
        if (GameObject.Find("Music Vol (1)") != null)
        {
            UiManager.Instance.SoundSlider = GameObject.Find("Music Vol (1)").GetComponentInChildren<Slider>();
        }
        if (GameObject.Find("Music Vol (2)") != null)
        {
            UiManager.Instance.NpcSlider = GameObject.Find("Music Vol (2)").GetComponentInChildren<Slider>();
        }
        //Pannels
        if (GameObject.Find("Panel") != null)
        {
            UiManager.Instance.Panel = GameObject.Find("Panel");
        }
        if (GameObject.Find("Panel (1)") != null)
        {
            UiManager.Instance.Panel1 = GameObject.Find("Panel (1)");
        }
        if (GameObject.Find("SettingsPanelInGame") != null)
        {
            UiManager.Instance.MenuPanel = GameObject.Find("SettingsPanelInGame");
        }
        if (GameObject.Find("AudioQualityInGame") != null)
        {
            UiManager.Instance.AudioPanel = GameObject.Find("AudioQualityInGame");
        if (GameObject.Find("GraphicsQualityInGame") != null)
        {
            UiManager.Instance.GraphicsPanel = GameObject.Find("GraphicsQualityInGame");
        }
            // Text
            if (GameObject.Find("MagSlots") != null)
            {
                UiManager.Instance.MagText = GameObject.Find("MagSlots").GetComponent<TextMeshProUGUI>();
            }
            if (GameObject.Find("Holster") != null)
            {
                UiManager.Instance.HolsterText = GameObject.Find("Holster").GetComponent<TextMeshProUGUI>();
            }
            if (UiManager.Instance.TextIsEnabled)
            {
                UiManager.Instance.MagText.enabled = true;
                UiManager.Instance.HolsterText.enabled = true;
                UiManager.Instance.TextIsEnabled= true; 
            }
            else
            {
                UiManager.Instance.MagText.enabled = false;
                UiManager.Instance.HolsterText.enabled = false;
                UiManager.Instance.TextIsEnabled = false;
            }
            //Buttons 
            if (GameObject.Find("Back") != null)
        {
            UiManager.Instance.Back = GameObject.Find("Back").GetComponent<Button>();
            UiManager.Instance.Back.onClick.AddListener(SoundManager.Instance.PlaySelectSound);
            // Add saving and loading of sound 
            UiManager.Instance.Back.onClick.AddListener(SavingAndLoading.Instance.SaveSettings);
            UiManager.Instance.Back.onClick.AddListener(SoundManager.Instance.GetAudioSources);
            UiManager.Instance.Back.onClick.AddListener(SoundManager.Instance.SetAudioSources);
        }
        if (GameObject.Find("Exit") != null)
        {
            UiManager.Instance.Exit = GameObject.Find("Exit").GetComponent<Button>();
            UiManager.Instance.Exit.onClick.AddListener(UiManager.Instance.Quit);
        }
        if (GameObject.Find("Skip") != null)
        {
            UiManager.Instance.Skip = GameObject.Find("Skip").GetComponent<Button>();
            UiManager.Instance.Skip.onClick.AddListener(UiManager.Instance.SkipLevel);
            UiManager.Instance.Skip.onClick.AddListener(SoundManager.Instance.PlaySelectSound);
        }
            if (GameObject.Find("EnableUI") != null)
            {
                UiManager.Instance.Skip = GameObject.Find("EnableUI").GetComponent<Button>();
                UiManager.Instance.Skip.onClick.AddListener(UiManager.Instance.EnableUI);
                UiManager.Instance.Skip.onClick.AddListener(SoundManager.Instance.PlaySelectSound);
            }
            if (GameObject.Find("Performance") != null)
        {
            UiManager.Instance.Performance = GameObject.Find("Performance").GetComponent<Button>();
            UiManager.Instance.Performance.onClick.AddListener (() => UiManager.Instance.SetGraphics(0));
            UiManager.Instance.Performance.onClick.AddListener(SoundManager.Instance.PlaySelectSound);
        }
        if (GameObject.Find("Balanced") != null)
        {
            UiManager.Instance.Balanced = GameObject.Find("Balanced").GetComponent<Button>();
            UiManager.Instance.Balanced.onClick.AddListener(() => UiManager.Instance.SetGraphics(1));
            UiManager.Instance.Balanced.onClick.AddListener(SoundManager.Instance.PlaySelectSound);
        }
        if (GameObject.Find("Quality") != null)
        {
            UiManager.Instance.Quality = GameObject.Find("Quality").GetComponent<Button>();
            UiManager.Instance.Quality.onClick.AddListener(() => UiManager.Instance.SetGraphics(2));
            UiManager.Instance.Quality.onClick.AddListener(SoundManager.Instance.PlaySelectSound);
        }
        UiManager.Instance.InvokeRepeating("SwitchPanel", 0, 15.1f);
        UiManager.Instance.InitializeControllersInGame();
        UiManager.Instance.GraphicsPanel.SetActive(false);
        UiManager.Instance.AudioPanel.SetActive(false);
        UiManager.Instance.MenuPanel.SetActive(false);
        UiManager.Instance.DeathCanvas.GetComponent<Image>().enabled = false;
        UiManager.Instance.DeathCanvas.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        }
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
            StoryManager.Instance.TutorialCharacter = GameObject.Find("Ch35_nonPBR");
            StoryManager.Instance.Agent = StoryManager.Instance.TutorialCharacter.GetComponentInChildren<NavMeshAgent>();
            StoryManager.Instance.Animator = StoryManager.Instance.TutorialCharacter.GetComponentInChildren<Animator>();
        }
        if (GameObject.Find("Leo") != null)
        {
            StoryManager.Instance.Leo = GameObject.Find("Leo");
        }
        if (GameObject.Find("Megan") != null)
        {
            StoryManager.Instance.Megan = GameObject.Find("Megan");
        }
        if (GameObject.Find("CampDynamiteLoadTrigger") != null)
        {
            StoryManager.Instance.CampDynamiteLoadTrigger = GameObject.Find("CampDynamiteLoadTrigger");
        }
        if (GameObject.Find("Rubble") != null)
        {
            StoryManager.Instance.Rubble = GameObject.Find("Rubble");
        }
        if (GameObject.Find("NewRubble") != null)
        {
            StoryManager.Instance.NewRubble = GameObject.Find("NewRubble");
        }
        if (GameObject.Find("Dynamite") != null)
        {
            StoryManager.Instance.Dynamite = GameObject.Find("Dynamite");
        }
        if (GameObject.Find("AlertIconParent 1") != null)
        {
            StoryManager.Instance.AlertIcon = GameObject.Find("AlertIconParent 1");
        }
        if (GameObject.Find("PlaceToMove") != null)
        {
            StoryManager.Instance.PlaceToMove = GameObject.Find("PlaceToMove");
        }
    }
    private void GameManagerSetup()
    {
        if (GameObject.Find("SpawnLocationOne") != null)
        {
            GameManager.Instance.FixedSpawnsLocations[0] = GameObject.Find("SpawnLocationOne");
        }
        if (GameObject.Find("SpawnLocationTwo") != null)
        {
            GameManager.Instance.FixedSpawnsLocations[1] = GameObject.Find("SpawnLocationTwo");
        }
    }

    [System.Obsolete]
    public void LevelSkipLogic(Vector3 NewPos)
    {
        if (StoryManager.LevelSkipped == false)
        {
            SavingAndLoading.Instance.LoadIngameData();
            Debug.Log(StoryManager.LevelSkipped);
        }
        else
        {
            // StoryManager.LevelSkipped = false;
            Debug.Log(StoryManager.LevelSkipped);
            if (SavingAndLoading.Instance != null && PlayerController.Instance != null)
            {
                GameObject Mag1 = Instantiate(SavingAndLoading.Instance.MagPrefab);
                GameObject Mag2 = Instantiate(SavingAndLoading.Instance.MagPrefab);
                PlayerController.Instance.MagLocations[0].startingSelectedInteractable = Mag1.GetComponent<XRGrabInteractable>();
                PlayerController.Instance.MagLocations[1].startingSelectedInteractable = Mag2.GetComponent<XRGrabInteractable>();
                PlayerController.Instance.MagLocations[0].StartManualInteraction( Mag1.GetComponent<XRGrabInteractable>());
                PlayerController.Instance.MagLocations[1].StartManualInteraction(Mag2.GetComponent<XRGrabInteractable>());
                Debug.Log(Mag1.gameObject + "+" + PlayerController.Instance.MagLocations[0]);
                Debug.Log("Level Has Been Skipped");
            }
            PlayerController.Instance.PlayerTransform.position = NewPos;
        }
    }
}
