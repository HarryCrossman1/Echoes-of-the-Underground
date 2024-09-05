using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder : MonoBehaviour
{
    [SerializeField] private Character character;

    private void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, PlayerController.instance.PlayerTransform.position) < 3)
        {
            Debug.Log("here");
            AudioSource scr= gameObject.GetComponentInChildren<AudioSource>();
            SoundManager.instance.PlayVoiceLine(scr,character, 0);
        }
    }
}
