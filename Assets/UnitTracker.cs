using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTracker : MonoBehaviour
{
    //init
    List<GameObject> targetableUnits = new List<GameObject>();
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

    public void AddUnitToTargetableList(GameObject unit)
    {
        targetableUnits.Add(unit);
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
}
