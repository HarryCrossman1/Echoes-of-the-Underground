using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompass : MonoBehaviour
{
    [SerializeField] private GameObject Compass;
    // Start is called before the first frame update
    void Awake()
    {
        if (GameObject.Find("Compass") != null)
        {
            Compass = GameObject.Find("Compass");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
