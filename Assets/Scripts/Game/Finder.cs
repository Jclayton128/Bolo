using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Finder : object
{
    public static List<GameObject> FindAllGameObjectsWithinSearchRange(Transform sourceTransform, float searchRange)
    {
        List<GameObject> possibleTargets = new List<GameObject>();
        GameObject[] possibleTargets_array = Object.FindObjectsOfType<GameObject>();
        if (possibleTargets_array.Length <= 0)
        {
            Debug.Log("no targets within search range");
            return null;
        }
        for (int i = 0; i < possibleTargets_array.Length; i++)
        {
            float dist = (sourceTransform.position - possibleTargets_array[i].transform.position).magnitude;
            if (dist <= searchRange)
            {
                GameObject go = possibleTargets_array[i];
                possibleTargets.Add(go);
            }
        }
        return possibleTargets;
    }

    public static List<GameObject> FindAllGameObjectsWithinSearchRange(Transform sourceTransform, float searchRange, string desiredTag)
    {

        List<GameObject> possibleTargets = new List<GameObject>();
        GameObject[] possibleTargets_array = Object.FindObjectsOfType<GameObject>();
        if (possibleTargets_array.Length <= 0)
        {
            Debug.Log("no targets within search range");
            return null;
        }
        for (int i = 0; i < possibleTargets_array.Length; i++)
        {
            if (possibleTargets_array[i].CompareTag(desiredTag))
            {
                Debug.Log("ignore object with tag: " + possibleTargets_array[i].tag);
                continue;
            }
            float dist = (sourceTransform.position - possibleTargets_array[i].transform.position).magnitude;
            if (dist <= searchRange)
            {
                GameObject go = possibleTargets_array[i];
                possibleTargets.Add(go);
            }
        }
        return possibleTargets;
    }
    public static GameObject FindNearestGameObjectWithTag(Transform sourceTransform, string tagName)
    {
        GameObject closestTarget = null;
        GameObject[] possibleTargets;
        possibleTargets = GameObject.FindGameObjectsWithTag(tagName);
        float distance = Mathf.Infinity;
        Vector3 position = sourceTransform.position;
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
    public static GameObject FindNearestGameObjectWithTag(Vector3 pos, string tagName)
    {
        GameObject closestTarget = null;
        GameObject[] possibleTargets;
        possibleTargets = GameObject.FindGameObjectsWithTag(tagName);
        float distance = Mathf.Infinity;
        Vector3 position = pos;
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

    public static GameObject FindNearestGameObjectWithTag(Transform sourceTransform, string tagName, float maxSearchDistance)
    {
        GameObject closestTarget = null;
        GameObject[] possibleTargets;
        possibleTargets = GameObject.FindGameObjectsWithTag(tagName);
        float distance = maxSearchDistance;
        Vector3 position = sourceTransform.position;
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
    public static GameObject FindNearestGameObjectWithTag(Vector3 pos, string tagName, float maxSearchDistance)
    {
        GameObject closestTarget = null;
        GameObject[] possibleTargets;
        possibleTargets = GameObject.FindGameObjectsWithTag(tagName);
        float distance = maxSearchDistance;
        Vector3 position = pos;
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

    public static GameObject FindNearestGameObjectOnLayer(Transform sourceTransform, string layerName)
    {
        int layerMask = LayerMask.NameToLayer(layerName);
        float distance = Mathf.Infinity;
        GameObject closestTarget = null;
        RaycastHit2D[] hitColliders = Physics2D.CircleCastAll(sourceTransform.position, distance, Vector2.up, 0f, layerMask, -1f, 1f);
        foreach (RaycastHit2D contact in hitColliders)
        {
            GameObject go = contact.transform.gameObject;
            float evaluatedObjectDistance = (go.transform.position - sourceTransform.position).sqrMagnitude;
            if (evaluatedObjectDistance <= distance)
            {
                distance = evaluatedObjectDistance;
                closestTarget = go;
            }
        }
        return closestTarget;
    }

    public static GameObject FindNearestGameObjectOnLayer(Transform sourceTransform, string layerName, float maxSearchDistance)
    {
        int layerMask = LayerMask.NameToLayer(layerName);
        float distance = maxSearchDistance;
        GameObject closestTarget = null;
        RaycastHit2D[] hitColliders = Physics2D.CircleCastAll(sourceTransform.position, maxSearchDistance, Vector2.up, 0f, layerMask, -1f, 1f);
        foreach (RaycastHit2D contact in hitColliders)
        {
            GameObject go = contact.transform.gameObject;
            float evaluatedObjectDistance = (go.transform.position - sourceTransform.position).sqrMagnitude;
            if (evaluatedObjectDistance <= distance)
            {
                distance = evaluatedObjectDistance;
                closestTarget = go;
            }
        }
        return closestTarget;
    }
    public static GameObject FindNearestGameObjectOnLayer(Transform sourceTransform, int layerIndex)
    {
        int layerMask = 1 << layerIndex;
        float distance = Mathf.Infinity;
        GameObject closestTarget = null;
        RaycastHit2D[] hitColliders = Physics2D.CircleCastAll(sourceTransform.position, distance, Vector2.up, 0f, layerMask, -1f, 1f);
        foreach (RaycastHit2D contact in hitColliders)
        {
            GameObject go = contact.transform.gameObject;
            float evaluatedObjectDistance = (go.transform.position - sourceTransform.position).sqrMagnitude;
            if (evaluatedObjectDistance <= distance)
            {
                distance = evaluatedObjectDistance;
                closestTarget = go;
            }
        }
        return closestTarget; ;
    }
    public static GameObject FindNearestGameObjectOnLayer(Transform sourceTransform, int layerIndex, float maxSearchDistance)
    {
        int layerMask = 1 << layerIndex;
        //Debug.Log(LayerMask.LayerToName(layerIndex));
        float distance = maxSearchDistance;
        GameObject closestTarget = null;
        RaycastHit2D[] hitColliders = Physics2D.CircleCastAll(sourceTransform.position, maxSearchDistance, Vector2.up, 0f, layerMask, -1f, 1f);
        foreach (RaycastHit2D contact in hitColliders)
        {
            GameObject go = contact.transform.gameObject;
            float evaluatedObjectDistance = (go.transform.position - sourceTransform.position).sqrMagnitude;
            if (evaluatedObjectDistance <= distance)
            {
                distance = evaluatedObjectDistance;
                closestTarget = go;
            }
        }
        return closestTarget; ;
    }

}
