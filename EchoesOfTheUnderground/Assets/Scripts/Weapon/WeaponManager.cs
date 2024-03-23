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
    [SerializeField] public XrSocketTag xRSocketInteractor;
    [SerializeField] private Magazine magazine;
    void Awake()
    {
        instance= this;
    }
    private void Start()
    {
        xRSocketInteractor.selectEntered.AddListener(AddMagazine);
        xRSocketInteractor.selectExited.AddListener(RemoveMagazine);
    }
    public void Fire()
    {
      
        if (Time.time - LastShot < (CurrentWeapon.FireRate))
        {
            return;
        }
        LastShot = Time.time;
        RaycastHit hit;
        if (HeldWeapon != null )
        {
            SoundManager.instance.PlayGunshot(CurrentWeapon);
            if (Physics.Raycast(HeldWeapon.transform.position, HeldWeapon.transform.forward, out hit, 100) && magazine.BulletNumber>0)
            {
                magazine.BulletNumber--;
                if (hit.collider.CompareTag("Zombie"))
                {
                    //Get the hit zombie 
                    Zombie_Behaviour zombie_Behaviour = hit.collider.GetComponent<Zombie_Behaviour>();
                    // Deal Damage 
                    zombie_Behaviour.ZombieHealth -= CurrentWeapon.DamageValue;
                    //Take ammo
                    zombie_Behaviour.DeathCheck();
                }
            }
            else
            {
                Debug.Log("Missed");
            }
        }
    }
    public void AddMagazine(SelectEnterEventArgs args)
    {
        magazine = args.interactableObject.transform.GetComponent<Magazine>();
    }
    public void RemoveMagazine(SelectExitEventArgs args)
    {
        magazine = null;
    }
}
