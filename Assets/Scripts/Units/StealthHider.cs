using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]


public class StealthHider : MonoBehaviour
{
    //init
    [SerializeField] Material mat = null;
    SpriteRenderer[] srs;
    Rigidbody2D rb;
    CircleCollider2D hiderColl;
    ControlSource cs;
    [SerializeField] GameObject sensorGhostPrefab = null;


    //param
    public float hiderRadius_Base;
    public float hiderGrowthRate; //per second;
    public float hiderShrinkRate; //per second;
    public float attackModifier = 3f; //attacking multiplies the size of hiderRadius_Base, 
    public bool isBuilding = false;
    float fadeRateSensorGhost = .2f; // 5 seconds at .2f

    //hood
    public float hiderRadius_Modified;
    public float hiderRadius_TerrainModifier = 1;
    public GameObject sensorGhost;

    // Start is called before the first frame update
    void Start()
    {
        srs = transform.root.GetComponentsInChildren<SpriteRenderer>();
        MakeObjectInvisible();
        rb = transform.root.GetComponentInChildren<Rigidbody2D>();
        hiderColl = GetComponent<CircleCollider2D>();
        hiderColl.radius = hiderRadius_Base;
        cs = transform.root.GetComponentInChildren<ControlSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHiderRadiusInputBasedOnSpeedAndTerrain();
        if (!isBuilding)
        {
            FadeOutSensorGhost();
        }
    }

    private void FadeOutSensorGhost()
    {
        if (!sensorGhost) { return; }
        SpriteRenderer sr = sensorGhost.GetComponent<SpriteRenderer>();
        float a = Mathf.MoveTowards(sr.color.a, 0, fadeRateSensorGhost * Time.deltaTime);
        sr.color = new Color(1, 1, 1, a);
        if (a <= Mathf.Epsilon) { Destroy(sensorGhost); }
    }

    private void UpdateHiderRadiusInputBasedOnSpeedAndTerrain()
    {
        if (!isBuilding)
        {
            float vel = rb.velocity.magnitude;
            hiderRadius_TerrainModifier = GetTerrainModifier();
            hiderRadius_Modified = hiderRadius_Base * vel * hiderRadius_TerrainModifier;
        }
        else
        {
            hiderRadius_Modified = 0;
        }
        AdjustHiderRadius();
    }

    private float GetTerrainModifier()
    {
        int terrain = cs.currentTerrainType;

        if (terrain == 4) //road
        {
            hiderRadius_TerrainModifier = 1.25f;
            //Debug.Log("hiding in road");
            return hiderRadius_TerrainModifier;
        }
        if (terrain == 5)  //hills
        {
            hiderRadius_TerrainModifier = .5f;
            //Debug.Log("hiding in hills");
            return hiderRadius_TerrainModifier;
        }
        if (terrain == 6) //forest
        {
            hiderRadius_TerrainModifier = .75f;
            //Debug.Log("hiding in forest");
            return hiderRadius_TerrainModifier;
        }
        else
        {
            hiderRadius_TerrainModifier = 1;
            //Debug.Log("hiding in field");
            return hiderRadius_TerrainModifier;
        }


    }

    public void SpikeLoudnessDueToAttack()
    {

        hiderColl.radius = attackModifier * hiderRadius_Base;
        Debug.Log("Spike the stealth!");
    }
    private void AdjustHiderRadius()
    {
        //Debug.Log($"coll mod {hiderRadius_Modified} and hider rad {hiderColl.radius}");
        if (hiderRadius_Modified > hiderColl.radius)
        {
            //Debug.Log("hider radius needs to grow");
            hiderColl.radius += hiderGrowthRate * Time.deltaTime;
        }
        if (hiderRadius_Modified < hiderColl.radius)
        {
            //Debug.Log("hider radius needs to shrink");
            hiderColl.radius -= hiderShrinkRate * Time.deltaTime;
        }
        if (!isBuilding)
        {
            hiderColl.radius = Mathf.Clamp(hiderColl.radius, hiderRadius_Base / 4, hiderRadius_Base * attackModifier * hiderRadius_TerrainModifier);
        }
        if (isBuilding)
        {
            hiderColl.radius = Mathf.Clamp(hiderColl.radius, 0, attackModifier * hiderRadius_Base);
        }

    }

    public void MakeObjectInvisible()
    {
        if (isBuilding) { return; }
        if (transform.root.tag != "Player")
        {
            sensorGhost = CreateSensorGhost();
            foreach (SpriteRenderer thisSR in srs)
            {
                thisSR.enabled = false;
            }
        }
    }

    private GameObject CreateSensorGhost()
    {
        Debug.Log("Create sensor ghost");
        if (sensorGhost)
        {
            //Debug.Log($"Destroying {sensorGhost}");
            Destroy(sensorGhost);
        }
        float z = transform.root.GetComponentInChildren<Rigidbody2D>().rotation;
        Quaternion currentRot = Quaternion.Euler(0, 0, z);
        GameObject sg = Instantiate(sensorGhostPrefab, transform.position, currentRot) as GameObject;
        SpriteRenderer sr = sg.GetComponent<SpriteRenderer>();
        sr.sprite = srs[0].sprite;
        sr.material = mat;
        return sg;
    }

    public void MakeObjectVisible()
    {
        if (isBuilding) { return; }
        if (sensorGhost != null)
        {
            Destroy(sensorGhost);
        }

        foreach (SpriteRenderer thisSR in srs)
        {
            thisSR.enabled = true;
        }
    }

    private void OnDestroy()
    {
       if (sensorGhost)
        {
            Destroy(sensorGhost);
        }
    }
}
