using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Streets()
    {
        UiManager.instance.LoadSceneAsync("StreetsScene", false);
    }
    public void Subway()
    {
        UiManager.instance.LoadSceneAsync("SubwayScene", false);
    }
    public void Sewers()
    {
        UiManager.instance.LoadSceneAsync("SewerScene", false);
    }
    public void DefHome()
    {
        UiManager.instance.LoadSceneAsync("HomeDefendScene", false);
    }
    public void Home() 
    {
        UiManager.instance.LoadSceneAsync("HomeScene", false);
    }
}
