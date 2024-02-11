using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : ManagerBehaviour
{
    [SerializeField] private Transform cameraHolder;
    [Space]
    [SerializeField] [Range(0, 1)] private float sensitivity;

    private float angle = 0;
    private Vector2 pressPosition;

    private void Start()
    {
        
    }

    private void Update()
    {
        float increment = 0;
        increment += Input.GetAxis("Horizontal");

        if (Input.GetMouseButtonDown(1))
        {
            pressPosition = Input.mousePosition;
        }

        if(Input.GetMouseButton(1))
        {
            Debug.Log("Right button");

            increment += Mathf.Clamp(Input.mousePosition.x - pressPosition.x, -1, 1);
        }

        angle += increment * Time.deltaTime * sensitivity * 100;
        cameraHolder.rotation = Quaternion.Euler(0, angle, 0);
    }
}
