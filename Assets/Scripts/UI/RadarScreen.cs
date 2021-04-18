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
    [SerializeField] Image[] sectorImages = null;
    public List<GameObject> targets = new List<GameObject>();
    Dictionary<int, float> sectorIntensities = new Dictionary<int, float>();


    //param
    public float radarRange = 20;
    public float fadePerSecond = 0.3f;
    public float arbitraryTestValue = .4f;
    public float radarAccuracy = 23f; //how far off can the direction-of-arrival be, in degrees.

    //hood


    // Start is called before the first frame update
    void Start()
    {
        ut = FindObjectOfType<UnitTracker>();
        player = GameObject.FindGameObjectWithTag("Player"); //TODO multiplayer this
        ResetAllSectorsToZero();
        PopulateSectorIntensitieswithZero();
    }


    private void PopulateSectorIntensitieswithZero()
    {
        for (int i = 0; i < 8; i++)
        {
            sectorIntensities.Add(i, 0);
        }
    }

    private void ResetAllSectorsToZero()
    {
        foreach (Image image in sectorImages)
        {
            image.color = Color.clear;
        }
    }


    // Update is called once per frame
    void Update()
    {
        GetTargets();
        GetNoiseFromTargetsInEachSectors();
        ConstantIntensityDecreaseForEachSector();
        TurnIntensityIntoImageAdjustment();
    }

    private void TurnIntensityIntoImageAdjustment()
    {
        for (int i = 0; i < 8; i++)
        {
            sectorImages[i].color = new Color(1,1,1,sectorIntensities[i]);
        }
    }

    private void GetNoiseFromTargetsInEachSectors()
    {
        foreach (GameObject target in targets)
        {
            Vector3 dir = target.transform.position - player.transform.position;
            float signedAngFromNorth = Vector3.SignedAngle(dir, Vector3.up, Vector3.forward);
            if (signedAngFromNorth < 0)
            {
                signedAngFromNorth += 360;
            }

            float randomSpread = UnityEngine.Random.Range(-radarAccuracy, radarAccuracy);
            signedAngFromNorth += randomSpread;

            float approxSector = signedAngFromNorth / 45;
            int sector = Mathf.RoundToInt(approxSector);
            //Get the current target's noise and add it instead of the arbitrary value;
            sectorIntensities[sector-1] += arbitraryTestValue;
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
