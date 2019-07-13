using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake_S : MonoBehaviour
{

    public Camera mainCam;

    float shakeAmount = 0;

    void Awake()
    {
        if (mainCam == null)
            mainCam = Camera.main;
    }


    // ** Methods **
    public void ShakeCamera(float amount, float length)
    {
        shakeAmount = amount;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", length);
    }


    void DoShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = mainCam.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            camPos.x += offsetX;
            camPos.y += offsetY;

            mainCam.transform.position = camPos;
        }
    }


    public void setCamera(Camera newCam)
    {
        mainCam = newCam;
    }


    void StopShake()
    {
        CancelInvoke("DoShake");
        mainCam.transform.localPosition = Vector3.zero;
    }
}
