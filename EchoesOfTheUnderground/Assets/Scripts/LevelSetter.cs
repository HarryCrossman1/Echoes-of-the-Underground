using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSetter : MonoBehaviour
{
    public static LevelSetter Instance;
    private string LevelName;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance= this;
    }
    void Start()
    {
        SetLevel();
    }
    public void SetLevel()
    {
       
        DontDestroyOnLoad(this);
        LevelName = SceneManager.GetActiveScene().name;
        Debug.Log(LevelName);
        switch (LevelName)
        {
            case "MenuScene":
                {
                    break;
                }
            case "HomeScene":
                {
                    SoundManager.instance.GetAudioSources();
                    SoundManager.instance.AssignVolumeIngame();
                    if (GameObject.Find("Pistol") != null)
                    {
                        SoundManager.instance.GunSource = GameObject.Find("Pistol").GetComponent<AudioSource>();
                        Debug.Log("CodeReached");
                    }
                    break;
                }
            case "OpenWorldMain":
                {
                    SoundManager.instance.GetAudioSources();
                    SoundManager.instance.AssignVolumeIngame();
                    if (GameObject.Find("Pistol") != null)
                    {
                        SoundManager.instance.GunSource = GameObject.Find("Pistol").GetComponent<AudioSource>();
                        Debug.Log("CodeReached");
                    }
                    break;
                }
            case "CampDynamite":
                {
                    SoundManager.instance.AssignVolumeIngame();

                    break;
                }
            case "SubwayScene":
                {
                    SoundManager.instance.AssignVolumeIngame();
                    break;
                }
        }
    }
}
