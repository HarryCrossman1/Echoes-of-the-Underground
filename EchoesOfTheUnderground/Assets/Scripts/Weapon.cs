using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    public int DamageValue;
    public int AmmoValue;
    public float FireRate;
    public AudioClip Clip;

    public enum AmmoType {Pistol,Rifle,Sniper }

    public AmmoType Type;
}
