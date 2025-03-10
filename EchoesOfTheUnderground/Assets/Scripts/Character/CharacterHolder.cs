using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder : MonoBehaviour
{
    [SerializeField] public Character character;
    [SerializeField] public float VoiceLineTimer;
    [SerializeField] public bool ReadyForVoiceline;

    private void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, PlayerController.instance.PlayerTransform.position) < 3)
        {
            if (!character.TutorialCharacter)
            {
                AudioSource scr = gameObject.GetComponent<AudioSource>();
                SoundManager.instance.PlayVoiceLine(scr, this, 0);
            }
        }
    }
}
