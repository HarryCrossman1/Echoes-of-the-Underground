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
    public void WeaponPickedUp()
    {
        WeaponManager.instance.HeldWeapon = WeaponShootPoint;
        WeaponManager.instance.CurrentWeapon= weapon;
        WeaponManager.instance.MuzzleFlash = MuzzleFlash;
        if (weapon.Type == Weapon.AmmoType.Pistol)
        {
            WeaponParent.transform.SetParent(null);
        }
    }
    public void PistolDropped()
    {
        WeaponParent.transform.position = PlayerController.instance.PistolHip.position;
        Vector3 DirVec = PlayerController.instance.PistolHip.position - WeaponParent.transform.position;
        Quaternion TargetAngle = Quaternion.LookRotation(DirVec);
        WeaponParent.transform.rotation = TargetAngle;
        WeaponParent.transform.SetParent(PlayerController.instance.PistolHip);
    }
}
