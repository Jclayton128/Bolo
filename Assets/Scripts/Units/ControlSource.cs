using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(IFF))]
public abstract class ControlSource : MonoBehaviour
{
    // Control Source class is intended to be the top-level of a unit. It is either the interaction level with the player, or
    // the level where an AI-controlled unit's inputs come from.

    //init
    protected GameObject targetGO;
    protected Movement move;
    protected Attack attack;
    protected UnitTracker ut;
    protected IFF iff;
    protected CityManager cm;
    protected AllegianceManager am;
    protected Health health;
    protected NavMeshAgent nma;


    //param
    public float scanRange = 3f;
    protected float timeBetweenScans = 0.2f;

    //hood
    public float horizComponent { get; protected set; }
    public float vertComponent { get; protected set; }
    public int speedSetting { get; protected set; } = 1;

    protected float timeSinceLastScan = 0;

    protected virtual void Start()
    {
        ut = FindObjectOfType<UnitTracker>();
        ut.AddUnitToTargetableList(gameObject);
        move = GetComponentInChildren<Movement>();
        attack = GetComponentInChildren<Attack>();
        targetGO = GameObject.FindGameObjectWithTag("Player");
        iff = GetComponent<IFF>();
        cm = FindObjectOfType<CityManager>();
        health = GetComponentInChildren<Health>();
        am = FindObjectOfType<AllegianceManager>();
        if (TryGetComponent<NavMeshAgent>(out nma))
        {
            nma.updateRotation = false;
        } 

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        timeSinceLastScan -= Time.deltaTime;
        if (timeSinceLastScan <= 0)
        {
            Scan();
            timeSinceLastScan = timeBetweenScans;
        }
    }

    protected virtual void OnDestroy()
    {
        ut.RemoveUnitFromTargetableList(gameObject);
    }

    public virtual GameObject GetTargetObject()
    {
        return targetGO;
    }

    protected abstract void Scan();

    public static void DebugDrawPath(Vector3[] corners)
    {
        if (corners.Length < 2) { return; }
        int i = 0;
        for (; i < corners.Length - 1; i++)
        {
            Debug.DrawLine(corners[i], corners[i + 1], Color.blue);
        }
        Debug.DrawLine(corners[0], corners[1], Color.red);
    }

}
