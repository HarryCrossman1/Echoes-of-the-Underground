using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSetter : MonoBehaviour
{
    public static LevelSetter Instance;
    private string LevelName;
    public Vector3 PlayerNextSpawnLocation;
   [SerializeField] private GameObject SoundManagerPrefab;
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
    public void SetLevel()
    {
        LevelName = SceneManager.GetActiveScene().name;
        // Across all scenes 
       
        switch (LevelName)
        {
            case "MenuScene":
                {
                    //Setup Sound
                    if (SoundManager.instance != null)
                    {
                        // Load the settings if they exist 
                        SavingAndLoading.instance.LoadSettings();
                        //Set the sliders for astethic purposes, actual volume set in the loadsettings()
                        SoundManager.instance.SoundSlider.value = SoundManager.instance.FxVol;
                        SoundManager.instance.MusicSlider.value = SoundManager.instance.MusicVol;
                        SoundManager.instance.NpcSlider.value = SoundManager.instance.NpcVol;
                        // Get and set the audiosources ingame
                        SoundManager.instance.GetAudioSources();
                        SoundManager.instance.SetAudioSources();
                    }
                    break;
                }
            case "HomeScene":
                {
                    //Add Saving and loading Manager 
                    if (SavingAndLoading.instance == null)
                    { 
                        GameObject obj = new GameObject("Saving And Loading");
                        obj.AddComponent<SavingAndLoading>();
                    }
                    //Setup Sound no need for null check 
                    if (SoundManager.instance != null)
                    {
                        SoundManager.instance.GetAudioSources();
                        SoundManager.instance.SetAudioSources();
                    }
                    else
                    { 
                        Instantiate(SoundManagerPrefab);
                        SoundManager.instance.GetAudioSources();
                        SoundManager.instance.SetAudioSources();
                    }
                    if (GameObject.Find("Pistol") != null)
                    {
                        SoundManager.instance.GunSource = GameObject.Find("Pistol").GetComponent<AudioSource>();

                    }
                    if (GameObject.Find("Watch") != null)
                    {
                        SoundManager.instance.WatchSource = GameObject.Find("Watch").GetComponent<AudioSource>();
                    }
                    break;
                }
            case "OpenWorldMain":
                {
                    SoundManager.instance.GetAudioSources();
                    
                    if (GameObject.Find("Pistol") != null)
                    {
                        SoundManager.instance.GunSource = GameObject.Find("Pistol").GetComponent<AudioSource>();
                        Debug.Log("CodeReached");
                    }
                    break;
                }
            case "CampDynamite":
                {
                    

                    break;
                }
            case "SubwayScene":
                {
                    
                    break;
                }
        }
    }
}
