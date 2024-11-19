using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSetter : MonoBehaviour
{
    private string LevelName;
    // Start is called before the first frame update
    void Start()
    {
        
        DontDestroyOnLoad(this);
        LevelName = SceneManager.GetActiveScene().name;
        switch (LevelName)
        {
            case "MenuScene":
                {
                    SoundManager.instance.source = GameObject.Find("soundfx").GetComponent<AudioSource>();
                    break;
                }
            case "HomeScene":
                {
                    SoundManager.instance.GunSource = GameObject.Find("M1911 Handgun Base Variant").GetComponent<AudioSource>();
                    break;
                }
            case "OpenWorldMain":
                {
                    SoundManager.instance.GunSource = GameObject.Find("M1911 Handgun Base Variant").GetComponent<AudioSource>();
                    break;
                }
            case "SubwayScene":
                {
                    break;
                }
        }
    }
}
