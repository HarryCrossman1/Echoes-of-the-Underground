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
    [SerializeField]private float GunCoolDownTimer;
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
        GunCoolDown();
        Debug.Log(GunCoolDownTimer);
    }
    public void Fire(Weapon CurrentWeapon)
    {
        
        RaycastHit hit;
        if (HeldWeapon != null && GunCoolDownTimer <= 0)
        {
            GunCoolDownTimer+=CurrentWeapon.FireRate;
 
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
        else
        {
            BulletTrail.instance.LineDeactivate();
        }
       
    }
    private void GunCoolDown()
    {
        if (GunCoolDownTimer >= 0)
        {
            GunCoolDownTimer -= Time.deltaTime;
        }
    }
}
