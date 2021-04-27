using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllegianceManager : MonoBehaviour
{
    //Init
    SceneLoader sl;
    [SerializeField] GameObject dummyFactionLeaderPrefab = null;
    [SerializeField] GameObject actualFactionLeaderPrefab = null;
    [SerializeField] Sprite[] flagSource = null;
    public SortedList<int, FactionLeader> factionLeaders = new SortedList<int, FactionLeader>();    

    //hood
    private int playerAllegiance = -1;

    private void Awake()
    {
        int amCount = FindObjectsOfType<AllegianceManager>().Length;
        if (amCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {

        sl = FindObjectOfType<SceneLoader>();
    }
    



    // Update is called once per frame
    void Update()
    {

    }

    public int GetNumberOfFactionsIncludingFeral()
    {
        return flagSource.Length;
    }
    public void PopulateFactionLeaders()
    {
        GameObject dummyFeralFL = Instantiate(dummyFactionLeaderPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        for (int i = 1; i <= flagSource.Length; i++)
        {
            GetFactionLeader(i);
        }
        
    }

    public int GetPlayerIFF()
    {
        return playerAllegiance;
    }

    public void SetPlayerIFF(int newIff)
    {
        playerAllegiance = newIff;
    }
    public void AddFactionLeaderToList(int allegiance, FactionLeader fl) //call this when the Arena scene is loaded.
    {
        if (factionLeaders.ContainsKey(allegiance))
        {
            Debug.Log("faction " + allegiance + " already has a leader!");
        }
        else
        {
            Debug.Log($"added {fl.transform.gameObject} for faction " + allegiance + ".");
            factionLeaders.Add(allegiance, fl);
        }
    }

    public void ReplaceFactionLeaderOnList(int allegiance, FactionLeader newFL)
    {
        if (factionLeaders.ContainsKey(allegiance))
        {
            factionLeaders.Remove(allegiance);
            AddFactionLeaderToList(allegiance, newFL);
        }
    }

    public FactionLeader GetFactionLeader(int iffAllegiance)
    {
        Debug.Log($"I was asked for the faction leader for {iffAllegiance}");
        if (!factionLeaders.ContainsKey(iffAllegiance))
        {
            Debug.Log($"A faction leader doesn't exist for {iffAllegiance}. Creating a dummy");
            GameObject newLeader = Instantiate(actualFactionLeaderPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            newLeader.GetComponentInChildren<IFF>().SetIFFAllegiance(iffAllegiance);
            factionLeaders[iffAllegiance] = newLeader.GetComponent<FactionLeader>();
            return factionLeaders[iffAllegiance];
        }
        else
        {
            FactionLeader fl = factionLeaders[iffAllegiance];
            return fl;
        }

    }

    public Sprite GetFlagOfAllegiance(int key)
    {
        return flagSource[key];
    }



}
