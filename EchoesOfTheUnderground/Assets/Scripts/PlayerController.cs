using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
//using AcmLib;
using System;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Transform PlayerTransform;
    public Transform PistolHip,AmmoHip;
    public int PlayerHealth;
    public XrSocketTag[] MagLocations;
    public List<GameObject> MagList = new List<GameObject>();
    string Path;
    void Awake()
    {
        Path = Application.persistentDataPath + ".txt";
        instance = this;
        PlayerTransform= transform;
        Time.timeScale = 1;
        foreach (XrSocketTag xrSocketTag in MagLocations)
        {
            xrSocketTag.selectEntered.AddListener(OnSelectEntered);
            xrSocketTag.selectExited.AddListener(OnSelectExited);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UiManager.instance != null)
           // UiManager.instance.PositionText.text = GetPlayerPosition(PlayerTransform).ToString();

        if (Input.GetKeyDown(KeyCode.I))
        { 
            SavingAndLoading.instance.SaveIngameData();
        }
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
                
            }
            else
            {
              
            }
        }
        foreach (var controller in ControllerLeft)
        {
            if (controller.IsPressed(InputHelpers.Button.Trigger, out bool IsPressedLeft) && IsPressedLeft == true)
            {
                Debug.Log("LeftTrigger");
                
            }
            else
            {
              //  BulletTrail.instance.LineDeactivate();
            }
        }

    }
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        GameObject interactor = args.interactableObject.transform.gameObject;
        MagList.Add(interactor);
    }
    public void OnSelectExited(SelectExitEventArgs args)
    {
        GameObject interactor = args.interactableObject.transform.gameObject;
        MagList.Remove(interactor);
    }
    public void PlayerDeathCheck()
    {
        if (UiManager.instance.HealthText.text != null)
        {
            UiManager.instance.HealthText.text = PlayerHealth.ToString();
        }

        if (PlayerHealth < 2)
        {
            SoundManager.instance.PlayWatch();
        }
        else
        {
            SoundManager.instance.StopWatch();
        }

        if (PlayerHealth <= 0)
        {
            PlayerDeath();
            HighScoreManager.instance.Save();
            Debug.Log("Dead");
        }
     
    }
    public Vector3 GetPlayerPosition(Transform playerTransform)
    { 
        return playerTransform.position;
    }
    public void PlayerDeath()
    {
        UiManager.instance.DeathCanvas.enabled = true;
        Time.timeScale= 0;
        SoundManager.instance.PlayDeath();
        SoundManager.instance.StopWatch();
        StartCoroutine(DeathTimer(3));
    }
    private IEnumerator DeathTimer(float seconds)
    { 
        yield return new WaitForSecondsRealtime(seconds);
        SceneManager.LoadScene("SubwayScene");
    }
}
