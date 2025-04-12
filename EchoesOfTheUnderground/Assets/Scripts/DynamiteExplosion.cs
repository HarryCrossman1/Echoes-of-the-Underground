using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteExplosion : MonoBehaviour
{
    public static DynamiteExplosion Instance;
    private ParticleSystem System;
    private AudioSource Source;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        System= GetComponentInChildren<ParticleSystem>();
        Source= GetComponent<AudioSource>();
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
