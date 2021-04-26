using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureTool : MonoBehaviour
{
    //init
    public Slider cityCaptureSlider;
    public CaptureSite capSite;

    //param

    //hood
    float timeSpentCapturing;

    void Start()
    {
        cityCaptureSlider = FindObjectOfType<UIManager>().GetCityCaptureSlider(transform.root.gameObject);
    }

    public void Reinitialize()
    {
        Start();
    }

    private void Update()
    {
        
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
            capSite = null;
        }

    }

    private void UpdateUI()
    {
        if (!cityCaptureSlider) { return; }
        cityCaptureSlider.maxValue = capSite.GetTimeRequiredToCapture();
        cityCaptureSlider.value = capSite.GetTimeSpentCapturing();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!capSite) { return; }
        capSite.ResetCaptureStatus();
        capSite.SetCapturer(null);
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (!capSite) { return; }
        capSite.ResetCaptureStatus();
        capSite.SetCapturer(null);
    }


}
