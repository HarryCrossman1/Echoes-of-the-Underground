using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaistTracker : MonoBehaviour
{
   [SerializeField] private Transform HeadTransform; // Put on main camera 
   [SerializeField] private Vector3 WaistOffset = new Vector3(0, -0.5f, 0); // Adjust Y val 

    void Update()
    {
        //Keep waist aligned with head 
        Vector3 forwardFlat = Vector3.ProjectOnPlane(HeadTransform.forward, Vector3.up).normalized;
        transform.position = HeadTransform.position + WaistOffset;
        transform.rotation = Quaternion.LookRotation(forwardFlat);
    }
}
