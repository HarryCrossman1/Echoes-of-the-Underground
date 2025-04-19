using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;
    public Weapon CurrentWeapon;
    public GameObject HeldWeapon;
    public Animator HeldAnimator;
    [SerializeField]private float LastShot;
    public XrSocketTag[] AllInteractors;

    // Visual Effects 
    [SerializeField] private GameObject BloodPrefab;

    [SerializeField] private Weapon Smg;
    //Accuracy
    public int ShotsHit;
    [SerializeField]private int ShotsTaken;
    void Awake()
    {
        instance= this;
    }
    private void Start()
    {
       
        for (int i = 0; i < AllInteractors.Length; i++)
        {
            if (AllInteractors[i] != null)
            {
                AllInteractors[i].selectEntered.AddListener(AddMagazine);
                AllInteractors[i].selectExited.AddListener(RemoveMagazine);
            }
           
        }
    }
    private void Update()
    {

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
            SoundManager.Instance.PlayGunshot(CurrentWeapon);
            HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag.BulletNumber--;
        }
        else
        {
            SoundManager.Instance.PlayEmpty();
        }
        if (Physics.Raycast(HeldWeapon.transform.position, HeldWeapon.transform.forward, out hit, 100) && HeldWeapon!=null && HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag!=null && HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag.BulletNumber > 0)
        {
            if (hit.collider.CompareTag("ZombieBody"))
            {
                ShotsHit++;
                //Get the hit zombie 
                Zombie_Behaviour zombie_Behaviour = hit.collider.GetComponentInParent<Zombie_Behaviour>();
                // Deal Damage 
                zombie_Behaviour.ZombieCurrentHealth -= CurrentWeapon.DamageValue;

                // Do blood effect
                GameObject Blood = Instantiate(BloodPrefab, hit.collider.gameObject.transform);
                Blood.transform.position = hit.point;
                GameManager.Instance.BulletWounds.Add(Blood);
                // Sound 
                zombie_Behaviour.PlayZombieAudio(zombie_Behaviour.GetComponent<Zombie_Behaviour>().ShotAudio, false);
            }
            else if (hit.collider.CompareTag("ZombieHead"))
            {
                ShotsHit++;
                Zombie_Behaviour zombie_Behaviour = hit.collider.GetComponentInParent<Zombie_Behaviour>();
                // Deal Damage 
                zombie_Behaviour.ZombieCurrentHealth -= CurrentWeapon.DamageValue * 2;

                // Do blood effect
                GameObject Blood = Instantiate(BloodPrefab, hit.collider.gameObject.transform);
                Blood.transform.position = hit.point;
                GameManager.Instance.BulletWounds.Add(Blood);
                //
                zombie_Behaviour.PlayZombieAudio(zombie_Behaviour.GetComponent<Zombie_Behaviour>().ShotAudio, false);
            }
            else if (hit.collider.CompareTag("MiscItem"))
            {
                if (StoryManager.Instance.gameObject != null)
                {
                    // In the tutorial the storymanager need to check if the bottle has been shot 
                    StoryManager.Instance.HitMiscItem = false;
                }   
                if (hit.collider.attachedRigidbody != null)
                {
                    Rigidbody body = hit.collider.attachedRigidbody;
                    body.AddForce(hit.point, ForceMode.Impulse);

                    if (StoryManager.Instance.gameObject != null)
                    {
                        StoryManager.Instance.HitMiscItem = true;
                    }
                }
            }
            else if (hit.collider.CompareTag("Dynamite"))
            {
                hit.collider.gameObject.GetComponent<DynamiteExplosion>().TriggerExplosion();
            }
            else
            {
                ShotsTaken++;
            }
            if (ShotsTaken > 0)
            {
                GameManager.Instance.AccuracyRating = (ShotsHit / (float)ShotsTaken) * 100f;
            }
            else
            {
                GameManager.Instance.AccuracyRating = 0f;
            }
        }
    }
    public void AddMagazine(SelectEnterEventArgs args)
    {
        if (HeldWeapon != null)
        {
            HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag = args.interactableObject.transform.GetComponent<Magazine>();
            Debug.Log(args.interactableObject.transform.GetComponent<Magazine>());
        }

        if (SoundManager.Instance != null && SoundManager.Instance.GunSource!=null)
        {
            SoundManager.Instance.PlayReload(true);
        }
    }
    public void RemoveMagazine(SelectExitEventArgs args)
    {
        if (HeldWeapon != null)
        {
            var weaponPickup = HeldWeapon.GetComponentInParent<XrWeaponPickup>();
            if (weaponPickup != null && weaponPickup.CurrentMag != null)
            {
                weaponPickup.CurrentMag = null;
            }
        }
        if (SoundManager.Instance != null && SoundManager.Instance.GunSource != null)
        {
            SoundManager.Instance.PlayReload(false);
        }
    }
    
}
