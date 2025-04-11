using UnityEngine;

public class Medkit : MonoBehaviour
{
    public void MedkitPickup()
    {
        Debug.Log("PickedUp");
        if (PlayerController.instance.PlayerHealth <= 2)
        {
            PlayerController.instance.AddHealth();
            SoundManager.instance.GenericSource.clip = gameObject.GetComponent<AudioSource>().clip;
            SoundManager.instance.GenericSource.Play();
            gameObject.SetActive(false);

        }
    }
}
