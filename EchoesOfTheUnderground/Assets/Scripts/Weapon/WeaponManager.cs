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

    // Visual Effects 
    [SerializeField] private GameObject BloodPrefab;

    [SerializeField] private Weapon Smg;

    public int ShotsHit;
    public int ShotsTaken;
    public int StoredAmmoCount;
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
        if (Physics.Raycast(HeldWeapon.transform.position, HeldWeapon.transform.forward, out hit, 100))
        {
            if (hit.collider.CompareTag("ZombieBody"))
            {
                ShotsHit++;
                //Get the hit zombie 
                Zombie_Behaviour zombie_Behaviour = hit.collider.GetComponentInParent<Zombie_Behaviour>();
                // Deal Damage 
                zombie_Behaviour.ZombieCurrentHealth -= CurrentWeapon.DamageValue;

                //Take Checkdeath
                zombie_Behaviour.DeathCheck();

                // Do blood effect
                GameObject Blood = Instantiate(BloodPrefab, hit.collider.gameObject.transform);
                Blood.transform.position = hit.point;
                GameManager.Instance.BulletWounds.Add(Blood);
            }
            else if (hit.collider.CompareTag("ZombieHead"))
            {
                ShotsHit++;
                Zombie_Behaviour zombie_Behaviour = hit.collider.GetComponentInParent<Zombie_Behaviour>();
                // Deal Damage 
                zombie_Behaviour.ZombieCurrentHealth -= CurrentWeapon.DamageValue * 2;

                //Take Checkdeath
                zombie_Behaviour.DeathCheck();

                // Do blood effect
                GameObject Blood = Instantiate(BloodPrefab, hit.collider.gameObject.transform);
                Blood.transform.position = hit.point;
                GameManager.Instance.BulletWounds.Add(Blood);
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
                DynamiteExplosion.Instance.TriggerExplosion();
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
        HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag = args.interactableObject.transform.GetComponent<Magazine>();
        Debug.Log(args.interactableObject.transform.GetComponent<Magazine>());
        SoundManager.Instance.PlayReload(true);
    }
    public void RemoveMagazine(SelectExitEventArgs args)
    {
        HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag = null;
        SoundManager.Instance.PlayReload(false);
    }
    
}
