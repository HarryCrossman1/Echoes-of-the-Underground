using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [Header("Main Menu")]
    public AudioSource GenericSource;
    [SerializeField] private AudioSource AmbienceSource;
    [SerializeField] private AudioClip MenuClipSelect;

    [Header("In Game")]
    public AudioSource GunSource;
    public AudioSource  WatchSource;
    [SerializeField] private AudioClip GunEmpty, GunReload, GunUnload, PlayerDeathClip;
    [SerializeField] private AudioClip ScaryAmbienceClip, ChillAmbienceClip;
    [HideInInspector]public bool WatchIsPlaying = false;
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
        if (Instance == null)
        {
            Instance = this;
        }
        else
        { 
            Destroy(gameObject);
        }
    }
    public void GetAudioSources()
    {
        AudioCategory[] Objects = FindObjectsOfType<AudioCategory>();
        MusicSources.Clear();
        SoundFxSources.Clear();
        NpcSources.Clear();
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
        if (SavingAndLoading.Instance != null)
        {
            SavingAndLoading.Instance.LoadSettings();
        }
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
        GenericSource?.Play();
    }
    public void PlayGunshot(Weapon weapon)
    {
        GunSource.clip = weapon.Clip;
        GunSource?.Play();
    }
    public void PlayEmpty()
    {
        GunSource.clip = GunEmpty;
        GunSource?.Play();
    }
    public void PlayReload(bool Loading)
    {
        if (Loading)
        {
            GunSource.clip = GunReload;
            GunSource?.Play();
        }
        else
        {
            GunSource.clip = GunUnload;
            GunSource?.Play();
        }
    }
    public void PlayWatch()
    {
        if (!WatchIsPlaying)
        {
            WatchSource?.Play();
            WatchIsPlaying= true;
        }
    }
    public void StopWatch()
    {
        WatchSource?.Stop();
        WatchIsPlaying = false;
    }
    public void PlayDeath()
    {
        GunSource.clip = PlayerDeathClip;
        GunSource?.Play();
    }
    public void PlayVoiceLine(AudioSource source, CharacterHolder character, int voicelineNum, bool random = true)
    {
        if (source == null || character == null)
        {
            Debug.LogError("AudioSource or Character is null");
            return;
        }

        if (character.ReadyForVoiceline)
        {
            AudioClip clip = random
                ? character.Character.Clips[Random.Range(0, character.Character.Clips.Length)]
                : character.Character.Clips[voicelineNum];

            source.clip = clip;
            source.Play();
            character.ReadyForVoiceline = false;
            character.VoiceLineTimer = 0f; // reset timer so CharacterHolder picks it up
        }
    }
    public void SoundManagerSetup()
    {
        // Get the sources
        if (GameObject.Find("Pistol") != null)
        {
            GunSource = GameObject.Find("Pistol").GetComponent<AudioSource>();
        }
        if (GameObject.Find("Watch") != null)
        {
            WatchSource = GameObject.Find("Watch").GetComponent<AudioSource>();
        }
        if (GameObject.Find("GenericSource") != null)
        {
            GenericSource = GameObject.Find("GenericSource").GetComponent<AudioSource>();

        }
        if (GameObject.Find("AmbientSource") != null)
        {
            AmbienceSource = GameObject.Find("AmbientSource").GetComponent<AudioSource>();
        }
        // Load the settings if they exist 
        if (SavingAndLoading.Instance != null)
            SavingAndLoading.Instance.LoadSettings();

        //Set the sliders for astethic purposes, actual volume set in the loadsettings()
        if (UiManager.Instance != null)
            UiManager.Instance.SetSoundSlider();
        // Get and set the audiosources ingame
        if (SoundManager.Instance != null)
        {
            GetAudioSources();
            SetAudioSources();
        }
    }
}
