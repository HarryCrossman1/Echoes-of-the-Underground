using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XrWeaponPickup : MonoBehaviour
{
   [SerializeField] private GameObject WeaponShootPoint;
    [SerializeField] private GameObject WeaponParent;
    [SerializeField] private Weapon weapon;
    [SerializeField] public Light MuzzleFlash;
    [SerializeField] private Animator CurrentAnimator;
    [SerializeField] private Light Torch;
    public Magazine CurrentMag;
    public void WeaponPickedUp()
    {
        WeaponManager.instance.HeldWeapon = WeaponShootPoint;
        WeaponManager.instance.CurrentWeapon= weapon;
        WeaponManager.instance.MuzzleFlash = MuzzleFlash;
        WeaponManager.instance.HeldAnimator = CurrentAnimator;
        Torch = GetComponentInChildren<Light>();
        Torch.enabled= true;
        if (weapon.Type == Weapon.AmmoType.Pistol)
        {
            WeaponParent.transform.SetParent(null);
        }
    }
}
