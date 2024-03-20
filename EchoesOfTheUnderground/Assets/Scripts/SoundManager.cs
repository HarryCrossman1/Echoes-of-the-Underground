using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource source,GunSource;
    [SerializeField] private AudioClip MenuClipSelect, MenuClipHover;
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
}
