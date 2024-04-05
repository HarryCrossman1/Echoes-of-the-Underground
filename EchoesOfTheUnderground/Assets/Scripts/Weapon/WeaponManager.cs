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
    [SerializeField] public Animator HeldAnimator;
    [SerializeField]private float LastShot;
    [SerializeField] public XrSocketTag[] AllInteractors;
    [SerializeField] public Light MuzzleFlash;
    [SerializeField] private GameObject PistolAmmoPrefab;
    // Visual Effects 
    [SerializeField] private GameObject BloodPrefab;

    [SerializeField] private Weapon Smg;
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
       // if (MuzzleFlash != null) { MuzzleFlash.enabled = false; }
    }
    public void Fire()
    {
      
        if (Time.time - LastShot < (CurrentWeapon.FireRate))
        {
            return;
        }
        LastShot = Time.time;
        RaycastHit hit;

            if (HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag != null && HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag.BulletNumber > 0)
            {
                //Play Sound
                SoundManager.instance.PlayGunshot(CurrentWeapon);
            //Take Ammo 
            HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag.BulletNumber--;
            //MuzzleFlash 
            //Play Animation 
            HeldAnimator.SetTrigger("Shooting");
               // MuzzleFlash.enabled = true;
                if (Physics.Raycast(HeldWeapon.transform.position, HeldWeapon.transform.forward, out hit, 100))
                {
                if (hit.collider.CompareTag("ZombieBody"))
                {
                    //Get the hit zombie 
                    Zombie_Behaviour zombie_Behaviour = hit.collider.GetComponentInParent<Zombie_Behaviour>();
                    // Deal Damage 
                    zombie_Behaviour.ZombieCurrentHealth -= CurrentWeapon.DamageValue;
                    //Apply Stun
                    if (CurrentWeapon != Smg)
                    {
                        zombie_Behaviour.ShotStun();
                        Debug.Log("Check");
                    }
                    
                    //Take ammo
                    zombie_Behaviour.DeathCheck();

                    // Do blood effect
                    
                    GameObject Blood = Instantiate(BloodPrefab, hit.collider.gameObject.transform);
                    Blood.transform.position = hit.point;
                    GameManager.instance.BulletWounds.Add(Blood);
                }
                else if (hit.collider.CompareTag("ZombieHead"))
                {
                    Zombie_Behaviour zombie_Behaviour = hit.collider.GetComponentInParent<Zombie_Behaviour>();
                    // Deal Damage 
                    zombie_Behaviour.ZombieCurrentHealth -= CurrentWeapon.DamageValue * 2;
                    //Apply Stun
                    if (CurrentWeapon != Smg)
                    {
                        zombie_Behaviour.ShotStun();
                    }
                    //Take ammo
                    zombie_Behaviour.DeathCheck();

                    // Do blood effect

                    GameObject Blood = Instantiate(BloodPrefab, hit.collider.gameObject.transform);
                    Blood.transform.position = hit.point;
                    GameManager.instance.BulletWounds.Add(Blood);
                }
                else if (hit.collider.CompareTag("Medkit"))
                {
                    if (PlayerController.instance.PlayerHealth < 3)
                    {
                        PlayerController.instance.PlayerHealth++;
                        UiManager.instance.HealthText.text = PlayerController.instance.PlayerHealth.ToString();
                        hit.collider.gameObject.SetActive(false);
                    }
                }
                }
            }
            else { SoundManager.instance.PlayEmpty(); }
    }
    public void AddMagazine(SelectEnterEventArgs args)
    {
        HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag = args.interactableObject.transform.GetComponent<Magazine>();
        Debug.Log(args.interactableObject.transform.GetComponent<Magazine>());
        SoundManager.instance.PlayReload(true);
    }
    public void RemoveMagazine(SelectExitEventArgs args)
    {
        HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag = null;
        SoundManager.instance.PlayReload(false);
    }
}
