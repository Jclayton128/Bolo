using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarScreen : MonoBehaviour
{
    //init
    GameObject player;
    UnitTracker ut;
    [SerializeField] RadarSector[] radarSectors = null;
    public List<GameObject> targets = new List<GameObject>();
    Dictionary<int, float> sectorIntensities = new Dictionary<int, float>();



    //param
    public float radarRange = 20;
    public float fadePerSecond = 0.3f;
    public float risePerSecond = 1.0f;
    public float signalIntensity;
    public float radarAccuracy; //how far off can the direction-of-arrival be, in degrees.

    //hood


    // Start is called before the first frame update
    void Start()
    {
        ut = FindObjectOfType<UnitTracker>();
        player = GameObject.FindGameObjectWithTag("Player"); //TODO multiplayer this
        //ResetAllSectorsToZero();
        PopulateSectorIntensitieswithZero();

    }

    private void SetFadeTimeInEachSector()
    {
        foreach (RadarSector rs in radarSectors)
        {
            rs.fadeRate = fadePerSecond;
            rs.riseRate = risePerSecond;
        }
    }

    private void PopulateSectorIntensitieswithZero()
    {
        for (int i = 0; i < 8; i++)
        {
            sectorIntensities.Add(i, 0);
        }
    }



    // Update is called once per frame
    void Update()
    {
        SetFadeTimeInEachSector();
        ResetSectorIntensityToZero();
        GetTargets();
        IncreaseIntensityFromNoiseInEachSector();
        //ConstantIntensityDecreaseForEachSector();
        AssignCurrentIntensityToEachSector();
        //TurnIntensityIntoImageAdjustment();
    }

    private void ResetSectorIntensityToZero()
    {
        for (int i = 0; i < 8; i++)
        {
            sectorIntensities[i] = 0;
        }
    }

    private void AssignCurrentIntensityToEachSector()
    {
        for (int i = 0; i < 8; i++)
        {
            radarSectors[i].intensityTarget = sectorIntensities[i];
        }
    }

    private void IncreaseIntensityFromNoiseInEachSector()
    {
        foreach (GameObject target in targets)
        {
            Vector3 dir = target.transform.position - player.transform.position;
            float signedAngFromNorth = Vector3.SignedAngle(dir, Vector3.up, Vector3.forward) - 22.5f ;
            if (signedAngFromNorth < 0)
            {
                signedAngFromNorth += 360;
            }

            //float randomSpread = 0;
            float randomSpread = UnityEngine.Random.Range(-radarAccuracy, radarAccuracy);
            signedAngFromNorth += randomSpread;

            float approxSector = (signedAngFromNorth /45 );
            int sector = Mathf.RoundToInt(approxSector);

            if (sector >= 8)
            {
                sector = 0;
            }
            if (sector < 0)
            {
                sector = 7;
            }
            Debug.Log("angleFromNorth: " + signedAngFromNorth + " goes into approxSector: " + approxSector + " rounds to: " + sector);
            //TODO: Get the current target's noise and add it instead of the arbitrary value;
            sectorIntensities[sector] = sectorIntensities[sector] + signalIntensity;
        }

    }

    private void GetTargets()
    {
        targets = ut.FindUnitsWithinSearchRange(player, radarRange);
    }

    private void ConstantIntensityDecreaseForEachSector()
    {
        for (int i = 0; i < 8; i++)
        {
            sectorIntensities[i] -= fadePerSecond * Time.deltaTime;
            sectorIntensities[i] = Mathf.Clamp01(sectorIntensities[i]);
        }
    }
}
