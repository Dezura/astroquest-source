using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerCamera : Utils
{
    public Camera cam;
    public Cinemachine.CinemachineVirtualCamera vCam;

    Cinemachine.CinemachineFollowZoom followZoom;
    Cinemachine.CinemachineBasicMultiChannelPerlin cameraShake;

    [HideInInspector] public float startingZoom;

    void Start()
    {
        followZoom = vCam.GetComponent<Cinemachine.CinemachineFollowZoom>();
        cameraShake = vCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        startingZoom = followZoom.m_Width;
    }

    void Update()
    {
        ApplyZoom(startingZoom);
    }

    public void ApplyZoom(float amount)
    {
        followZoom.m_Width = amount;
    }

    public void ApplyScreenShake(float amount, float durationSeconds = 0.35f)
    {
        cameraShake.m_AmplitudeGain += amount;
        cameraShake.m_FrequencyGain += amount;
        Timing.RunCoroutine(StopScreenShakeIn(durationSeconds).CancelWith(gameObject));
    }

    public IEnumerator<float> StopScreenShakeIn(float seconds)
    {
        yield return Timing.WaitForSeconds(seconds); 
        cameraShake.m_AmplitudeGain = 0;
        cameraShake.m_FrequencyGain = 0;
    }
}
