using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] public AudioSource source,GunSource,WatchSource, AmbienceSource;
    [SerializeField] private AudioClip MenuClipSelect, MenuClipHover;
    [SerializeField] private AudioClip GunEmpty, GunReload, GunUnload, PlayerDeathClip;
    [HideInInspector]public bool WatchIsPlaying;
    [SerializeField] private AudioClip[] VoiceLine;
    [SerializeField] private bool VoiceLineFinished;
    [SerializeField] private float VoiceLineTimer;
    // 
    [SerializeField] private AudioClip ScaryAmbienceClip, ChillAmbienceClip;
    public List<AudioSource>SoundFxSources = new List<AudioSource>();
    public List<AudioSource>MusicSources = new List<AudioSource>();
    public List<AudioSource>NpcSources = new List<AudioSource>();
    [SerializeField] private Slider MusicSlider, SoundSlider, NpcSlider;
    [SerializeField] private float MusicVol;
    [SerializeField] private float FxVol;
    [SerializeField] private float NpcVol;
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
    public void AssignVolumeStartMenu()
    {
        MusicVol = MusicSlider.value;
        FxVol = SoundSlider.value;
        NpcVol = NpcSlider.value;
        //
        AmbienceSource.volume = MusicSlider.value;
        source.volume = SoundSlider.value;
    }
    public void GetAudioSources()
    {
        AudioCategory[] Objects = FindObjectsOfType<AudioCategory>();

        foreach (AudioCategory category in Objects)
        {
            AudioSource source = category.GetComponent<AudioSource>();
            if (source != null) 
            {
                switch (category.Category)
                {
                    case AudioCategory.CategoryType.Music:
                        {
                            MusicSources.Add(source);
                            break;
                        }
                    case AudioCategory.CategoryType.SoundFx:
                        {
                            SoundFxSources.Add(source);
                            break;
                        }
                    case AudioCategory.CategoryType.Npc:
                        {
                            NpcSources.Add(source);
                            break;
                        }
                }
            }
        }
    }
    public void AddAudioSource(AudioCategory.CategoryType categoryType)
    {
        switch (categoryType)
        {
            case AudioCategory.CategoryType.Music:
                MusicSources.Add(source);
                break;
            case AudioCategory.CategoryType.SoundFx:
                SoundFxSources.Add(source);
                break;
            case AudioCategory.CategoryType.Npc:
                NpcSources.Add(source);
                break;
        }
    }
    public void AssignVolumeIngame()
    {
        if (MusicSources != null)
        {
            foreach (AudioSource source in MusicSources)
            {
                source.volume = MusicVol;
            }
        }
        if (SoundFxSources != null)
        {
            foreach (AudioSource source in SoundFxSources)
            {
                source.volume = FxVol;
            }
        }
        if (NpcSources != null)
        {
            foreach (AudioSource source in NpcSources)
            {
                source.volume = NpcVol;
            }
        }
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
                StoryManager.Instance.CurrentState++;
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
