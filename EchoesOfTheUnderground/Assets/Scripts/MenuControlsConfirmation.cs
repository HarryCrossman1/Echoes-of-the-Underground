using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class MenuControlsConfirmation : MonoBehaviour
{
    private InputDevice LeftController;
    private InputDevice RightController;

    [SerializeField] private TextMeshProUGUI Shoot, Pickup, Menu, Move, TurnCamera;
    [SerializeField] private bool ShootBool, PickupBool, MenuBool, MoveBool, TurncameraBool;
    [SerializeField] private GameObject ControlsPanel, DisclamerPanel;

    void Start()
    {
        InitializeControllers();
    }

    void InitializeControllers()
    {
        var leftDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftDevices);
        if (leftDevices.Count > 0)
            LeftController = leftDevices[0];

        var rightDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightDevices);
        if (rightDevices.Count > 0)
            RightController = rightDevices[0];
    }

    void Update()
    {
        if (!LeftController.isValid || !RightController.isValid)
        {
            InitializeControllers();
            return;
        }
        if (ControlsPanel.activeSelf)
        {
            if (!ShootBool &&(LeftController.TryGetFeatureValue(CommonUsages.triggerButton, out bool leftTrigger) && leftTrigger || RightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool rightTrigger) && rightTrigger))
            {
                Shoot.color = Color.green;
                ShootBool = true;
                SoundManager.Instance.PlaySelectSound();
                Debug.Log("Trigger pressed");
            }

            if (!PickupBool &&(LeftController.TryGetFeatureValue(CommonUsages.gripButton, out bool leftGrip) && leftGrip || RightController.TryGetFeatureValue(CommonUsages.gripButton, out bool rightGrip) && rightGrip))
            {
                Pickup.color = Color.green;
                PickupBool = true;
                SoundManager.Instance.PlaySelectSound();
                Debug.Log("Grip pressed");
            }

            if (!MenuBool &&(LeftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPrimary) && leftPrimary || RightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPrimary) && rightPrimary))
            {
                Menu.color = Color.green;
                MenuBool = true;
                SoundManager.Instance.PlaySelectSound();
                Debug.Log("Primary button (A/X) pressed");
            }

            if (!MoveBool && LeftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 leftStick) && leftStick != Vector2.zero)
            {
                Move.color = Color.green;
                MoveBool = true;
                SoundManager.Instance.PlaySelectSound();
                Debug.Log("Left joystick moved (Move)");
            }

            if (!TurncameraBool && RightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 rightStick) && rightStick != Vector2.zero)
            {
                TurnCamera.color = Color.green;
                TurncameraBool = true;
                SoundManager.Instance.PlaySelectSound();
                Debug.Log("Right joystick moved (Turn)");
            }

            if (ShootBool && PickupBool && MenuBool && MoveBool && TurncameraBool)
            {
                ControlsPanel.SetActive(false);
                DisclamerPanel.SetActive(true);
            }
        }
    }
}
