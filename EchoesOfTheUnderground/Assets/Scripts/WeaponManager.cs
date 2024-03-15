using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WeaponManager : MonoBehaviour
{
  [SerializeField] private Weapon CurrentWeapon;
    [SerializeField] private GameObject HeldWeapon;
    private float GunCoolDownTimer { get; set; }
    void Awake()
    {

    }
    private void Start()
    {
        GunCoolDownTimer = 1;   
    }

    // Update is called once per frame
    void Update()
    {
        GunCoolDown(CurrentWeapon.FireRate);
    }
    public void Fire(Weapon CurrentWeapon)
    { 
        RaycastHit hit;
        if(HeldWeapon!=null && GunCoolDownTimer <=0)
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
    private void GunCoolDown(float FireRate)
    {
        if(GunCoolDownTimer>=0)
        GunCoolDownTimer-= FireRate*Time.deltaTime;
    }
}
