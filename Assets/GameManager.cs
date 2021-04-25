using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    //init
    GameObject player;
    [SerializeField] GameObject playerPrefab = null;
    SceneLoader sl;
    CityManager cm;
    AllegianceManager am;

    private void Awake()
    {
        int count = FindObjectsOfType<GameManager>().Length;
        if (count > 1)
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
                Camera.main.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
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
    }

    public void InitializePlayerInArena()
    {
        int playerIFF = am.playerAllegiance;
        am.AddFactionLeaderToList(playerIFF, player.GetComponent<FactionLeader>());
        player.GetComponent<PlayerInput>().ReinitializePlayer();
        player.GetComponent<IFF>().SetIFFAllegiance(playerIFF);
        cm = FindObjectOfType<CityManager>(); //City Manager won't exist on start scene
        //Debug.Log($"{cm} was asked for number of cities and reported {cm.GetNumberOfCitySquares()}");
        int randomCity = Random.Range(0, cm.GetNumberOfCitySquares());
        CitySquare playerCity = cm.GetCitySquare(randomCity);
        //Debug.Log($"Player city is {playerCity.name}");
        playerCity.GetComponentInChildren<IFF>().SetIFFAllegiance(playerIFF);
        player.transform.position = playerCity.transform.position;
        Camera.main.GetComponentInChildren<CinemachineVirtualCamera>().Follow = player.transform;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
