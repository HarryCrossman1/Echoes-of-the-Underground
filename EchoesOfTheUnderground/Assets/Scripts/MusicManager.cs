using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource Speaker;
   [SerializeField] private AudioClip MainTrack;
    void Start()
    {
        Speaker= GetComponent<AudioSource>();
        Speaker.clip = MainTrack;
        Speaker.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
