using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

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
        if (StoryManager.instance!=null)
        {
            if (StoryManager.State != StoryManager.StoryState.Menu && MenuPanel!=null)
            {
                if (!MenuIsActive && (LeftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPrimary) && leftPrimary || RightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPrimary) && rightPrimary))
                {
                    MenuPanel.SetActive(true);
                    SoundManager.Instance.PlaySelectSound();
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
