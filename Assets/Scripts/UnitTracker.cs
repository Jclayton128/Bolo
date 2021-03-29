using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTracker : MonoBehaviour
{
    //init
    List<GameObject> targetableUnits = new List<GameObject>();
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

    public List<GameObject> FindTargetsWithinSearchRange(GameObject callingGameObject, float searchRange, int ownAllegiance)
    {
        List<GameObject> unitsWithinRange = new List<GameObject>();

        foreach (GameObject unit in targetableUnits)
        {
            if (unit == callingGameObject) { continue; }
            if (unit.GetComponentInChildren<IFF>().GetIFFAllegiance() == ownAllegiance ) { continue; }
            float diff = (callingGameObject.transform.position - unit.transform.position).magnitude;
            if (diff <= searchRange)
            {
                unitsWithinRange.Add(unit);
            }
        }
        return unitsWithinRange;
    }

    public GameObject FindClosestTargetWithinSearchRange(GameObject callingGameObject, float searchRange, int ownAllegiance)
    {
        GameObject closetTarget = null;
        float distance = searchRange;
        foreach (GameObject unit in targetableUnits)
        {
            if (unit == callingGameObject) { continue; }
            if (unit.GetComponentInChildren<IFF>().GetIFFAllegiance() == ownAllegiance) { continue; }

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
