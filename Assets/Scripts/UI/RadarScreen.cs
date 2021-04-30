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
    UIManager uim;



    //param
    public float timeBetweenScans;
    public float radarRange;
    public float fadePerSecond;
    public float risePerSecond;
    //public float signalIntensity;
    public float signalFudge;
    public float radarAccuracy; //how far off can the direction-of-arrival be, in degrees.

    //hood
    float timeSinceLastScan = 0;


    // Start is called before the first frame update
    void Start()
    {
        ut = FindObjectOfType<UnitTracker>();
        player = GameObject.FindGameObjectWithTag("Player"); //TODO multiplayer this

        PopulateSectorIntensitieswithZero();
        SetFadeTimeInEachSector();

    }


    private void PopulateSectorIntensitieswithZero()
    {
        for (int i = 0; i < 8; i++)
        {
            sectorIntensities.Add(i, 0);
        }
    }
    private void SetFadeTimeInEachSector()
    {
        foreach (RadarSector rs in radarSectors)
        {
            rs.SetRates(risePerSecond, fadePerSecond);
        }
    }



    // Update is called once per frame
    void Update()
    {
        timeSinceLastScan += Time.deltaTime;
        if (timeSinceLastScan >= timeBetweenScans)
        {
            ResetSectorIntensityToZero();
            GetTargets();
            IncreaseIntensityFromNoiseInEachSector();
            ClampIntensityLevelFloorToSelfNoiseInEachSector();
            AssignCurrentIntensityToEachSector();
            timeSinceLastScan = 0;
        }

    }

    private void ClampIntensityLevelFloorToSelfNoiseInEachSector()
    {
        float rawNoiseLevel = player.GetComponentInChildren<StealthHider>().gameObject.GetComponent<CircleCollider2D>().radius;
        float selfNoiseLevel = (rawNoiseLevel - .5f) / 4f;
        for (int i = 0; i < 8; i++)
        {
            sectorIntensities[i] = Mathf.Clamp(sectorIntensities[i], selfNoiseLevel, 1);
        }

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
            radarSectors[i].SetIntensityLevel(sectorIntensities[i]);
        }
    }

    private void IncreaseIntensityFromNoiseInEachSector()
    {
        foreach (GameObject target in targets)
        {
            int sector = DetermineSector(target);
            //Debug.Log("angleFromNorth: " + signedAngFromNorth + " goes into approxSector: " + approxSector + " rounds to: " + sector);

            float signalIntensity = DetermineSignalIntensity(target);
            
            //TODO: Get the current target's noise and add it instead of the arbitrary value;
            sectorIntensities[sector] = sectorIntensities[sector] + signalIntensity;
        }

    }


    private float DetermineSignalIntensity(GameObject target)
    {
        float dist = (target.transform.position - player.transform.position).magnitude;
        float dist_normalized = dist / radarRange;
        float targetNoiseLevel = target.GetComponentInChildren<StealthHider>().gameObject.GetComponent<CircleCollider2D>().radius;
        Debug.Log($"{target} is making {targetNoiseLevel} noise");
        float intensity = targetNoiseLevel / (dist_normalized) * signalFudge;
        //Debug.Log("intensity: " + intensity);
        return intensity;

    }

    private int DetermineSector(GameObject target)
    {
        Vector3 dir = target.transform.position - player.transform.position;
        float signedAngFromNorth = Vector3.SignedAngle(dir, Vector3.up, Vector3.forward) - 22.5f;
        if (signedAngFromNorth < 0)
        {
            signedAngFromNorth += 360;
        }

        signedAngFromNorth = InputRandomSignalSpread(signedAngFromNorth);

        float approxSector = (signedAngFromNorth / 45);
        int sector = Mathf.RoundToInt(approxSector);

        if (sector >= 8)
        {
            sector = 0;
        }
        if (sector < 0)
        {
            sector = 7;
        }

        return sector;
    }

    private float InputRandomSignalSpread(float signedAngFromNorth)
    {
        float randomSpread = UnityEngine.Random.Range(-radarAccuracy, radarAccuracy);
        signedAngFromNorth += randomSpread;
        return signedAngFromNorth;
    }

    private void GetTargets()
    {
        targets = ut.FindUnitsWithinSearchRange(player, radarRange);  //TODO this ut function needs to return Turrets
    }

}
