using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XrWeaponPickup : MonoBehaviour
{
   [SerializeField] private GameObject WeaponShootPoint;
    [SerializeField] private Weapon weapon;
    [SerializeField] private XrSocketTag socket;
    public void WeaponPickedUp()
    {
        WeaponManager.instance.HeldWeapon = WeaponShootPoint;
        WeaponManager.instance.CurrentWeapon= weapon;
        WeaponManager.instance.xRSocketInteractor = socket;
    }
}
