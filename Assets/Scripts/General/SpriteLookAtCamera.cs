using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLookAtCamera : MonoBehaviour
{
    private Transform cam;


    private void Awake()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        var dir = cam.position - transform.position;
        transform.forward = dir.normalized;
    }
}
