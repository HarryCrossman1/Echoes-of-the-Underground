using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public enum StoryState {Tutorial,Streets,Subway,Sewers,DefHome }
    public StoryState State;

    //Tutorial Stuff
    [SerializeField] private GameObject AlertIcon;
    [SerializeField] private GameObject TutorialCharacter;
    void Start()
    {
        AlertIcon.GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case StoryState.Tutorial:
            {
                    AlertIcon.GetComponentInChildren<MeshRenderer>().enabled = true;
                    AlertIcon.transform.position = TutorialCharacter.transform.position + new Vector3(0,2,0);
                    break;    
            }
        }
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