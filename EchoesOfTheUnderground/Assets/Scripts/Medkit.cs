using UnityEngine;
using System.Collections;

public class Medkit : MonoBehaviour
{
    public void MedkitPickup()
    {
        if (PlayerController.Instance.PlayerHealth <= 2)
        {
            // Add health and play audio
            PlayerController.Instance.AddHealth();
            SoundManager.Instance.GenericSource.clip = gameObject.GetComponent<AudioSource>().clip;
            SoundManager.Instance.GenericSource.Play();
            gameObject.SetActive(false);

        }
    }
}
