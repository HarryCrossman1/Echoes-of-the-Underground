using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    [SerializeField] private Slider LoadingSlider;
    public Canvas DeathCanvas,TutorialCanvas;
    [SerializeField] public TextMeshProUGUI HealthText;
    public bool PauseLevelLoading;
    private AsyncOperation Operation;
    [SerializeField] public Slider MusicSlider, SoundSlider, NpcSlider;
    [SerializeField] public GameObject Panel, Panel1;
    private bool TutorialControls = true;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
    public void StartCampaign()
    {
        StartCoroutine(LoadSceneAsync("HomeScene", true));
    }
    public void Quit()
    {
        Application.Quit();
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
        if (UiManager.instance.SoundSlider != null)
        {
            UiManager.instance.SoundSlider.value = SoundManager.instance.FxVol;
            UiManager.instance.MusicSlider.value = SoundManager.instance.MusicVol;
            UiManager.instance.NpcSlider.value = SoundManager.instance.NpcVol;
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
