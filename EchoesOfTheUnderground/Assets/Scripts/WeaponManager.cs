using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
  [SerializeField] private Weapon CurrentWeapon;
    [SerializeField] private GameObject HeldWeapon;
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Fire(Weapon CurrentWeapon)
    { 
        RaycastHit hit;
        if(HeldWeapon!=null)
        {
            if (Physics.Raycast(HeldWeapon.transform.position,HeldWeapon.transform.forward,out hit))
            {
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
        }
       
    }
}
