using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
[System.Serializable]
public class Settings
{
    // Saveable settings such as graphics and volume 
    public float MusicVol;
    public float FxVol;
    public float NpcVol;
    public int GraphicsLevel;
}

[System.Serializable]

public class IngameData
{
    // Saving of procedurally generated objects is handled by the procedural generation manager
    // This is gonna be a difficult one 
    public int PlayerHealth;
    public Vector3 PlayerSpawnPosition;
    public List<string> MagObject = new List<string>();
    public List<int> AmmoValue = new List<int>();
}

public class SavingAndLoading : MonoBehaviour
{
    public static SavingAndLoading Instance;
    private string FilePathSettings;
    private string FilePathGameData;
    public GameObject MagPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(this);
        FilePathSettings = Path.Combine(Application.persistentDataPath, "Settings.json");
        FilePathGameData = Path.Combine(Application.persistentDataPath, "PlayerData.json");
    }
    public void SaveSettings()
    { 
        Settings settings = new Settings();
        settings.MusicVol = UiManager.Instance.MusicSlider.value;
        settings.FxVol = UiManager.Instance.SoundSlider.value;
        settings.NpcVol = UiManager.Instance.NpcSlider.value;
        settings.GraphicsLevel = QualitySettings.GetQualityLevel();

        string json = JsonUtility.ToJson(settings,true);
        File.WriteAllText(FilePathSettings, json);
    }
    public void LoadSettings() 
    {
        if (File.Exists(FilePathSettings))
        {
            Debug.Log("FileExists");
            string json = File.ReadAllText(FilePathSettings);
            Settings settings = JsonUtility.FromJson<Settings>(json);

            SoundManager.Instance.MusicVol = settings.MusicVol;
            SoundManager.Instance.FxVol = settings.FxVol;
            SoundManager.Instance.NpcVol = settings.NpcVol;
            UiManager.Instance.SetGraphics(settings.GraphicsLevel);
        }
    }
    public void SaveIngameData(Vector3 NextLocation) 
    {
        IngameData ingameData = new IngameData();

        ingameData.PlayerHealth = PlayerController.Instance.PlayerHealth;
        ingameData.PlayerSpawnPosition = NextLocation;
        // add the player sockets to playercontroller and then get the info from there 
        foreach (GameObject obj in PlayerController.Instance.MagList)
        {
            ingameData.MagObject.Add(obj.name);
            ingameData.AmmoValue.Add(obj.GetComponent<Magazine>().BulletNumber);
        }
        string json = JsonUtility.ToJson(ingameData, true);
        File.WriteAllText(FilePathGameData, json);
    }

    //Do 12/03/25 
    [System.Obsolete]
    public void LoadIngameData()
    {
        if (File.Exists(FilePathGameData))
        {
            string json = File.ReadAllText(FilePathGameData);
            IngameData data = JsonUtility.FromJson<IngameData>(json);

            PlayerController.Instance.PlayerHealth = data.PlayerHealth;
            if (data.PlayerSpawnPosition != new Vector3(0, 0, 0))
            {
                PlayerController.Instance.PlayerTransform.position = data.PlayerSpawnPosition;
            }
            for (int i = 0; i < data.MagObject.Count; i++)
            {
                //Add the saved ammo to the mag 
                GameObject Mag = Instantiate(MagPrefab);
                Mag.GetComponent<Magazine>().BulletNumber = data.AmmoValue[i];
                PlayerController.Instance.MagLocations[i].startingSelectedInteractable = Mag.GetComponent<XRGrabInteractable>();
                PlayerController.Instance.MagLocations[i].StartManualInteraction(Mag.GetComponent<XRGrabInteractable>());
            }
        }
    }
}
