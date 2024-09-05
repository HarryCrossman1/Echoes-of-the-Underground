using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character", order = 2)]
public class Character : ScriptableObject
{
    public GameObject Obj;
    public AudioClip[] Clips;
}
