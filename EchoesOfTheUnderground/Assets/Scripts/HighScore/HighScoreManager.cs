//using AcmLib;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager instance;
    public int CurrentHighScore;

    public int AllTimeHighScore;

    private HighScore HighScoreData;
    private string Path;

    private void Awake()
    {
        instance= this;
        Path = Application.persistentDataPath + ".txt";
        HighScoreData = new HighScore();
       
        DontDestroyOnLoad(this);
        
    }

    public void Save()
    {
        if (CurrentHighScore >= AllTimeHighScore)
        {
            HighScoreData.Score = CurrentHighScore;
           // Saving.WriteToXmlFile(Path, HighScoreData);
        }
    }
    public void Load()
    {
        if (File.Exists(Path))
        {
            Debug.Log(Path);
           // HighScoreData = (HighScore)Saving.ReadFromXmlFile(Path, HighScoreData);
            
        }
        else
        {
            Debug.LogError("There is no original save file, try creating an original save file");
            // create the file here 
            HighScoreData.Score=0;
            // save it 
           // Saving.WriteToXmlFile(Path, HighScoreData);
            // initilize position
            AllTimeHighScore = HighScoreData.Score;
        }
        AllTimeHighScore = HighScoreData.Score;
    }
}
