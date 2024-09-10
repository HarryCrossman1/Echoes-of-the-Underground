using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;
    public enum StoryState {Tutorial,Streets,Subway,Sewers,DefHome }
    public StoryState State;

    //Tutorial Stuff
    [SerializeField] private GameObject AlertIcon;
    [SerializeField] private GameObject TutorialCharacter;
    [SerializeField] public int TutorialState;
    [SerializeField] public bool HitMiscItem;
    private void Awake()
    {
        Instance = this;
    }
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
                    AlertIcon.transform.position = TutorialCharacter.transform.position + new Vector3(-0.13f,2,-0.2f);

                    if (Vector3.Distance(TutorialCharacter.transform.position, PlayerController.instance.PlayerTransform.position) < 3 && TutorialState == 0)
                    {
                        SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>().character, 0, false);
                        AlertIcon.GetComponentInChildren<MeshRenderer>().enabled = false;
                    }
                    if (TutorialState == 1)
                    {
                        TutorialCharacter.GetComponentInChildren<NavMeshAgent>().SetDestination(new Vector3(-2, 0, 2));
                        if (Vector3.Distance(TutorialCharacter.transform.position,new Vector3(-2,0,2))<2)
                        {
                            AlertIcon.GetComponentInChildren<MeshRenderer>().enabled = true;
                            if (Vector3.Distance(TutorialCharacter.transform.position, PlayerController.instance.PlayerTransform.position) < 3 && TutorialState == 1)
                            {
                                AlertIcon.GetComponentInChildren<MeshRenderer>().enabled = false;
                                SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>().character, 1, false);
                            }
                        }
                    }
                    if (Vector3.Distance(TutorialCharacter.transform.position, PlayerController.instance.PlayerTransform.position) < 3 && TutorialState == 2)
                    {
                        SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>().character, 2, false);

                    }
                    if (TutorialState == 3 && WeaponManager.instance.HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag.MaxAmmo > WeaponManager.instance.HeldWeapon.GetComponentInParent<XrWeaponPickup>().CurrentMag.BulletNumber)
                    {
                        if (HitMiscItem)
                        {
                            SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>().character, 3, false);
                        }
                        else
                        {
                            SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>().character, 4, false);
                        }
                    }
                    if (TutorialState == 4)
                    {
                        SoundManager.instance.PlayVoiceLine(TutorialCharacter.GetComponent<AudioSource>(), TutorialCharacter.GetComponent<CharacterHolder>().character, 5, false);
                    }
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
