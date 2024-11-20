using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShaking : MonoBehaviour
{
    private static CameraShaking instance;
    public static CameraShaking Instance => instance;

    private CameraController _cameraController;
    private float shakeTime;
    private float shakeIntensity;

    private void Awake()
    {
        _cameraController = GetComponent<CameraController>();
    }

    public CameraShaking()
    {
        instance = this;
    }
    
    public void OnShakeCamera(float shakeTime = 1.0f, float shakeIntensity = 0.1f)
    {
        Debug.Log("카메라 흔들기");
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;
        
        StopCoroutine("ShakeByPosition");
        StartCoroutine("ShakeByPosition");
    }

    private IEnumerator ShakeByPosition()
    {
        _cameraController.IsOnShake = true;
        
        Vector3 startPosition = transform.position;

        while (shakeTime > 0.0f)
        {
            transform.position = startPosition + Random.insideUnitSphere * shakeIntensity;

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = startPosition;

        _cameraController.IsOnShake = false;
    }
    
}
