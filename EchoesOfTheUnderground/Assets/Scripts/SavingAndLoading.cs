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
    public static SavingAndLoading instance;
    private string FilePathSettings;
    private string FilePathGameData;
    [SerializeField] private GameObject MagPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        settings.MusicVol = UiManager.instance.MusicSlider.value;
        settings.FxVol = UiManager.instance.SoundSlider.value;
        settings.NpcVol = UiManager.instance.NpcSlider.value;
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

            SoundManager.instance.MusicVol = settings.MusicVol;
            SoundManager.instance.FxVol = settings.FxVol;
            SoundManager.instance.NpcVol = settings.NpcVol;
            UiManager.instance.SetGraphics(settings.GraphicsLevel);
        }
    }
    public void SaveIngameData(Vector3 NextLocation) 
    {
        IngameData ingameData = new IngameData();

        ingameData.PlayerHealth = PlayerController.instance.PlayerHealth;
        ingameData.PlayerSpawnPosition = NextLocation;
        // add the player sockets to playercontroller and then get the info from there 
        foreach (GameObject obj in PlayerController.instance.MagList)
        {
            ingameData.MagObject.Add(obj.name);
            ingameData.AmmoValue.Add(obj.GetComponent<Magazine>().BulletNumber);
        }
        string json = JsonUtility.ToJson(ingameData, true);
        File.WriteAllText(FilePathGameData, json);
    }
    //Do 12/03/25 
    public void LoadIngameData()
    {
        if (File.Exists(FilePathGameData))
        {
            string json = File.ReadAllText(FilePathGameData);
            IngameData data = JsonUtility.FromJson<IngameData>(json);

            PlayerController.instance.PlayerHealth = data.PlayerHealth;
            if (data.PlayerSpawnPosition != new Vector3(0, 0, 0))
            {
                PlayerController.instance.PlayerTransform.position = data.PlayerSpawnPosition;
            }
            for (int i = 0; i < data.MagObject.Count; i++)
            {
                GameObject Mag = Instantiate(MagPrefab);
                Mag.GetComponent<Magazine>().BulletNumber = data.AmmoValue[i];
                PlayerController.instance.MagLocations[i].startingSelectedInteractable = Mag.GetComponent<XRGrabInteractable>();
            }
        }
    }
}
