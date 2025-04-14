using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuildLogger : MonoBehaviour
{
    public static BuildLogger Instance;
    public string FileName;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void BuildLog(string text)
    {
        FileName = Path.Combine(Application.persistentDataPath, "BuildLog.json");
        File.AppendAllText(FileName, text);

    }
}
