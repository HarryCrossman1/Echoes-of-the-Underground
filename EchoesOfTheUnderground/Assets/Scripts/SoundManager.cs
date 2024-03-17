using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip MenuClipSelect, MenuClipHover;
    // Start is called before the first frame update
    void Awake()
    {
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
}
