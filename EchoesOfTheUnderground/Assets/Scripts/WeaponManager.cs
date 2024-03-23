using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;
  [SerializeField] public Weapon CurrentWeapon;
    [SerializeField] public GameObject HeldWeapon;
    [SerializeField]private float LastShot;
    void Awake()
    {
        instance= this;
    }
    public void Fire(Weapon CurrentWeapon)
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
            if (Physics.Raycast(HeldWeapon.transform.position, HeldWeapon.transform.forward, out hit, 100))
            {
                Debug.Log("Fire");
                if (hit.collider.CompareTag("Zombie"))
                {
                    //Get the hit zombie 
                    Zombie_Behaviour zombie_Behaviour = hit.collider.GetComponent<Zombie_Behaviour>();
                    // Deal Damage 
                    zombie_Behaviour.ZombieHealth -= CurrentWeapon.DamageValue;
                    //Take ammo
                    CurrentWeapon.AmmoValue--;
                    zombie_Behaviour.DeathCheck();
                }
            }
            else
            {
                Debug.Log("Missed");
            }
        }
       
    }
}
