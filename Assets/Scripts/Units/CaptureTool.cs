using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureTool : MonoBehaviour
{
    //init
    Slider cityCaptureSlider;
    CaptureSite capSite;

    //param

    //hood
    float timeSpentCapturing;

    void Start()
    {
        cityCaptureSlider = FindObjectOfType<UIManager>().GetCityCaptureSlider(transform.root.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int collisionIFF = collision.transform.root.GetComponentInChildren<IFF>().GetIFFAllegiance();
        int ownIFF = transform.root.GetComponentInChildren<IFF>().GetIFFAllegiance();
        if ( collisionIFF == ownIFF ) { return; }
        capSite = collision.GetComponent<CaptureSite>();
        capSite.SetCapturer(this);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!capSite) { return; }

        capSite.BuildCaptureTime(Time.deltaTime);
        UpdateUI();

        if (capSite.GetCapturedStatus())
        {
            int newAllegiance = transform.root.GetComponentInChildren<IFF>().GetIFFAllegiance();
            capSite.transform.root.GetComponentInChildren<IFF>().SetIFFAllegiance(newAllegiance);
            capSite.transform.root.GetComponentInChildren<CitySquare>().SetAllegianceForBuildingsInCity(newAllegiance);
            capSite.ResetCaptureStatus();
        }

    }

    private void UpdateUI()
    {
        cityCaptureSlider.maxValue = capSite.GetTimeRequiredToCapture();
        cityCaptureSlider.value = capSite.GetTimeSpentCapturing();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        capSite.SetCapturer(null);
    }

    private void OnDestroy()
    {
        capSite.ResetCaptureStatus();
        capSite.SetCapturer(null);
    }


}
