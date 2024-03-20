using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    public static BulletTrail instance;
    [SerializeField] private LineRenderer Line;
   // [SerializeField] private ParticleSystem System;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Line= GetComponent<LineRenderer>();
        Line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LineActivate()
    {
        Line.enabled = true;
       // System.Play();
    }
    public void LineDeactivate() 
    { 
        Line.enabled = false;
       // System.Stop();
        Line.SetPosition(0, WeaponManager.instance.HeldWeapon.transform.position);
        Line.SetPosition(1, WeaponManager.instance.HeldWeapon.transform.position);
    }
    private void FixedUpdate()
    {
        if(!Line.enabled) return;

        Ray ray = new Ray(WeaponManager.instance.HeldWeapon.transform.position, WeaponManager.instance.HeldWeapon.transform.forward);
        bool cast = Physics.Raycast(ray, out RaycastHit hit, 100);
        Vector3 hitPos = cast? hit.point : WeaponManager.instance.HeldWeapon.transform.position + WeaponManager.instance.HeldWeapon.transform.forward * 100;

        Line.SetPosition(0, WeaponManager.instance.HeldWeapon.transform.position);
        Line.SetPosition(1, hitPos);
      //  System.transform.position= hitPos;
    }
}
