using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaistTracker : MonoBehaviour
{
   [SerializeField] private Transform headTransform; // Put on main camera 
   [SerializeField] private Vector3 waistOffset = new Vector3(0, -0.5f, 0); // Adjust Y val 

    void Update()
    {
        //Keep waist aligned with head 
        Vector3 forwardFlat = Vector3.ProjectOnPlane(headTransform.forward, Vector3.up).normalized;
        transform.position = headTransform.position + waistOffset;
        transform.rotation = Quaternion.LookRotation(forwardFlat);
    }
}
