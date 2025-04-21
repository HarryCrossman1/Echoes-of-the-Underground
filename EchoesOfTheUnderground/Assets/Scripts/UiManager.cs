using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    [SerializeField] private Slider LoadingSlider;
    public Canvas DeathCanvas, TutorialCanvas;
    public TextMeshProUGUI HealthText;
    private AsyncOperation Operation;
    public Slider MusicSlider, SoundSlider, NpcSlider;
    [SerializeField] private GameObject Panel, Panel1;
    [SerializeField] private bool TutorialControls = true;
    //Menu
    private InputDevice LeftController;
    private InputDevice RightController;
    [SerializeField] private bool MenuIsActive = false;
    [SerializeField] private GameObject MenuPanel,GraphicsPanel,AudioPanel;
    [SerializeField] private Button Back, Exit, Skip, Quality, Balanced, Performance,GraphicsMenu,AudioMenu;
    public static bool InCampDynamite = false;
    //GunHolster Menu
    [SerializeField] private TextMeshProUGUI HolsterText, MagText;
    [SerializeField] private bool TextIsEnabled=true;
    // Editing the button press to make it not work by frame 
    [SerializeField] private bool leftPrimaryPrev = false;
    [SerializeField] private bool rightPrimaryPrev = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (StoryManager.Instance !=null && MenuPanel != null)
        {
            if (StoryManager.State != StoryManager.StoryState.Menu)
            {
                bool leftPrimary = false;
                bool rightPrimary = false;

                LeftController.TryGetFeatureValue(CommonUsages.primaryButton, out leftPrimary);
                RightController.TryGetFeatureValue(CommonUsages.primaryButton, out rightPrimary);

                // Check for "button down" (current true, previous false)
                bool leftPressedThisFrame = leftPrimary && !leftPrimaryPrev;
                bool rightPressedThisFrame = rightPrimary && !rightPrimaryPrev;

                if (!MenuIsActive && (leftPressedThisFrame || rightPressedThisFrame))
                {
                    if (PlayerController.Instance != null)
                    {
                        Time.timeScale= 0f;
                        PlayerController.Instance.LeftHandController.SetActive(false);
                        PlayerController.Instance.RightHandController.SetActive(false);
                        PlayerController.Instance.LeftHandControllerRay.SetActive(true);
                        PlayerController.Instance.RightHandControllerRay.SetActive(true);
                    }
                    Debug.Log("ActivatedMenu");
                    MenuPanel.SetActive(true);
                    SoundManager.Instance.PlaySelectSound();
                    MenuIsActive = true;
                }
                else if (MenuIsActive && (leftPressedThisFrame || rightPressedThisFrame))
                {
                    Time.timeScale = 1f;
                    MenuPanel.SetActive(false);
                    GraphicsPanel.SetActive(false);
                    AudioPanel.SetActive(false);
                    SoundManager.Instance.PlaySelectSound();
                    MenuIsActive = false;
                    if (PlayerController.Instance != null)
                    {
                        PlayerController.Instance.LeftHandController.SetActive(true);
                        PlayerController.Instance.RightHandController.SetActive(true);
                        PlayerController.Instance.LeftHandControllerRay.SetActive(false);
                        PlayerController.Instance.RightHandControllerRay.SetActive(false);
                    }
                }

                // Update previous states
                leftPrimaryPrev = leftPrimary;
                rightPrimaryPrev = rightPrimary;
            }
        }
    }
    public void StartCampaign()
    {
        StartCoroutine(LoadSceneAsync("HomeScene", true));
    }
    public void InitializeControllersInGame()
    {
        var leftDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftDevices);
        if (leftDevices.Count > 0)
            LeftController = leftDevices[0];

        var rightDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightDevices);
        if (rightDevices.Count > 0)
            RightController = rightDevices[0];
    }
    public void CloseMenu()
    { MenuPanel.SetActive(false); }
    public void Quit()
    {
        Application.Quit();
    }
    public IEnumerator SetHealthTextOnStart()
    {
        yield return new WaitUntil(() => PlayerController.Instance != null);
        HealthText.text = PlayerController.Instance.PlayerHealth.ToString();
    }
    public void SkipLevel()
    {
        leftPrimaryPrev = false;
        rightPrimaryPrev = false;
        MenuIsActive = false;
        StoryManager.LevelSkipped = true;
        if (StoryManager.Instance != null)
        {
            switch (StoryManager.State)
            {
                case StoryManager.StoryState.Tutorial:
                    {
                        StoryManager.State = StoryManager.StoryState.Streets;
                        StoryManager.Instance.CurrentState = 0;
                        SceneManager.LoadScene("OpenWorldMain");
                        break;
                    }
                case StoryManager.StoryState.Streets:
                    {
                        if (!InCampDynamite)
                        {
                            StoryManager.Instance.CurrentState = 1;
                            SceneManager.LoadScene("CampDynamite");
                        }
                        else
                        {
                            StoryManager.State = StoryManager.StoryState.StreetsPartTwo;
                            StoryManager.Instance.CurrentState = 0;
                            SceneManager.LoadScene("OpenWorldMain");
                        }

                        break;
                    }
                case StoryManager.StoryState.StreetsPartTwo:
                    {
                        StoryManager.State = StoryManager.StoryState.Subway;
                        StoryManager.Instance.CurrentState = 0;
                        SceneManager.LoadScene("SubwayScene");
                        break;
                    }
                case StoryManager.StoryState.Subway:
                    {
                        ColorBlock cb = Skip.colors;
                        cb.normalColor = Color.red;
                        Skip.colors = cb;
                        break;
                    }
            }
        }
    }

    private void SwitchPanel()
    {
        if (TutorialCanvas != null&&TutorialCanvas.enabled)
        {
            if (TutorialControls)
            {
                Panel.SetActive(true);
                Panel1.SetActive(false);
                TutorialControls= false;
            }
            else
            {
                Panel.SetActive(false);
                Panel1.SetActive(true);
                TutorialControls = true;
            }
        }
    }
    public void SetGraphics(int Level)
    {
        QualitySettings.SetQualityLevel(Level, true);
    }
    public void SetSoundSlider()
    {
        if (SoundSlider != null)
        {
            SoundSlider.value = SoundManager.Instance.FxVol;
            MusicSlider.value = SoundManager.Instance.MusicVol;
            NpcSlider.value = SoundManager.Instance.NpcVol;
        }
    }
    public IEnumerator LoadSceneAsync(string SceneName, bool IsInMenu)
    {
        Operation = SceneManager.LoadSceneAsync(SceneName);
        Operation.allowSceneActivation = false;
        if (IsInMenu == true)
        {
            Operation.allowSceneActivation = true;
            while (!Operation.isDone)
            {
                float Progress = Mathf.Clamp01(Operation.progress / 0.9f);
                LoadingSlider.value = Progress;
                yield return null;
            }
        }
    }
    public void UiManagerSetup()
    {
        if (GameObject.Find("DeathCanvas") != null)
        {
            DeathCanvas = GameObject.Find("DeathCanvas").GetComponent<Canvas>();
        }
        if (GameObject.Find("TutorialCanvas") != null)
        {
            TutorialCanvas = GameObject.Find("TutorialCanvas").GetComponent<Canvas>();
        }
        if (GameObject.Find("Health") != null)
        {
            HealthText = GameObject.Find("Health").GetComponent<TextMeshProUGUI>();
            if (HealthText != null)
            {
                StartCoroutine(SetHealthTextOnStart());
            }
        }
        //Menu loading slider 
        if (GameObject.Find("LoadingBar") != null)
        {
            LoadingSlider = GameObject.Find("LoadingBar").GetComponent<Slider>();
        }
        // Sound Sliders
        if (GameObject.Find("Music Vol") != null)
        {
            MusicSlider = GameObject.Find("Music Vol").GetComponentInChildren<Slider>();
        }
        if (GameObject.Find("Music Vol (1)") != null)
        {
            SoundSlider = GameObject.Find("Music Vol (1)").GetComponentInChildren<Slider>();
        }
        if (GameObject.Find("Music Vol (2)") != null)
        {
            NpcSlider = GameObject.Find("Music Vol (2)").GetComponentInChildren<Slider>();
        }
        //Pannels
        if (GameObject.Find("Panel") != null)
        {
            Panel = GameObject.Find("Panel");
        }
        if (GameObject.Find("Panel (1)") != null)
        {   
            Panel1 = GameObject.Find("Panel (1)");
        }
        if (GameObject.Find("SettingsPanelInGame") != null)
        {
            MenuPanel = GameObject.Find("SettingsPanelInGame");
        }
        if (GameObject.Find("AudioQualityInGame") != null)
        {
            AudioPanel = GameObject.Find("AudioQualityInGame");
            if (GameObject.Find("GraphicsQualityInGame") != null)
            {
                GraphicsPanel = GameObject.Find("GraphicsQualityInGame");
            }
            // Text
            if (GameObject.Find("MagSlots") != null)
            {
                MagText = GameObject.Find("MagSlots").GetComponent<TextMeshProUGUI>();
            }
            if (GameObject.Find("Holster") != null)
            {
                HolsterText = GameObject.Find("Holster").GetComponent<TextMeshProUGUI>();
            }
            if (TextIsEnabled)
            {
                MagText.enabled = true;
                HolsterText.enabled = true;
                TextIsEnabled = true;
            }
            else
            {
                MagText.enabled = false;
                HolsterText.enabled = false;
                TextIsEnabled = false;
            }
            //Buttons 
            if (GameObject.Find("Back") != null)
            {
                Back = GameObject.Find("Back").GetComponent<Button>();
                Back.onClick.AddListener(SoundManager.Instance.PlaySelectSound);
                // Add saving and loading of sound 
                Back.onClick.AddListener(SavingAndLoading.Instance.SaveSettings);
                Back.onClick.AddListener(SoundManager.Instance.GetAudioSources);
                Back.onClick.AddListener(SoundManager.Instance.SetAudioSources);
            }
            if (GameObject.Find("Exit") != null)
            {
                Exit = GameObject.Find("Exit").GetComponent<Button>();
                Exit.onClick.AddListener(Quit);
            }
            if (GameObject.Find("Skip") != null)
            {
                Skip = GameObject.Find("Skip").GetComponent<Button>();
                Skip.onClick.AddListener(SkipLevel);
                Skip.onClick.AddListener(SoundManager.Instance.PlaySelectSound);
            }

            if (GameObject.Find("Performance") != null)
            {
                Performance = GameObject.Find("Performance").GetComponent<Button>();
                Performance.onClick.AddListener(() => SetGraphics(0));
                Performance.onClick.AddListener(SoundManager.Instance.PlaySelectSound);
            }
            if (GameObject.Find("Balanced") != null)
            {
                Balanced = GameObject.Find("Balanced").GetComponent<Button>();
                Balanced.onClick.AddListener(() => SetGraphics(1));
                Balanced.onClick.AddListener(SoundManager.Instance.PlaySelectSound);
            }
            if (GameObject.Find("Quality") != null)
            {
                Quality = GameObject.Find("Quality").GetComponent<Button>();
                Quality.onClick.AddListener(() => SetGraphics(2));
                Quality.onClick.AddListener(SoundManager.Instance.PlaySelectSound);
            }
            if (GameObject.Find("Graphics") != null)
            {
                Quality = GameObject.Find("Graphics").GetComponent<Button>();
                Quality.onClick.AddListener(SoundManager.Instance.PlaySelectSound);
            }
            if (GameObject.Find("Audio Settings") != null)
            {
                Quality = GameObject.Find("Audio Settings").GetComponent<Button>();
                Quality.onClick.AddListener(SoundManager.Instance.PlaySelectSound);
            }
            InvokeRepeating("SwitchPanel", 0, 15.1f);
            InitializeControllersInGame();
            GraphicsPanel.SetActive(false);
            AudioPanel.SetActive(false);
            MenuPanel.SetActive(false);
            DeathCanvas.GetComponent<Image>().enabled = false;
            DeathCanvas.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        }
    }
}
