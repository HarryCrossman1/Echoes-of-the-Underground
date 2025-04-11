using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [Header("Main Menu")]
    [SerializeField] public AudioSource GenericSource;
    public AudioSource AmbienceSource;
    [SerializeField] private AudioClip MenuClipSelect;

    [Header("In Game")]
    public AudioSource  GunSource;
    public AudioSource  WatchSource;
    [SerializeField] private AudioClip GunEmpty, GunReload, GunUnload, PlayerDeathClip;
    [SerializeField] private AudioClip ScaryAmbienceClip, ChillAmbienceClip;
    [HideInInspector]public bool WatchIsPlaying;
    [Header("Data")]
    public float MusicVol;
    public float FxVol;
    public float NpcVol;
   
    public List<AudioSource>SoundFxSources = new List<AudioSource>();
    public List<AudioSource>MusicSources = new List<AudioSource>();
    public List<AudioSource>NpcSources = new List<AudioSource>();
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else
        { 
            Destroy(gameObject);
        }
        Debug.Log(instance);
    }

    // Update is called once per frame
    void Start()
    {
        WatchIsPlaying = false;
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
    public void SetAudioSources()
    {
        SavingAndLoading.instance.LoadSettings();
        MusicSources.Clear();
        SoundFxSources.Clear();
        NpcSources.Clear();
        if (MusicSources != null)
        {
            foreach (AudioSource source in MusicSources)
            {
                if(source!=null)
                source.volume = MusicVol;
            }
        }
        if (SoundFxSources != null)
        {
            foreach (AudioSource source in SoundFxSources)
            {
                if (source != null)
                    source.volume = FxVol;
            }
        }
        if (NpcSources != null)
        {
            foreach (AudioSource source in NpcSources)
            {
                if (source != null)
                    source.volume = NpcVol;
            }
        }
    }
    public void PlaySelectSound()
    { 
        GenericSource.clip= MenuClipSelect;
        GenericSource.Play();
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
    public void PlayVoiceLine(AudioSource source, CharacterHolder character, int voicelineNum, bool random = true)
    {
        if (source == null || character == null)
        {
            Debug.LogError("AudioSource or Character is null.");
            return;
        }

        if (character.ReadyForVoiceline)
        {
            AudioClip clip = random
                ? character.character.Clips[Random.Range(0, character.character.Clips.Length)]
                : character.character.Clips[voicelineNum];

            source.clip = clip;
            source.Play();
            character.ReadyForVoiceline = false;
            character.VoiceLineTimer = 0f; // reset timer so CharacterHolder picks it up
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
