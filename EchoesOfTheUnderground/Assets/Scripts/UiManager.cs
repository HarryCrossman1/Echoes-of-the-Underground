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
using static StoryManager;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    [SerializeField] private Slider LoadingSlider;
    public Canvas DeathCanvas, TutorialCanvas;
    [SerializeField] public TextMeshProUGUI HealthText;
    public bool PauseLevelLoading;
    private AsyncOperation Operation;
    [SerializeField] public Slider MusicSlider, SoundSlider, NpcSlider;
    [SerializeField] public GameObject Panel, Panel1;
    private bool TutorialControls = true;
    //Menu
    private InputDevice LeftController;
    private InputDevice RightController;
    private bool MenuIsActive = false;
    public GameObject MenuPanel,GraphicsPanel,AudioPanel;
    public Button Back, Exit, Skip, Quality, Balanced, Performance;
    public static bool LevelSkipped = false;
    public static bool InCampDynamite = false;
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

    // Update is called once per frame
    void Start()
    {

    }
    private void Update()
    {
        if (StoryManager.Instance !=null)
        {
            if (StoryManager.State != StoryManager.StoryState.Menu && MenuPanel!=null)
            {
                if (!MenuIsActive && (LeftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPrimary) && leftPrimary || RightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPrimary) && rightPrimary))
                {
                    MenuPanel.SetActive(true);
                    SoundManager.Instance.PlaySelectSound();
                    MenuIsActive= true;
                }
                else if (MenuIsActive && (LeftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPrimary1) && leftPrimary1 || RightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPrimary1) && rightPrimary1))
                {
                    MenuPanel.SetActive(false);
                    SoundManager.Instance.PlaySelectSound();
                    MenuIsActive = false;
                }
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
    public void SkipLevel()
    { 
        LevelSkipped= true;
        if (SavingAndLoading.Instance != null && PlayerController.Instance != null)
        {
            GameObject Mag1 = Instantiate(SavingAndLoading.Instance.MagPrefab);
            GameObject Mag2 = Instantiate(SavingAndLoading.Instance.MagPrefab);
            PlayerController.Instance.MagLocations[0].startingSelectedInteractable = Mag1.GetComponent<XRGrabInteractable>();
            PlayerController.Instance.MagLocations[1].startingSelectedInteractable = Mag2.GetComponent<XRGrabInteractable>();
        }
        if (StoryManager.Instance != null)
        {
            switch (StoryManager.State)
            {
                case StoryState.Tutorial:
                    {
                        SceneManager.LoadScene("OpenWorldMain");
                        StoryManager.State = StoryState.Streets;
                        StoryManager.Instance.CurrentState = 0;
                        break;
                    }
                case StoryState.Streets:
                    {
                        if (!InCampDynamite)
                        {
                            SceneManager.LoadScene("CampDynamite");
                            StoryManager.Instance.CurrentState = 1;
                        }
                        else
                        {
                            SceneManager.LoadScene("OpenWorldMain");
                            StoryManager.State = StoryState.StreetsPartTwo;
                            StoryManager.Instance.CurrentState = 0;
                        }

                        break;
                    }
                case StoryState.StreetsPartTwo:
                    {
                        SceneManager.LoadScene("SubwayScene");
                        StoryManager.State = StoryState.Subway;
                        StoryManager.Instance.CurrentState = 0;
                        break;
                    }
                case StoryState.Subway:
                    {
                        ColorBlock cb = Skip.colors;
                        cb.normalColor = Color.red;
                        Skip.colors = cb;
                        break;
                    }
            }
        }
    }
    public void ResetSkippedLevel()
    {
        LevelSkipped = false;
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
}
