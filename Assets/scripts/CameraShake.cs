using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    static public CameraShake instance;
    CinemachineCamera virtualCamera;
    float shakeTimer;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        virtualCamera = GetComponent<CinemachineCamera>();
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                StopShaking();
            }
        }
    }


    public void ShakeCamera(float _duration, float _magnitude, float _frequency)
    {
        CinemachineBasicMultiChannelPerlin multuPerlin = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        multuPerlin.AmplitudeGain = _magnitude;
        multuPerlin.FrequencyGain= _frequency;
        shakeTimer = _duration;
    }

    void StopShaking()
    {
        CinemachineBasicMultiChannelPerlin multuPerlin = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        multuPerlin.AmplitudeGain = 0;
    }
}
