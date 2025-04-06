using UnityEngine;

public class Medkit : MonoBehaviour
{
    public void MedkitPickup()
    {
        Debug.Log("PickedUp");
        if (PlayerController.instance.PlayerHealth <= 2)
        {
            PlayerController.instance.AddHealth();
            gameObject.SetActive(false);
            Debug.Log("RestoringHealth");
        }
    }
}
