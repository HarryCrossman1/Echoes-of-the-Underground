using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AccessControllers();
    }
    private void AccessControllers()
    {
        var ControllerRight = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, ControllerRight);

        var ControllerLeft = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, ControllerLeft);
        foreach (var controller in ControllerRight)
        {
            if (controller.IsPressed(InputHelpers.Button.Trigger, out bool IsPressedRight) && IsPressedRight == true)
            {
                Debug.Log("RightTrigger");
                WeaponManager.instance.Fire(WeaponManager.instance.CurrentWeapon);
            }
            else
            {
                BulletTrail.instance.LineDeactivate();
            }
        }
        foreach (var controller in ControllerLeft)
        {
            if (controller.IsPressed(InputHelpers.Button.Trigger, out bool IsPressedLeft) && IsPressedLeft == true)
            {
                Debug.Log("LeftTrigger");
                WeaponManager.instance.Fire(WeaponManager.instance.CurrentWeapon);
            }
            else
            {
              //  BulletTrail.instance.LineDeactivate();
            }
        }

    }
}
