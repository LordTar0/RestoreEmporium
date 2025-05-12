using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clean : MonoBehaviour
{
    public Material newMaterial; 
    public string targetTag = "tool"; 
    private bool hasChanged = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasChanged) return;

        if (collision.gameObject.CompareTag(targetTag))
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && newMaterial != null)
            {
                renderer.material = newMaterial;
                hasChanged = true;
            }
        }
    }
}

