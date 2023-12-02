using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private float orthographicSizeMin = 6;
    [SerializeField] private float orthographicSizeMax = 8.5f;
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float targetOrthographicSize;

    private void Start()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        Zoom();
    }

    private void Zoom()
    {
        if (Input.mouseScrollDelta.y < 0)
            targetOrthographicSize += 1;
        if (Input.mouseScrollDelta.y > 0)
            targetOrthographicSize -= 1;

        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, orthographicSizeMin, orthographicSizeMax);

        float zoomSpeed = 10f;
        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
    }
}
