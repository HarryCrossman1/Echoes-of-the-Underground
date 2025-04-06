using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder : MonoBehaviour
{
    [SerializeField] public Character character;
    [SerializeField] public float VoiceLineTimer;
    [SerializeField] public bool ReadyForVoiceline = true;

    [SerializeField] private float DialogueRadius = 3f;
    [SerializeField] private float NpcCooldown = 10f;

    private float npcCooldownTimer = 0f;
    private bool hasPlayedAmbientLine = false;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, PlayerController.instance.PlayerTransform.position);

        
        if (!ReadyForVoiceline && source.isPlaying)
        {
            VoiceLineTimer += Time.deltaTime;
        }

        
        if (!source.isPlaying && !ReadyForVoiceline)
        {
            ReadyForVoiceline = true;
            VoiceLineTimer = 0f;

            if (character.TutorialCharacter)
            {
                StoryManager.instance.CurrentState++;
            }

            hasPlayedAmbientLine = false; // Allow ambient NPCs to speak again
        }

        
        if (character.TutorialCharacter)
        {
            return;
        }

        
        if (distance < DialogueRadius && ReadyForVoiceline && !hasPlayedAmbientLine && npcCooldownTimer <= 0f)
        {
            SoundManager.instance.PlayVoiceLine(source, this, 0, random: true);
            hasPlayedAmbientLine = true;
            npcCooldownTimer = NpcCooldown;
        }

        
        if (npcCooldownTimer > 0f)
        {
            npcCooldownTimer -= Time.deltaTime;
        }
    }
}
