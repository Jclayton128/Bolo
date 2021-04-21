using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarSector : MonoBehaviour
{
    //init
    [SerializeField] Image[] dotLevels = null;

    //param
    Color radarGreen = new Color(0.2195399f, 0.95f, .2134212f);
    Color radarYellow = Color.yellow;
    Color radarRed = Color.red;

    //hood
    float intensityActual;
    public float intensityTarget;
    public float fadeRate;
    public float riseRate;


    // Start is called before the first frame update
    void Start()
    {
        SetAllDotsToZero();
    }

    private void SetAllDotsToZero()
    {
        foreach (Image dot in dotLevels)
        {
            dot.color = new Color(0.2195399f, 0.95f, .2134212f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DrainActualIntensityToMatchTargetIntensity();
        IlluminateDotsBasedOnIntensity();
    }

    private void DrainActualIntensityToMatchTargetIntensity()
    {
        if (intensityActual < intensityTarget)
        {
            intensityActual = Mathf.MoveTowards(intensityActual, intensityTarget, riseRate * Time.deltaTime);
        }
        if (intensityActual > intensityTarget)
        {
            intensityActual = Mathf.MoveTowards(intensityActual, intensityTarget, fadeRate * Time.deltaTime);
        }


    }

    private void IlluminateDotsBasedOnIntensity()
    {
        float alpha_0 = (intensityActual - 0) / .2f;
        float alpha_1 = (intensityActual - .20f) / .2f;
        float alpha_2 = (intensityActual - .4f) / .2f;
        float alpha_3 = (intensityActual - .6f) / .2f;
        float alpha_4 = (intensityActual - .8f) / .2f;
        dotLevels[0].color = new Color(radarGreen.r, radarGreen.g, radarGreen.b, alpha_0);
        dotLevels[1].color = new Color(radarGreen.r, radarGreen.g, radarGreen.b, alpha_1);
        dotLevels[2].color = new Color(radarGreen.r, radarGreen.g, radarGreen.b, alpha_2);
        dotLevels[3].color = new Color(radarYellow.r, radarYellow.g, radarYellow.b, alpha_3);
        dotLevels[4].color = new Color(radarRed.r, radarRed.g, radarRed.b, alpha_4);
    }
}
