using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    public int BulletNumber;
    public int MaxAmmo;

    public void UseGravity()
    { 
        //hack
        gameObject.GetComponent<Rigidbody>().useGravity= true;
    }
}
