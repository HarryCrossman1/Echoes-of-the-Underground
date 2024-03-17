using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI DifficultyText;
    [SerializeField] private Slider LoadingSlider;
   [SerializeField] private string[] CampaignDifficultyModes;
    private int DifficultyTracker =1;
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(DifficultyTracker);
    }
    public void StartCampaign()
    {
        StartCoroutine(LoadSceneAsync("SubwayScene"));
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
    private IEnumerator LoadSceneAsync(string SceneName)
    { 
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);

        while (!operation.isDone)
        { 
            float Progress = Mathf.Clamp01(operation.progress/0.9f);
            LoadingSlider.value = Progress;
            yield return null;
        }
    }
}
