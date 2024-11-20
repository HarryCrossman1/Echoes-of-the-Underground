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
                    SoundManager.instance.source = GameObject.Find("soundfx").GetComponent<AudioSource>();
                    break;
                }
            case "HomeScene":
                {
                    if (GameObject.Find("Pistol") != null)
                    {
                        SoundManager.instance.GunSource = GameObject.Find("Pistol").GetComponent<AudioSource>();
                        Debug.Log("CodeReached");
                    }
                    break;
                }
            case "OpenWorldMain":
                {
                    if (GameObject.Find("Pistol") != null)
                    {
                        SoundManager.instance.GunSource = GameObject.Find("Pistol").GetComponent<AudioSource>();
                        Debug.Log("CodeReached");
                    }
                    break;
                }
            case "SubwayScene":
                {
                    break;
                }
        }
    }
}
