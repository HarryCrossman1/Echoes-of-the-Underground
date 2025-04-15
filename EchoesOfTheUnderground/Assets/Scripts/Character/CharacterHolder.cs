using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder : MonoBehaviour
{
    [SerializeField] public Character Character;
    [SerializeField] public float VoiceLineTimer;
    [SerializeField] public bool ReadyForVoiceline = true;

    [SerializeField] private float DialogueRadius = 3f;
    [SerializeField] private float NpcCooldown = 10f;

    private float NpcCooldownTimer = 0f;
    private bool HasPlayedAmbientLine = false;
    private AudioSource Source;

    private void Awake()
    {
        Source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, PlayerController.Instance.PlayerTransform.position);

        
        if (!ReadyForVoiceline && Source.isPlaying)
        {
            VoiceLineTimer += Time.deltaTime;
        }

        
        if (!Source.isPlaying && !ReadyForVoiceline)
        {
            ReadyForVoiceline = true;
            VoiceLineTimer = 0f;

            if (Character.TutorialCharacter)
            {
                StoryManager.Instance.CurrentState++;
            }

            HasPlayedAmbientLine = false; // Allow ambient NPCs to speak again
        }

        
        if (Character.TutorialCharacter)
        {
            return;
        }

        
        if (distance < DialogueRadius && ReadyForVoiceline && !HasPlayedAmbientLine && NpcCooldownTimer <= 0f)
        {
            SoundManager.Instance.PlayVoiceLine(Source, this, 0, random: true);
            HasPlayedAmbientLine = true;
            NpcCooldownTimer = NpcCooldown;
        }

        
        if (NpcCooldownTimer > 0f)
        {
            NpcCooldownTimer -= Time.deltaTime;
        }   
    }
}
