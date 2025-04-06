using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject CurrentGrenade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PullPin()
    {
        StartCoroutine(GrenadeCountdown(3));
        gameObject.transform.SetParent(null);
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }  

    private IEnumerator GrenadeCountdown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        CurrentGrenade.GetComponentInChildren<ParticleSystem>().Play();
        CurrentGrenade.GetComponent<AudioSource>().Play();
        Collider[] HitColliders = Physics.OverlapSphere(CurrentGrenade.transform.position, 5);
        Debug.Log("Boom");
        foreach (var Hit in HitColliders)
        {
            if (Hit.CompareTag("ZombieBody"))
            {
                HighScoreManager.instance.CurrentHighScore += 75;
                Hit.gameObject.GetComponentInParent<Zombie_Behaviour>().ZombieCurrentHealth -= 300;
            }
            else if (Hit.CompareTag("Dynamite"))
            {
                DynamiteExplosion.instance.TriggerExplosion();
            }
        }
        yield return new WaitForSeconds(seconds/2);
        CurrentGrenade.SetActive(false);
    }
}
