using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;
  [SerializeField] public Weapon CurrentWeapon;
    [SerializeField] public GameObject HeldWeapon;
    [SerializeField]private float LastShot;
    [SerializeField] public XrSocketTag[] AllInteractors;
    [SerializeField] private Magazine magazine;
    [SerializeField] public Light MuzzleFlash;
    void Awake()
    {
        instance= this;
    }
    private void Start()
    {
        for (int i = 0; i < AllInteractors.Length; i++)
        {
            AllInteractors[i].selectEntered.AddListener(AddMagazine);
            AllInteractors[i].selectExited.AddListener(RemoveMagazine);
        }
    }
    private void Update()
    {
        if (MuzzleFlash != null) { MuzzleFlash.enabled = false; }
    }
    public void Fire()
    {
      
        if (Time.time - LastShot < (CurrentWeapon.FireRate))
        {
            return;
        }
        LastShot = Time.time;
        RaycastHit hit;

            if (magazine != null && magazine.BulletNumber > 0)
            {
                //Play Sound
                SoundManager.instance.PlayGunshot(CurrentWeapon);
                //Take Ammo 
                magazine.BulletNumber--;
                //MuzzleFlash 
               // MuzzleFlash.enabled = true;
                if (Physics.Raycast(HeldWeapon.transform.position, HeldWeapon.transform.forward, out hit, 100))
                {
                    if (hit.collider.CompareTag("Zombie"))
                    {
                        //Get the hit zombie 
                        Zombie_Behaviour zombie_Behaviour = hit.collider.GetComponent<Zombie_Behaviour>();
                        // Deal Damage 
                        zombie_Behaviour.ZombieHealth -= CurrentWeapon.DamageValue;
                        //Apply Stun
                        zombie_Behaviour.ShotStun();
                        //Take ammo
                        zombie_Behaviour.DeathCheck();
                    }
                }
            }
            else { SoundManager.instance.PlayEmpty(); }
    }
    public void AddMagazine(SelectEnterEventArgs args)
    {
        magazine = args.interactableObject.transform.GetComponent<Magazine>();
        SoundManager.instance.PlayReload(true);
    }
    public void RemoveMagazine(SelectExitEventArgs args)
    {
        magazine = null;
        SoundManager.instance.PlayReload(false);
    }
}
