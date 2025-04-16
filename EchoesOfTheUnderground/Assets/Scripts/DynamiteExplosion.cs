using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteExplosion : MonoBehaviour
{
    private ParticleSystem System;
    private AudioSource Source;
    private void Awake()
    {
        System = GetComponentInChildren<ParticleSystem>();
        Source = GetComponent<AudioSource>();
    }
    void Start()
    {

    }
    public void TriggerExplosion()
    { 
        System.Play();
        Source.Play();
        StartCoroutine(ExplosionDelay(1));
    }
    private IEnumerator ExplosionDelay(int Seconds)
    { 
        yield return new WaitForSeconds(Seconds);
        gameObject.SetActive(false);
    }
}
