using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float MinZoom = 1;

    [SerializeField] private float MaxZoom = 8;

    [SerializeField] private float zoomSpeed = 1;
    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        Debug.Assert(camera != null);
    }


    void Update()
    {
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - Input.mouseScrollDelta.y * zoomSpeed, MinZoom, MaxZoom);
    }
}