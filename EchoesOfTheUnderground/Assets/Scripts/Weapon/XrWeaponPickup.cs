using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XrWeaponPickup : MonoBehaviour
{
    [SerializeField] private GameObject WeaponShootPoint;
    [SerializeField] private GameObject WeaponParent;
    [SerializeField] private Weapon weapon;
    public Magazine CurrentMag;
    public void WeaponPickedUp()
    {
        WeaponManager.instance.HeldWeapon = WeaponShootPoint;
        WeaponManager.instance.CurrentWeapon= weapon;
        if (weapon.Type == Weapon.AmmoType.Pistol)
        {
            WeaponParent.transform.SetParent(null);
        }
    }
}
