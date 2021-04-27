using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System;

public class GameManager : MonoBehaviour
{
    //init
    public GameObject player;
    [SerializeField] GameObject playerPrefab = null;
    SceneLoader sl;
    CityManager cm;
    AllegianceManager am;

    private void Awake()
    {
        int count = FindObjectsOfType<GameManager>().Length;
        if (count > 1)
        {
            Debug.Log("destroying the GM");
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

        sl = FindObjectOfType<SceneLoader>();
        am = FindObjectOfType<AllegianceManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
        {
            player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;            
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))  //for those development times where i start in Arena scene
            {
                Camera.main.GetComponentInChildren<CinemachineVirtualCamera>().Follow = player.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene == SceneManager.GetSceneByBuildIndex(0)) { return; }  //don't call rest of this method if still on starting screen


        InitializePlayerInArena();
        InitializeAIsInArena();
    }        

    private void InitializeAIsInArena()
    {
        am.PopulateFactionLeaders();  //TODO: this role should be performed by the Game Manager. NEed to move the prefabs over. AM should only serve, not instantiate
        int factions = am.GetNumberOfFactionsIncludingFeral();
        for (int i = 1; i <= factions-1; i++)
        {
            GameObject thisLeader = am.GetFactionLeader(i).gameObject;
            CitySquare startingCS = cm.FindNearestCitySquare(transform, i);
            Vector3 startingPos = startingCS.transform.position;
            thisLeader.transform.position = startingPos;
            startingCS.SetAllegianceForBuildingsInCity(i);
        }
    }

    public void InitializePlayerInArena()
    {
        int playerIFF = am.GetPlayerIFF();
        am.AddFactionLeaderToList(playerIFF, player.GetComponent<FactionLeader>());
        player.GetComponent<PlayerInput>().ReinitializePlayer();
        player.GetComponent<IFF>().SetIFFAllegiance(playerIFF);
        cm = FindObjectOfType<CityManager>(); //City Manager won't exist on start scene
        CitySquare playerCity = cm.FindNearestCitySquare(transform, playerIFF);
        playerCity.GetComponentInChildren<IFF>().SetIFFAllegiance(playerIFF);
        player.transform.position = playerCity.transform.position;
        Camera.main.GetComponentInChildren<CinemachineVirtualCamera>().Follow = player.transform;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
