using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTracker : MonoBehaviour
{
    //init
    public List<GameObject> targetableUnits = new List<GameObject>();
    AllegianceManager am;
    private void Awake()
    {
        int utCount = FindObjectsOfType<UnitTracker>().Length;
        if (utCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        am = FindObjectOfType<AllegianceManager>();
    }

    public void AddUnitToTargetableList(GameObject unit)
    {
        if (unit.GetComponentInChildren<IFF>())
        {
            targetableUnits.Add(unit);
        }
        else
        {
            Debug.Log("Can't add a targetable unit if it doesn't have an IFF");
        }

    }

    public void RemoveUnitFromTargetableList(GameObject unit)
    {
        targetableUnits.Remove(unit);
    }

    public List<GameObject> FindTargetsWithinSearchRange(GameObject callingGameObject, float searchRange)
    {
        List<GameObject> unitsWithinRange = new List<GameObject>();

        foreach(GameObject unit in targetableUnits)
        {
            if (unit == callingGameObject) { continue; }
            float diff = (callingGameObject.transform.position - unit.transform.position).magnitude;
            if (diff <= searchRange)
            {
                unitsWithinRange.Add(unit);
            }
        }
        return unitsWithinRange;
    }

    public List<GameObject> FindTargetsWithinSearchRange(GameObject callingGameObject, float searchRange, int allegianceToIgnore)
    {
        List<GameObject> unitsWithinRange = new List<GameObject>();

        foreach (GameObject unit in targetableUnits)
        {
            if (unit == callingGameObject) { continue; }
            if (unit.GetComponentInChildren<IFF>().GetIFFAllegiance() == allegianceToIgnore ) { continue; }
            float diff = (callingGameObject.transform.position - unit.transform.position).magnitude;
            if (diff <= searchRange)
            {
                unitsWithinRange.Add(unit);
            }
        }
        return unitsWithinRange;
    }

    public List<GameObject> FindUnitsWithinSearchRange(GameObject callingGameObject, float searchRange, bool includeDefenseTurrets )
    {
        List<GameObject> unitsWithinRange = new List<GameObject>();

        foreach (GameObject unit in targetableUnits)
        {
            if (unit == callingGameObject) { continue; }
            if (includeDefenseTurrets)
            {
                if (unit.transform.root.GetComponent<DefenseTurret>() == false & unit.transform.root.GetComponent<ControlSource>() == false) { continue; }
            }
            if (!includeDefenseTurrets)
            {
                if (unit.transform.root.GetComponent<ControlSource>() == false ) { continue; }
            }

            float diff = (callingGameObject.transform.position - unit.transform.position).magnitude;
            if (diff <= searchRange)
            {
                unitsWithinRange.Add(unit);
            }
        }
        return unitsWithinRange;
    }

    public GameObject FindClosestTargetWithAllegiance(GameObject callingGameObject, float searchRange, int allegianceToFind)
    {
        GameObject closetTarget = null;
        float distance = searchRange;
        foreach (GameObject unit in targetableUnits)
        {
            if (unit == callingGameObject) { continue; }
            if (unit.GetComponentInChildren<IFF>().GetIFFAllegiance() != allegianceToFind) { continue; }

            float diff = (callingGameObject.transform.position - unit.transform.position).magnitude;

            if (diff < distance)
            {
                closetTarget = unit;
                distance = diff;
            }
        }
        return closetTarget;
    }

    public bool TryGetClosestUnitWithinRange(GameObject callingGameObject, float searchRange, int allegianceToFind, out GameObject foundUnit)
    {
        bool foundSomething = false;
        foundUnit = null;
        float distance = searchRange;
        foreach (GameObject unit in targetableUnits)
        {
            if (unit == callingGameObject) { continue; }
            if (unit.GetComponentInChildren<IFF>().GetIFFAllegiance() != allegianceToFind) { continue; }

            float diff = (callingGameObject.transform.position - unit.transform.position).magnitude;

            if (diff < distance)
            {
                foundUnit = unit;
                distance = diff;
            }
        }
        return foundSomething;
    }

    public bool TryGetClosestUnitWithinRange(GameObject callingGameObject, float searchRange, string tagToLookFor, out GameObject foundUnit)
    {
        bool foundSomething = false;
        foundUnit = null;
        float distance = searchRange;
        foreach (GameObject unit in targetableUnits)
        {
            if (unit == callingGameObject) { continue; }
            if (unit.tag != tagToLookFor) { continue; }

            float diff = (callingGameObject.transform.position - unit.transform.position).magnitude;

            if (diff < distance)
            {
                foundUnit = unit;
                distance = diff;
            }
        }
        return foundSomething;
    }

    public bool TryGetClosestUnitWithinRange(GameObject callingGameObject, float searchRange, string tagToLookFor, int allegianceToIgnore, out GameObject foundUnit)
    {
        bool foundSomething = false;
        foundUnit = null;
        float distance = searchRange;
        foreach (GameObject unit in targetableUnits)
        {
            if (unit == callingGameObject) { continue; }
            if (unit.tag != tagToLookFor) { continue; }
            if (unit.GetComponentInChildren<IFF>().GetIFFAllegiance() == allegianceToIgnore) { continue; }

            float diff = (callingGameObject.transform.position - unit.transform.position).magnitude;

            if (diff < distance)
            {
                foundUnit = unit;
                distance = diff;
            }
        }
        return foundSomething;
    }



    public bool TryGetClosestAttackerWithinRange(GameObject callingGameObject, float searchRange, int allegianceToIgnore, out GameObject foundUnit)
    {
        bool foundSomething = false;
        foundUnit = null;
        float distance = searchRange;
        foreach (GameObject unit in targetableUnits)
        {
            if (unit == callingGameObject) { continue; }
            if (unit.GetComponentInChildren<IFF>().GetIFFAllegiance() == allegianceToIgnore) { continue; }
            if (!unit.GetComponentInChildren<Attack>()) { continue; }

            float diff = (callingGameObject.transform.position - unit.transform.position).magnitude;

            if (diff < distance)
            {
                foundSomething = true;
                foundUnit = unit;
                distance = diff;
            }
        }
        return foundSomething;
    }


    public GameObject FindClosestTargetWithinSearchRange(GameObject callingGameObject, float searchRange, int allegianceToIgnore)
    {
        GameObject closetTarget = null;
        float distance = searchRange;
        foreach (GameObject unit in targetableUnits)
        {
            if (unit == callingGameObject) { continue; }
            if (unit.GetComponentInChildren<IFF>().GetIFFAllegiance() == allegianceToIgnore) { continue; }

            float diff = (callingGameObject.transform.position - unit.transform.position).magnitude;

            if (diff < distance)
            {
                closetTarget = unit;
                distance = diff;
            }
        }
        return closetTarget;
    }


}
