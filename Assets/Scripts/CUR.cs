using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class CUR : object
{
    static public Vector3 CreateRandomPointNearInputPoint(Vector3 center, float offsetDistance, float offsetRandomFactor)
    {
        float ang = UnityEngine.Random.value * 360;
        float random = offsetDistance + UnityEngine.Random.Range(-offsetRandomFactor, offsetRandomFactor);
        Vector3 pos;
        pos.x = center.x + random * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + random * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        Debug.DrawLine(center, pos, Color.red, 1.0f);
        return pos;
    }


    static public GameObject GetNearestGameObjectWithTag(Transform posToSearchFrom, string tagName)
    {
        GameObject closestTarget = null;
        GameObject[] possibleTargets;
        possibleTargets = GameObject.FindGameObjectsWithTag(tagName);
        float distance = Mathf.Infinity;
        Vector3 position = posToSearchFrom.position;
        foreach (GameObject currentTargetBeingEvaluated in possibleTargets)
        {
            Vector3 diff = currentTargetBeingEvaluated.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closestTarget = currentTargetBeingEvaluated;
                distance = curDistance;
            }
        }
        return closestTarget;
    }

    static public GameObject GetNearestGameObjectWithTag(Transform posToSearchFrom, string tagName, string componentToExcludeFromSearch)
    {

        GameObject closestTarget = null;
        GameObject[] possibleTargets;
        possibleTargets = GameObject.FindGameObjectsWithTag(tagName);
        float distance = Mathf.Infinity;
        Vector3 position = posToSearchFrom.position;
        foreach (GameObject currentTargetBeingEvaluated in possibleTargets)
        {
            if (IterateThroughChildrenLookingForComponent(currentTargetBeingEvaluated, componentToExcludeFromSearch) == true)
            {
                continue; //if the examined GO has that component, then skip it.
            }

            Vector3 diff = currentTargetBeingEvaluated.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closestTarget = currentTargetBeingEvaluated;
                distance = curDistance;
            }
        }
        return closestTarget;
    }

    static private bool IterateThroughChildrenLookingForComponent(GameObject go, string component)
    {
        int childrenCount = go.transform.childCount;
        if (childrenCount == 0) { return false; }
        for (int i = 0; i < childrenCount; i++)
        {
            if (go.transform.GetChild(i).GetComponent(component) == true)
            {
                return true;
            }
            else
            {
                continue;
            }
        }
        return false;
    }


    static public GameObject GetNearestGameObjectWithTag(Transform posToSearchFrom, string tagName, float maxSearchDistance)
    {
        GameObject closestTarget = null;
        GameObject[] possibleTargets;
        possibleTargets = GameObject.FindGameObjectsWithTag(tagName);
        float distance = maxSearchDistance;
        Vector3 position = posToSearchFrom.position;
        foreach (GameObject currentTargetBeingEvaluated in possibleTargets)
        {
            Vector3 diff = currentTargetBeingEvaluated.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closestTarget = currentTargetBeingEvaluated;
                distance = curDistance;
            }
        }
        return closestTarget;
    }

    static public GameObject GetNearestGameObjectWithTag(Transform posToSearchFrom, string tagName,
        float maxSearchDistance, string componentToExcludeFromSearch)
    {
        GameObject closestTarget = null;
        GameObject[] possibleTargets;
        possibleTargets = GameObject.FindGameObjectsWithTag(tagName);
        float distance = maxSearchDistance;
        Vector3 position = posToSearchFrom.position;
        foreach (GameObject currentTargetBeingEvaluated in possibleTargets)
        {
            if (IterateThroughChildrenLookingForComponent(currentTargetBeingEvaluated, componentToExcludeFromSearch) == true)
            {
                continue; //if the examined GO has that component, then skip it.
            }
            Vector3 diff = currentTargetBeingEvaluated.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closestTarget = currentTargetBeingEvaluated;
                distance = curDistance;
            }
        }
        return closestTarget;
    }

    static public GameObject GetNearestGameObjectOnLayer(Transform posToSearchFrom, string layerName)
    {
        int layerMask = LayerMask.NameToLayer(layerName);
        float distance = Mathf.Infinity;
        GameObject closestTarget = null;
        RaycastHit2D[] hitColliders = Physics2D.CircleCastAll(posToSearchFrom.position, distance, Vector2.up, 0f, layerMask, -1f, 1f);
        foreach (RaycastHit2D contact in hitColliders)
        {
            GameObject go = contact.transform.gameObject;
            float evaluatedObjectDistance = (go.transform.position - posToSearchFrom.position).sqrMagnitude;
            if (evaluatedObjectDistance <= distance)
            {
                distance = evaluatedObjectDistance;
                closestTarget = go;
            }
        }
        return closestTarget;
    }

    static public GameObject GetNearestGameObjectOnLayer(Transform posToSearchFrom, string layerName, float maxSearchDistance)
    {
        int layerMask = LayerMask.NameToLayer(layerName);
        float distance = maxSearchDistance;
        GameObject closestTarget = null;
        RaycastHit2D[] hitColliders = Physics2D.CircleCastAll(posToSearchFrom.position, maxSearchDistance, Vector2.up, 0f, layerMask, -1f, 1f);
        foreach (RaycastHit2D contact in hitColliders)
        {
            GameObject go = contact.transform.gameObject;
            float evaluatedObjectDistance = (go.transform.position - posToSearchFrom.position).sqrMagnitude;
            if (evaluatedObjectDistance <= distance)
            {
                distance = evaluatedObjectDistance;
                closestTarget = go;
            }
        }
        return closestTarget;
    }
    static public GameObject GetNearestGameObjectOnLayer(Transform posToSearchFrom, int layerMask)
    {
        float distance = Mathf.Infinity;
        GameObject closestTarget = null;
        RaycastHit2D[] hitColliders = Physics2D.CircleCastAll(posToSearchFrom.position, distance, Vector2.up, 0f, layerMask, -1f, 1f);
        foreach (RaycastHit2D contact in hitColliders)
        {
            GameObject go = contact.transform.gameObject;
            float evaluatedObjectDistance = (go.transform.position - posToSearchFrom.position).sqrMagnitude;
            if (evaluatedObjectDistance <= distance)
            {
                distance = evaluatedObjectDistance;
                closestTarget = go;
            }
        }
        return closestTarget; ;
    }

    static public Vector2 GetPointOnUnitCircleCircumference()
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
        return new Vector2(Mathf.Sin(randomAngle), Mathf.Cos(randomAngle)).normalized;
    }
    static public GameObject GetNearestGameObjectOnLayer(Transform posToSearchFrom, int layerIndex, float maxSearchDistance)
    {
        int layerMask = 1 << layerIndex;
        //Debug.Log(LayerMask.LayerToName(layerIndex));
        float distance = maxSearchDistance;
        GameObject closestTarget = null;
        RaycastHit2D[] hitColliders = Physics2D.CircleCastAll(posToSearchFrom.position, maxSearchDistance, Vector2.up, 0f, layerMask, -1f, 1f);
        foreach (RaycastHit2D contact in hitColliders)
        {
            GameObject go = contact.transform.gameObject;
            float evaluatedObjectDistance = (go.transform.position - posToSearchFrom.position).sqrMagnitude;
            if (evaluatedObjectDistance <= distance)
            {
                distance = evaluatedObjectDistance;
                closestTarget = go;
            }
        }
        return closestTarget; ;
    }
}

