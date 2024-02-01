using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Referance : MonoBehaviour
{
    public static General_Referance instance;
    public GameObject Player;
    // Start is called before the first frame update
    void Awake()
    {
        instance= this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
