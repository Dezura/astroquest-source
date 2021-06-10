using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void ApplyScreenShake(float amount)
    {
        StopCoroutine("ReduceShakeCoroutine");
        cameraShake.m_AmplitudeGain += amount;
        cameraShake.m_FrequencyGain += amount;
        StartCoroutine("ReduceShakeCoroutine");
    }

    private IEnumerator ReduceShakeCoroutine()
    {
        yield return new WaitUntil(ReducedShake); 
    }

    bool ReducedShake() // This design is very weird, but the WaitUntil in the ReduceShakeCoroutine() demands this type of design
    {
        cameraShake.m_AmplitudeGain = Mathf.Lerp(cameraShake.m_AmplitudeGain, 0, 0.025f);
        cameraShake.m_FrequencyGain = Mathf.Lerp(cameraShake.m_FrequencyGain, 0, 0.025f);

        if (cameraShake.m_AmplitudeGain == 0 && cameraShake.m_FrequencyGain == 0) return true;

        return false;
    }
}
