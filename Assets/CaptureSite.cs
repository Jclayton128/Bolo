using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CaptureSite : MonoBehaviour
{
    //param
    float timeToCapture = 5f;
    float captureBleedOutRate = 3f; //you lose 3 second of capture every second you aren't on the tile;

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
        ReduceCaptureTimeIfNotBeingCaptured();
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
    }

    private void ReduceCaptureTimeIfNotBeingCaptured()
    {
        if (!capturer)
        {
            timeSpentCapturing -= Time.deltaTime * captureBleedOutRate;
            timeSpentCapturing = Mathf.Clamp(timeSpentCapturing, 0, timeToCapture);
        }
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
