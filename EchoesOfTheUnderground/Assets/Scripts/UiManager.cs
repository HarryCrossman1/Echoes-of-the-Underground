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
   [SerializeField] public TextMeshProUGUI DifficultyText,HighScoreText,IngameHighScoreAllTime,IngameCurrentStore;
    [SerializeField] private Slider LoadingSlider;
   [SerializeField] private string[] CampaignDifficultyModes;
    private int DifficultyTracker =1;
    public Canvas DeathCanvas;
    [SerializeField] GameObject MovingText;
    [SerializeField] public TextMeshProUGUI HealthText,PositionText,AmmoText;
    public bool PauseLevelLoading;
    private AsyncOperation Operation;
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
        StartCoroutine(LoadSceneAsync("HomeScene",true));
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void SetGraphics(int Level)
    {
        QualitySettings.SetQualityLevel(Level, true);
    }
    public void IncreaseDifficulty()
    {
        if (DifficultyTracker < CampaignDifficultyModes.Length-1)
        {
            DifficultyTracker++;
            DifficultyText.text = CampaignDifficultyModes[DifficultyTracker];
        }
    }
    public void DecreaseDifficulty() 
    {
        if (DifficultyTracker > 0)
        {
            DifficultyTracker--;
            DifficultyText.text = CampaignDifficultyModes[DifficultyTracker];
        }
    }
    public IEnumerator LoadSceneAsync(string SceneName,bool IsInMenu)
    { 
        Operation = SceneManager.LoadSceneAsync(SceneName);
        Operation.allowSceneActivation = false;
        if (IsInMenu == true)
        {
            Operation.allowSceneActivation = true;
            PlayerPrefs.SetInt("Difficulty", DifficultyTracker);
            while (!Operation.isDone)
            {
                float Progress = Mathf.Clamp01(Operation.progress / 0.9f);
                LoadingSlider.value = Progress;
                yield return null;
            }
            if (Operation.isDone)
            {
                LevelSetter.Instance.SetLevel();
            }
        }
        else
        {
            if (Operation.isDone)
            {
                LevelSetter.Instance.SetLevel();
            }
        }
    }
}
