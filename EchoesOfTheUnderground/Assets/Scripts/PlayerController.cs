using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public Transform PlayerTransform;
    public Transform PistolHip,AmmoHip;
    public int PlayerHealth;
    public XrSocketTag[] MagLocations;
    public List<GameObject> MagList = new List<GameObject>();
    string Path;
    private float LerpValue = 1f;
    private float LerpDuration = 1f;
    public GameObject LeftHandController, RightHandController;
    public GameObject LeftHandControllerRay,RightHandControllerRay;
    void Awake()
    {
        Path = Application.persistentDataPath + ".txt";
        Instance = this;
        PlayerTransform= transform;
        Time.timeScale = 1;
        foreach (XrSocketTag xrSocketTag in MagLocations)
        {
            xrSocketTag.selectEntered.AddListener(OnSelectEntered);
            xrSocketTag.selectExited.AddListener(OnSelectExited);
        }
    }

    // Update is called once per frame
    void Start()
    {

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
        if (UiManager.Instance.HealthText.text != null)
        {
            UiManager.Instance.HealthText.text = PlayerHealth.ToString();
        }

        if (PlayerHealth < 2)
        {
            SoundManager.Instance.PlayWatch();
        }
        else
        {
            SoundManager.Instance.StopWatch();
        }

        if (PlayerHealth <= 0)
        {
            PlayerDeath();
            Debug.Log("Dead");
        }
     
    }
    public void AddHealth()
    {
        PlayerHealth++;
        if (UiManager.Instance.HealthText.text != null)
        {
            UiManager.Instance.HealthText.text = PlayerHealth.ToString();
        }
    }
    public void PlayerDeath()
    {
        Time.timeScale= 0;
        SoundManager.Instance.PlayDeath();
        SoundManager.Instance.StopWatch();
        StartCoroutine(DeathTimer(2));
    }
    private IEnumerator DeathTimer(float seconds)
    {
        UiManager.Instance.DeathCanvas.GetComponent<Image>().enabled = true;
        UiManager.Instance.DeathCanvas.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        float Elapsed = 0f;
        while (Elapsed < LerpDuration) 
        {
            Elapsed += Time.unscaledDeltaTime;
            LerpValue = Mathf.Lerp(1f, 0f, Elapsed / LerpDuration);
            yield return null;
        }
        LerpValue = 0f;
        Color NewCol = UiManager.Instance.DeathCanvas.GetComponent<Image>().color;
        NewCol.a = LerpValue;
        UiManager.Instance.DeathCanvas.gameObject.GetComponent<Image>().color = NewCol;
        yield return new WaitForSecondsRealtime(seconds);
        Application.Quit();
    }
}
