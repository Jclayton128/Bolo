using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CaptureSite : MonoBehaviour
{
    //param
    float timeToCapture = 5f;

    //hood
    float timeSpentCapturing = 0;
    CaptureTool capturer;
    bool isCaptured = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckIfCaptured();
    }

    private void CheckIfCaptured()
    {
        if (timeSpentCapturing >= timeToCapture)
        {
            isCaptured = true;
        }
    }

    public bool GetCapturedStatus()
    {
        return isCaptured;
    }

    public void ResetCaptureStatus()
    {
        isCaptured = false;
        timeSpentCapturing = 0;
        capturer = null;
    }

    public void BuildCaptureTime(float amount)
    {
        timeSpentCapturing += amount;
    }

    public float GetTimeSpentCapturing()
    {
        return timeSpentCapturing;
    }

    public float GetTimeRequiredToCapture()
    {
        return timeToCapture;
    }
    public void SetCapturer(CaptureTool captool)
    {
        capturer = captool;
    }
}
