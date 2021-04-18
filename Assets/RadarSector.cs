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
    Color myGreen = new Color(0.2195399f, 0.95f, .2134212f);

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
        float alpha_0 = (intensityActual - 0) / .25f;
        float alpha_1 = (intensityActual - .25f) / .25f;
        float alpha_2 = (intensityActual - .5f) / .25f;
        float alpha_3 = (intensityActual - .75f) / .25f;
        dotLevels[0].color = new Color(0.2195399f, 0.95f, .2134212f, alpha_0);
        dotLevels[1].color = new Color(0.2195399f, 0.95f, .2134212f, alpha_1);
        dotLevels[2].color = new Color(0.2195399f, 0.95f, .2134212f, alpha_2);
        dotLevels[3].color = new Color(0.2195399f, 0.95f, .2134212f, alpha_3);
    }
}
