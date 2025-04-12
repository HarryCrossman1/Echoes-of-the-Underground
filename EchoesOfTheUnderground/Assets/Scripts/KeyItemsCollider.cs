using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemsCollider : MonoBehaviour
{ 
    public static KeyItemsCollider Instance;

    private void OnCollisionEnter(Collision collision)
    {
        if (ProceduralGeneration.Instance.spawnedObjects.Contains(collision.gameObject))
        {
            ProceduralGeneration.Instance.spawnedObjects.Remove(collision.gameObject);

            collision.gameObject.SetActive(false);
        }
        else if (ProceduralGeneration.Instance.spawnedBlocks.Contains(collision.gameObject))
        {
            ProceduralGeneration.Instance.spawnedBlocks.Remove(collision.gameObject);

            collision.gameObject.SetActive(false);
        }
    }
}
