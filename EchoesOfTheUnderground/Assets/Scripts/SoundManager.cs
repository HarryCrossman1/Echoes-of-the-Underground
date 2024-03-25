using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource source,GunSource;
    [SerializeField] private AudioClip MenuClipSelect, MenuClipHover;
    [SerializeField] private AudioClip GunEmpty,GunReload, GunUnload;
    // Start is called before the first frame update
    void Awake()
    {
        instance= this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
