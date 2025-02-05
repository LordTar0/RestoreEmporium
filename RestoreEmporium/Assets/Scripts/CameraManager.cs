using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Camera cam;


    private void Awake()
    {
        if(!cam) cam = GetComponentInChildren<Camera>();
    }
}
