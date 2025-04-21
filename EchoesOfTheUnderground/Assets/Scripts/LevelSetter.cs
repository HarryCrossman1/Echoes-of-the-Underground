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
                    SoundManager.Instance.SoundManagerSetup();
                    UiManager.Instance.UiManagerSetup();
                    StoryManager.Instance.StoryManagerSetup();
                    GameManager.Instance.GameManagerSetup();
                    GameManager.Instance.HasZombies= false;
                    StoryManager.State = StoryManager.StoryState.Menu;
                    StoryManager.Instance.CurrentState = 0;
                    break;
                }
            case "HomeScene":
                {
                    SoundManager.Instance.SoundManagerSetup();
                    UiManager.Instance.UiManagerSetup();
                    StoryManager.Instance.StoryManagerSetup();
                    StoryManager.State = StoryManager.StoryState.Tutorial;
                    break;
                }
            case "OpenWorldMain":
                {
                    SoundManager.Instance.SoundManagerSetup();
                    UiManager.Instance.UiManagerSetup();
                    StoryManager.Instance.StoryManagerSetup();
                    GameManager.Instance.GameManagerSetup();
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
                        StoryManager.LevelSkipped = false;
                    }
                    else
                    {
                        LevelSkipLogic(new Vector3(83.357f, 1, 27.313f));
                        StoryManager.LevelSkipped = false;
                    }

                    break;
                }
            case "CampDynamite":
                {
                    SoundManager.Instance.SoundManagerSetup();
                    UiManager.Instance.UiManagerSetup();
                    StoryManager.Instance.StoryManagerSetup();
                    GameManager.Instance.GameManagerSetup();
                    UiManager.InCampDynamite = true;
                    GameManager.Instance.HasZombies= false;
                    GameManager.Instance.IsActive = false;
                    GameManager.Instance.Init();
                    LevelSkipLogic(new Vector3(149.6411f, -0.03065634f, 40.60464f));
                    StoryManager.LevelSkipped = false;
                    break;
                }
            case "SubwayScene":
                {
                    SoundManager.Instance.SoundManagerSetup();
                    UiManager.Instance.UiManagerSetup();
                    StoryManager.Instance.StoryManagerSetup();
                    GameManager.Instance.GameManagerSetup();
                    GameManager.FixedSpawns= true;
                    GameManager.Instance.HasZombies = true;
                    GameManager.Instance.IsActive = false;
                    GameManager.Instance.WaitingForZombies = true;
                    GameManager.Instance.Init();
                    LevelSkipLogic(new Vector3(-4.317f, 0, -27.59f));
                    StoryManager.LevelSkipped = false;
                    break;
                }
            case "CampDynamiteRuined":
                {
                    SoundManager.Instance.SoundManagerSetup();
                    GameManager.Instance.HasZombies = false;
                    GameManager.Instance.IsActive = true;
                    if (!CoroutineStarted)
                    {
                        StartCoroutine(EndGameDelayed(8));
                        CoroutineStarted = true;
                    }
                    break;
                }
            case "HomeSceneRuined":
                {
                    SoundManager.Instance.SoundManagerSetup();
                    GameManager.Instance.HasZombies = false;
                    GameManager.Instance.IsActive = true;
                    if (!CoroutineStarted)
                    {
                        StartCoroutine(EndGameDelayed(8));
                        CoroutineStarted= true;
                    }
                    break;
                }
        }
    }
    private IEnumerator EndGameDelayed(int Seconds)
    {
        yield return new WaitForSeconds(Seconds);
        Application.Quit();
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
                PlayerController.Instance.MagLocations[0].StartManualInteraction(Mag1.GetComponent<XRGrabInteractable>());
                PlayerController.Instance.MagLocations[1].StartManualInteraction(Mag2.GetComponent<XRGrabInteractable>());
                Debug.Log(Mag1.gameObject + "+" + PlayerController.Instance.MagLocations[0]);
                Debug.Log("Level Has Been Skipped");
            }
            PlayerController.Instance.PlayerTransform.position = NewPos;
        }
    }
}
