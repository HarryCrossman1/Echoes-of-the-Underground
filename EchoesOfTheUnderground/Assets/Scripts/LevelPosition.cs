using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPosition : MonoBehaviour
{
    [SerializeField] private int LevelPos;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        { 
            GameManager.instance.LevelPosition = LevelPos;
        }
    }
}
