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
    [SerializeField] public TextMeshProUGUI HealthText,AmmoText;
    public bool PauseLevelLoading;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    // Update is called once per frame
    void Start()
    {
        //HighScoreManager.instance.Load();
        //if(HighScoreText!=null)
        //HighScoreText.text = HighScoreManager.instance.AllTimeHighScore.ToString();
    }
    public void StartCampaign()
    {
        StartCoroutine(LoadSceneAsync("HomeScene",true));
    }
    public void Quit()
    {
        Application.Quit();
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
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);
        operation.allowSceneActivation = false;
        if (IsInMenu == true)
        {
            operation.allowSceneActivation = true;
            PlayerPrefs.SetInt("Difficulty", DifficultyTracker);
            while (!operation.isDone)
            {
                float Progress = Mathf.Clamp01(operation.progress / 0.9f);
                LoadingSlider.value = Progress;
                yield return null;
            }
            if (operation.isDone)
            {
                LevelSetter.Instance.SetLevel();
            }
        }
        else
        {
            if(!PauseLevelLoading)
            {
                operation.allowSceneActivation = true;
                if (operation.isDone)
                {
                    LevelSetter.Instance.SetLevel();
                }
            }
        }
    }
}
