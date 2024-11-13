using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource source,GunSource,WatchSource;
    [SerializeField] private AudioClip MenuClipSelect, MenuClipHover;
    [SerializeField] private AudioClip GunEmpty, GunReload, GunUnload, PlayerDeathClip;
    [HideInInspector]public bool WatchIsPlaying;
    [SerializeField] private AudioClip[] VoiceLine;
    [SerializeField] private bool VoiceLineFinished;
    [SerializeField] private float VoiceLineTimer;
    // 
    [SerializeField] private AudioSource AmbienceSource;
    [SerializeField] private AudioClip ScaryAmbienceClip, ChillAmbienceClip;
    // Start is called before the first frame update
    void Awake()
    {
        instance= this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Start()
    {
        WatchIsPlaying = false;
    }
    public void PlaySelectSound()
    { 
        source.clip= MenuClipSelect;
        source.Play();
    }
    public void PlayHoverSound()
    { 
        
    }
    public void PlayGunshot(Weapon weapon)
    {
        GunSource.clip = weapon.Clip;
        GunSource.Play();
    }
    public void PlayEmpty()
    {
        GunSource.clip = GunEmpty;
        GunSource.Play();
    }
    public void PlayReload(bool Loading)
    {
        if (Loading)
        {
            GunSource.clip = GunReload;
            GunSource.Play();
        }
        else
        {
            GunSource.clip = GunUnload;
            GunSource.Play();
        }
    }
    public void PlayWatch()
    {
        if (!WatchIsPlaying)
        {
            WatchSource.Play();
            WatchIsPlaying= true;
        }
       
    }
    public void StopWatch()
    {
        WatchSource.Stop();
        WatchIsPlaying = false;
    }
    public void PlayDeath()
    {
        GunSource.clip = PlayerDeathClip;
        GunSource.Play();
    }
    public void PlayVoiceLine(AudioSource source,Character character,int voicelineNum,bool random = true)
    {
        VoiceLineTimer += Time.deltaTime;
        if (VoiceLineFinished)
        {
            if (random == true)
            {
                int rand = Random.Range(0, character.Clips.Length);
                source.clip = character.Clips[rand];
                source.Play();
            } 
            else 
            {
                source.clip = character.Clips[voicelineNum];
                source.Play();
            }
            VoiceLineFinished= false;
        }
        if (source.clip.length <= VoiceLineTimer)
        {
            if (random == true)
            {
                VoiceLineFinished = true;
                VoiceLineTimer = 0;
            }
            else
            {
                VoiceLineFinished = true;
                VoiceLineTimer = 0;
                StoryManager.Instance.TutorialState++;
            }
        
        }
    }

    public void ChangeAmbientMusic(bool IsScary)
    {
        if (IsScary)
        {
            AmbienceSource.Stop();
            AmbienceSource.clip = ScaryAmbienceClip;
            AmbienceSource.Play();
        }
        else
        {
            AmbienceSource.Stop();
            AmbienceSource.clip = ChillAmbienceClip;
            AmbienceSource.Play();
        }
    }
}
