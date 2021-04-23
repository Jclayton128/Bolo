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
    public MoneyHolder ownerMoneyHolder = null;
    [SerializeField] GameObject housePrefab = null;
    [SerializeField] GameObject turretPrefab = null;

    //param
    public float cityRadius { get; private set; } = 2f;
    float cityMinDistFromSquare = 0f;
    public float timeToCapture = 15; //seconds
    public float timeBetweenMoneyDrops = 5f;
    public int numberOfHousesToSpawn = 6;
    public int numberOfTurretsToSpawn = 1;

    //hood
    public string cityName { get; protected set; }
    public List<House> buildingsInCity = new List<House>();
    public float timeSpentCapturing = 0;
    public int iffOfPreviousCaptureAttempt = 0;
    public GameObject capturingGO = null;

    float timeSinceLastMoneyDrop = 0;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        iff = GetComponentInChildren<IFF>();
        SelectCityName();
        SpawnHousesWithinCity(numberOfHousesToSpawn);
        //ConvertHousesToTurrets();


        //FindBuildingsWithinCity();
        //SetAllegianceForBuildingsInCity(iff.GetIFFAllegiance());       
    }

    private void SpawnHousesWithinCity(int numberOfHouses)
    {
        Grid grid = FindObjectOfType<Grid>();
        float gridUnit = grid.cellSize.x;
        for(int i = 0; i< numberOfHouses; i++)
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
                Debug.Log($"generated pos: {pos}, which is dist {(transform.position- pos3).magnitude} and {gridSnappedPos} is gsp.  ActualPos is {actualPos}. Distance is {(transform.position - actualPos).magnitude}");

            }
            while (IsTestLocationValid_NavMesh(actualPos) == false && IsTestLocationValid_Physics(actualPos) == false);

            GameObject newHouse = Instantiate(housePrefab, actualPos, housePrefab.transform.rotation) as GameObject;
            
        }
    }

    private bool IsTestLocationValid_Physics(Vector3 testPos)
    {
        Collider2D rchit = Physics2D.OverlapCircle(testPos, 0.5f, 1 << 8);
        if (rchit)
        {
            Debug.Log($"invalid due to physics at {rchit.transform.position} on {rchit.transform.gameObject.name}");
            return false;
        }
        else
        {
            Debug.Log("physics is good");
            return true;
        }
    }

    private bool IsTestLocationValid_NavMesh(Vector3 testPos)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(testPos, out hit, 1.0f, NavMesh.AllAreas);
        bool[] layersFound = LayerMaskExtensions.HasLayers(hit.mask);

        if (layersFound[3])
        {
            Debug.Log($"3 is good at {testPos}");
            return true;
        }
        else
        {
            Debug.Log($"Invalid at {testPos}");
            return false;
        }

    }






    private void SelectCityName()
    {
        CityManager cnm = FindObjectOfType<CityManager>();
        cityName = cnm.GetRandomCityName();
    }

    //private void FindBuildingsWithinCity()
    //{
    //    buildingsInCity.Clear();
    //    List<GameObject> allGOs = Finder.FindAllGameObjectsWithinSearchRange(transform, cityRadius);
    //    foreach (GameObject currentHouseGO in allGOs)
    //    {
    //        House currentHouse;
    //        if (currentHouseGO.TryGetComponent<House>(out currentHouse) == false) { continue; }
    //        buildingsInCity.Add(currentHouse);
    //        currentHouse.SetOwningCity(this);
    //    }
    //}

    //private void SetAllegianceForBuildingsInCity(int newIFF)
    //{
    //    foreach (House house in buildingsInCity)
    //    {
    //        house.SetHouseIFFAllegiance(newIFF);
    //    }
    //}


    // Update is called once per frame
    void Update()
    {
        ReduceCaptureTimeIfNotBeingCaptured();
        HandleCaptureAttempt();
        //ProvideMoneyDropToOwner();
    }
    private void ProvideMoneyDropToOwner()
    {
        timeSinceLastMoneyDrop -= Time.deltaTime;
        if (timeSinceLastMoneyDrop <= 0)
        {
            if (!ownerMoneyHolder) { return; }
            ownerMoneyHolder.AddMoney(buildingsInCity.Count);
            timeSinceLastMoneyDrop = timeBetweenMoneyDrops;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        capturingGO = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        capturingGO = null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!capturingGO) { return; }
        if (capturingGO.GetComponentInParent<IFF>().GetIFFAllegiance() != iff.GetIFFAllegiance() && timeSpentCapturing < timeToCapture)
        {
            timeSpentCapturing += Time.deltaTime;
        }
    }

    private void HandleCaptureAttempt()
    {
        if (!capturingGO) { return; }

        if (timeSpentCapturing >= timeToCapture)
        {
            CompleteSuccessfulCapture();
        }
    }

    private void CompleteSuccessfulCapture()
    {
        int newAllegiance = capturingGO.GetComponentInParent<IFF>().GetIFFAllegiance();
        iff.SetIFFAllegiance(newAllegiance);
        ownerMoneyHolder = capturingGO.transform.root.GetComponentInChildren<MoneyHolder>();

        //FindBuildingsWithinCity();
        //SetAllegianceForBuildingsInCity(newAllegiance);
        timeSpentCapturing = 0;
    }

    private void ReduceCaptureTimeIfNotBeingCaptured()
    {
        if (!capturingGO)
        {
            timeSpentCapturing -= Time.deltaTime * 2;
            timeSpentCapturing = Mathf.Clamp(timeSpentCapturing, 0, timeToCapture);
        }
    }

    public void RemoveBuildingFromList(House deadThing)
    {
        buildingsInCity.Remove(deadThing);
    }
}
