using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
[RequireComponent (typeof(IFF))]

public class CitySquare : MonoBehaviour
{

    //init
    public IFF iff;
    SpriteRenderer sr;
    [SerializeField] GameObject housePrefab = null;
    [SerializeField] GameObject turretPrefab = null;
    AllegianceManager am;

    //param
    public float cityRadius { get; private set; } = 4f;
    int numberOfHousesToSpawn = 6;
    int numberOfTurretsToSpawn = 1;

    //hood
    public string cityName { get; protected set; }
    public List<House> housesInCity = new List<House>();
    public List<House> turretsInCity = new List<House>();

    void Start()
    {
        am = FindObjectOfType<AllegianceManager>();
        sr = GetComponent<SpriteRenderer>();
        SelectCityName();
        SpawnHousesWithinCity(numberOfHousesToSpawn);
        ConvertHousesToTurrets();
        SetAllegianceForBuildingsInCity(iff.GetIFFAllegiance());
    }

    #region creation

    private void SpawnHousesWithinCity(int numberOfHouses)
    {
        Grid grid = FindObjectOfType<Grid>();
        float gridUnit = grid.cellSize.x;
        for (int i = 0; i < numberOfHouses; i++)
        {
            Vector3 actualPos = Vector3.zero;
            do
            {
                Vector3 gridSnappedPos = Vector3.zero;
                Vector2 pos = UnityEngine.Random.insideUnitCircle * cityRadius;
                Vector3 pos3 = pos;
                gridSnappedPos = new Vector3(Mathf.Round(pos.x / gridUnit), Mathf.Round(pos.y / gridUnit), 0);
                //if (Mathf.Abs(gridSnappedPos.x) < cityMinDistFromSquare)
                //{
                //    float sign = Mathf.Sign(gridSnappedPos.x);
                //    gridSnappedPos.x = cityMinDistFromSquare * sign;
                //}
                //if (Mathf.Abs(gridSnappedPos.y) < cityMinDistFromSquare)
                //{
                //    float sign = Mathf.Sign(gridSnappedPos.y);
                //    gridSnappedPos.y = cityMinDistFromSquare * sign;
                //}

                //Vector3 halfStep = (new Vector3(1, 1, 1)) * gridUnit / 2f;
                actualPos = transform.position + gridSnappedPos;
                //Debug.Log($"generated pos: {pos}, which is dist {(transform.position - pos3).magnitude} and {gridSnappedPos} is gsp.  ActualPos is {actualPos}. Distance is {(transform.position - actualPos).magnitude}");

            }
            while (!(IsTestLocationValid_NavMesh(actualPos) & IsTestLocationValid_Physics(actualPos)));

            GameObject newHouse = Instantiate(housePrefab, actualPos, housePrefab.transform.rotation) as GameObject;
            House house = newHouse.GetComponent<House>();
            housesInCity.Add(house);
            house.am = am;
            house.SetOwningCity(this);
        }
    }

    private void ConvertHousesToTurrets()
    {
        for (int i = 0; i < numberOfTurretsToSpawn; i++)
        {
            if (housesInCity.Count == 0) { return; }
            int random = UnityEngine.Random.Range(0, housesInCity.Count);
            GameObject houseToReplace = housesInCity[random].gameObject;
            GameObject newTurret = Instantiate(turretPrefab, houseToReplace.transform.position, turretPrefab.transform.rotation) as GameObject;
            House turret = newTurret.GetComponent<House>();
            turret.am = am;
            turret.SetOwningCity(this);
            turretsInCity.Add(turret);
            housesInCity.Remove(houseToReplace.GetComponent<House>());
            Destroy(houseToReplace);
        }
    }
    private bool IsTestLocationValid_Physics(Vector3 testPos)
    {
        Collider2D rchit = Physics2D.OverlapCircle(testPos, 0.3f, 1 << 8);
        if (rchit)
        {
            //Debug.Log($"invalid due to physics at {rchit.transform.position} on {rchit.transform.gameObject.name}");
            return false;
        }
        else
        {
            //Debug.Log("physics is good");
            return true;
        }
    }
    private bool IsTestLocationValid_NavMesh(Vector3 testPos)
    {
        NavMeshHit hit;
        NavMeshQueryFilter filter = new NavMeshQueryFilter();
        filter.areaMask = NavMesh.AllAreas;
        filter.agentTypeID = GameObject.FindGameObjectWithTag("NavMeshSurface").GetComponent<NavMeshSurface2d>().agentTypeID;
        NavMesh.SamplePosition(testPos, out hit, 0.1f, filter);
        bool[] layersFound = LayerMaskExtensions.HasLayers(hit.mask);

        if (layersFound[0])
        {
            //Debug.Log($"0 is good at {testPos}");
            return true;
        }
        else
        {
            //Debug.Log($"Invalid at {testPos}");
            return false;
        }

    }
    private void SelectCityName()
    {
        CityManager cnm = FindObjectOfType<CityManager>();
        cityName = cnm.GetRandomCityName();
    }

    public void SetAllegianceForBuildingsInCity(int newIFF)
    {
        //Debug.Log($"{housesInCity.Count} houses should be changing iff state to {iff.GetIFFAllegiance()}");
        //Debug.Log($"{turretsInCity.Count} turrets should be changing iff state to {iff.GetIFFAllegiance()}");
        foreach (House house in housesInCity)
        {
            house.SetHouseIFFAllegiance(newIFF);
            house.UpdateCurrentOwner();
        }
        foreach (House turret in turretsInCity)
        {
            turret.SetHouseIFFAllegiance(newIFF);
            turret.UpdateCurrentOwner();
        }
    }
    #endregion  

    // Update is called once per frame
    void Update()
    {

    }
    



    public void RemoveBuildingFromList(House deadThing)
    {
        housesInCity.Remove(deadThing);
        turretsInCity.Remove(deadThing);
    }
}
