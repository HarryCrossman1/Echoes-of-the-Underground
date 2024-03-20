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
    private float GunCoolDownTimer { get; set; }
    void Awake()
    {
        instance= this;
    }
    private void Start()
    {
        GunCoolDownTimer = 1;   
    }

    // Update is called once per frame
    void Update()
    {
        GunCoolDown(CurrentWeapon.FireRate);
        Debug.Log(GunCoolDownTimer);
    }
    public void Fire(Weapon CurrentWeapon)
    {
        
        RaycastHit hit;
        if(HeldWeapon!=null&& GunCoolDownTimer <=0)
        {
            Debug.DrawRay(HeldWeapon.transform.position, HeldWeapon.transform.forward, Color.red);
            BulletTrail.instance.LineActivate();
            if (Physics.Raycast(HeldWeapon.transform.position, HeldWeapon.transform.forward, out hit, 50))
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
            else
            {
                Debug.Log("Missed");
            }
        }
       
    }
    private void GunCoolDown(float FireRate)
    {
        if (GunCoolDownTimer >= 0)
        {
            GunCoolDownTimer -= FireRate * Time.deltaTime;
        }
        else
        { 
            
        }
    }
}
