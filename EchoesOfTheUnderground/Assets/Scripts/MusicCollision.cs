using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCollision : MonoBehaviour
{
    public static MusicCollision instance;
    private void Awake()
    {
        instance = this;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.ChangeAmbientMusic(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.ChangeAmbientMusic(true);
        }
    }
}
